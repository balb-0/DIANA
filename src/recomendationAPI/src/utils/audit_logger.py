"""
Módulo de Auditoria para MongoDB
Responsável por logar todas as requisições e respostas do endpoint /recomendacoes
"""

import os
import json
from datetime import datetime
from typing import Dict, Any, Optional
from pymongo import MongoClient
from pymongo.collection import Collection
from pymongo.database import Database
import logging

# Configurar logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

class AuditLogger:
    """
    Classe responsável por gerenciar a auditoria das requisições/respostas
    no MongoDB para compliance e rastreabilidade.
    """
    
    def __init__(self):
        """Inicializa o cliente MongoDB e conecta à base de auditoria"""
        self.client: Optional[MongoClient] = None
        self.db: Optional[Database] = None
        self.logs_collection: Optional[Collection] = None
        self.metrics_collection: Optional[Collection] = None
        self._connect_to_mongodb()
    
    def _connect_to_mongodb(self):
        """Estabelece conexão com MongoDB usando variáveis de ambiente"""
        try:            # Configurações do MongoDB via env vars (docker-compose)
            mongo_host = os.getenv('MONGO_HOST', 'btg_mongodb_auditoria')
            mongo_port = int(os.getenv('MONGO_PORT', '27017'))
            mongo_username = os.getenv('MONGO_USERNAME', 'audit_user')
            mongo_password = os.getenv('MONGO_PASSWORD', 'audit_pass_2025')
            mongo_database = os.getenv('MONGO_DATABASE', 'auditoria_db')
            
            # String de conexão
            mongo_uri = f"mongodb://{mongo_username}:{mongo_password}@{mongo_host}:{mongo_port}/{mongo_database}"
            
            # Conectar ao MongoDB
            self.client = MongoClient(mongo_uri, serverSelectionTimeoutMS=5000)
            
            # Testar conexão
            self.client.admin.command('ping')
            
            # Acessar database e collections
            self.db = self.client[mongo_database]
            self.logs_collection = self.db['recommendation_logs']
            self.metrics_collection = self.db['recommendation_metrics']
            
            logger.info("✅ Conexão com MongoDB de auditoria estabelecida com sucesso")
            
        except Exception as e:
            logger.error(f"❌ Erro ao conectar com MongoDB de auditoria: {e}")
            # Em caso de falha, o sistema deve continuar funcionando
            self.client = None
            self.db = None
            self.logs_collection = None
            self.metrics_collection = None
    
    def is_connected(self) -> bool:
        """Verifica se a conexão com MongoDB está ativa"""
        return self.client is not None and self.logs_collection is not None
    
    def log_recommendation_request(
        self, 
        cliente_id: str, 
        request_data: Dict[str, Any], 
        response_data: Dict[str, Any],
        processing_time_ms: float,
        status_code: int = 200,
        error_message: Optional[str] = None
    ) -> bool:
        """
        Registra uma requisição/resposta de recomendação no MongoDB
        
        Args:
            cliente_id: ID do cliente que fez a requisição
            request_data: Dados da requisição (JSON serializable)
            response_data: Dados da resposta (JSON serializable)
            processing_time_ms: Tempo de processamento em milissegundos
            status_code: Código de status HTTP (200, 500, etc)
            error_message: Mensagem de erro, se houver
        
        Returns:
            bool: True se log foi salvo com sucesso, False caso contrário
        """
        if not self.is_connected():
            logger.warning("MongoDB não conectado - auditoria não será registrada")
            return False
        
        try:            # Preparar documento de log
            log_document = {
                "timestamp": datetime.utcnow(),
                "client_id": cliente_id,
                "endpoint": "/recomendacoes",
                "method": "POST",
                "status_code": status_code,
                "processing_time_ms": processing_time_ms,
                "request_data": {
                    "ClienteId": request_data.get("ClienteId"),
                    "Ativos": request_data.get("Ativos", []),
                    "ClassificacaoDeclarada": request_data.get("ClassificacaoDeclarada"),
                    "total_ativos": len(request_data.get("Ativos", [])),
                    "valor_total_investido": sum([ativo.get("ValorInvestido", 0) for ativo in request_data.get("Ativos", [])])
                },
                "response_data": response_data if status_code == 200 else None,
                "error": error_message if error_message else None,
                "metadata": {
                    "cluster_assignado": response_data.get("ClusterAssignado") if status_code == 200 else None,
                    "perfil_calculado": response_data.get("PerfilCalculadoCliente") if status_code == 200 else None,
                    "perfil_declarado": response_data.get("PerfilDeclaradoCliente") if status_code == 200 else None,
                    "total_recomendacoes": len(response_data.get("AtivosRecomendados", [])) if status_code == 200 else 0
                }
            }
            
            # Inserir no MongoDB
            result = self.logs_collection.insert_one(log_document)
            
            if result.inserted_id:
                logger.info(f"✅ Log de auditoria salvo para cliente {cliente_id} - ID: {result.inserted_id}")
                
                # Atualizar métricas agregadas
                self._update_metrics(cliente_id, status_code, processing_time_ms, log_document)
                
                return True
            else:
                logger.error("❌ Falha ao inserir log no MongoDB")
                return False
                
        except Exception as e:
            logger.error(f"❌ Erro ao salvar log de auditoria: {e}")
            return False
    
    def _update_metrics(self, cliente_id: str, status_code: int, processing_time_ms: float, log_document: Dict[str, Any]):
        """Atualiza métricas agregadas na collection de métricas"""
        try:
            today = datetime.utcnow().date()
            
            # Documento de métricas para o dia
            metrics_filter = {
                "date": datetime.combine(today, datetime.min.time()),
                "cliente_id": cliente_id
            }
            
            # Preparar update com operadores MongoDB
            metrics_update = {
                "$inc": {
                    "total_requests": 1,
                    "successful_requests": 1 if status_code == 200 else 0,
                    "error_requests": 1 if status_code != 200 else 0,
                    "total_processing_time_ms": processing_time_ms
                },
                "$push": {
                    "request_times": processing_time_ms
                },
                "$set": {
                    "last_request_timestamp": log_document["timestamp"],
                    "last_cluster_assignado": log_document["metadata"].get("cluster_assignado"),
                    "last_perfil_calculado": log_document["metadata"].get("perfil_calculado")
                }
            }
            
            # Upsert (insert se não existir, update se existir)
            self.metrics_collection.update_one(
                metrics_filter,
                metrics_update,
                upsert=True
            )
            
            logger.debug(f"Métricas atualizadas para cliente {cliente_id}")
            
        except Exception as e:
            logger.error(f"Erro ao atualizar métricas: {e}")
    
    def get_client_audit_summary(self, cliente_id: str, days: int = 30) -> Dict[str, Any]:
        """
        Retorna resumo de auditoria para um cliente específico
        
        Args:
            cliente_id: ID do cliente
            days: Número de dias para buscar (padrão: 30)
        
        Returns:
            Dict com resumo de auditoria do cliente
        """
        if not self.is_connected():
            return {"error": "MongoDB não conectado"}
        
        try:
            # Data limite
            from_date = datetime.utcnow() - datetime.timedelta(days=days)
            
            # Buscar logs do cliente
            logs = list(self.logs_collection.find({
                "cliente_id": cliente_id,
                "timestamp": {"$gte": from_date}
            }).sort("timestamp", -1))
            
            # Calcular estatísticas
            total_requests = len(logs)
            successful_requests = len([log for log in logs if log.get("status_code") == 200])
            avg_processing_time = sum([log.get("processing_time_ms", 0) for log in logs]) / total_requests if total_requests > 0 else 0
            
            return {
                "cliente_id": cliente_id,
                "period_days": days,
                "total_requests": total_requests,
                "successful_requests": successful_requests,
                "error_requests": total_requests - successful_requests,
                "success_rate": (successful_requests / total_requests * 100) if total_requests > 0 else 0,
                "avg_processing_time_ms": round(avg_processing_time, 2),
                "last_request": logs[0]["timestamp"].isoformat() if logs else None
            }
            
        except Exception as e:
            logger.error(f"Erro ao gerar resumo de auditoria: {e}")
            return {"error": str(e)}
    
    def close_connection(self):
        """Fecha a conexão com MongoDB"""
        if self.client:
            self.client.close()
            logger.info("Conexão com MongoDB de auditoria fechada")

# Instância global do audit logger
audit_logger = AuditLogger()
