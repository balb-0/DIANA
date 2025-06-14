"""
Middleware de Auditoria para FastAPI
Intercepta todas as requisições para o endpoint /recomendacoes e registra no MongoDB
"""

import time
import json
from typing import Callable
from fastapi import Request, Response
from fastapi.routing import APIRoute
from starlette.middleware.base import BaseHTTPMiddleware
import logging

from utils.audit_logger import audit_logger

logger = logging.getLogger(__name__)

class AuditMiddleware(BaseHTTPMiddleware):
    """
    Middleware para interceptar e auditar requisições/respostas do endpoint /recomendacoes
    """
    
    async def dispatch(self, request: Request, call_next: Callable) -> Response:
        """
        Processa a requisição, chama o endpoint e audita o resultado
        """
        # Verificar se é o endpoint de recomendações
        if request.url.path != "/recomendacoes" or request.method != "POST":
            # Para outros endpoints, apenas processar normalmente
            return await call_next(request)
        
        # Capturar tempo de início
        start_time = time.time()
        
        # Capturar dados da requisição
        request_body = None
        cliente_id = "unknown"
        
        try:
            # Ler body da requisição
            body = await request.body()
            if body:
                request_body = json.loads(body.decode('utf-8'))
                cliente_id = request_body.get("ClienteId", "unknown")
            
            # Recriar request com body (já que body foi consumido)
            request._body = body
            
        except Exception as e:
            logger.error(f"Erro ao capturar dados da requisição: {e}")
            request_body = {"error": "Failed to parse request body"}
        
        # Processar requisição
        response = None
        response_data = None
        status_code = 500
        error_message = None
        
        try:
            response = await call_next(request)
            status_code = response.status_code
            
            # Capturar dados da resposta
            if response.status_code == 200:
                # Ler body da resposta
                response_body = b""
                async for chunk in response.body_iterator:
                    response_body += chunk
                
                # Parse JSON da resposta
                if response_body:
                    response_data = json.loads(response_body.decode('utf-8'))
                
                # Recriar response
                from fastapi import Response as FastAPIResponse
                response = FastAPIResponse(
                    content=response_body,
                    status_code=response.status_code,
                    headers=dict(response.headers),
                    media_type="application/json"
                )
            else:
                error_message = f"HTTP {response.status_code}"
                response_data = {"error": error_message}
                
        except Exception as e:
            logger.error(f"Erro durante processamento da requisição: {e}")
            error_message = str(e)
            status_code = 500
            response_data = {"error": error_message}
            
            # Criar resposta de erro
            from fastapi import Response as FastAPIResponse
            response = FastAPIResponse(
                content=json.dumps({"error": error_message}),
                status_code=500,
                media_type="application/json"
            )
        
        # Calcular tempo de processamento
        processing_time_ms = (time.time() - start_time) * 1000
        
        # Auditar no MongoDB (de forma assíncrona, não bloquear resposta)
        try:
            success = audit_logger.log_recommendation_request(
                cliente_id=cliente_id,
                request_data=request_body or {},
                response_data=response_data or {},
                processing_time_ms=processing_time_ms,
                status_code=status_code,
                error_message=error_message
            )
            
            if success:
                logger.info(f"Auditoria registrada para cliente {cliente_id}")
            else:
                logger.warning(f"Falha ao registrar auditoria para cliente {cliente_id}")
                
        except Exception as e:
            logger.error(f"Erro crítico na auditoria: {e}")
            # Não falhar a requisição por causa da auditoria
        
        return response


class AuditRoute(APIRoute):
    """
    Rota customizada que inclui auditoria automática
    Alternativa ao middleware para controle mais granular
    """
    
    def get_route_handler(self) -> Callable:
        original_route_handler = super().get_route_handler()
        
        async def custom_route_handler(request: Request) -> Response:
            # Apenas para endpoint /recomendacoes
            if request.url.path != "/recomendacoes":
                return await original_route_handler(request)
            
            start_time = time.time()
            cliente_id = "unknown"
            request_data = {}
            
            try:
                # Capturar dados da requisição
                body = await request.body()
                if body:
                    request_data = json.loads(body.decode('utf-8'))
                    cliente_id = request_data.get("ClienteId", "unknown")
                
                # Recriar request
                request._body = body
                
            except Exception as e:
                logger.error(f"Erro ao processar requisição: {e}")
            
            # Processar requisição original
            try:
                response = await original_route_handler(request)
                processing_time_ms = (time.time() - start_time) * 1000
                
                # Se resposta é JSON, extrair dados para auditoria
                response_data = {}
                if hasattr(response, 'body') and response.media_type == "application/json":
                    try:
                        response_data = json.loads(response.body)
                    except:
                        response_data = {"response": "Non-JSON response"}
                
                # Auditar
                audit_logger.log_recommendation_request(
                    cliente_id=cliente_id,
                    request_data=request_data,
                    response_data=response_data,
                    processing_time_ms=processing_time_ms,
                    status_code=200
                )
                
                return response
                
            except Exception as e:
                processing_time_ms = (time.time() - start_time) * 1000
                
                # Auditar erro
                audit_logger.log_recommendation_request(
                    cliente_id=cliente_id,
                    request_data=request_data,
                    response_data={},
                    processing_time_ms=processing_time_ms,
                    status_code=500,
                    error_message=str(e)
                )
                
                raise e
        
        return custom_route_handler
