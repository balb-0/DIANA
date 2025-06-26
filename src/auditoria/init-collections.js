// Script para criação das collections e índices de auditoria

db = db.getSiblingDB('btg_auditoria');

// Collection principal: recomendacoes_audit
// Armazena todas as requisições e respostas da rota /recomendacoes
db.createCollection('recomendacoes_audit', {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["audit_id", "client_id", "timestamp", "request_data", "response_data"],
      properties: {
        audit_id: {
          bsonType: "string",
          description: "ID único da auditoria (UUID)"
        },
        client_id: {
          bsonType: "string",
          description: "ID do cliente que fez a requisição"
        },
        timestamp: {
          bsonType: "date",
          description: "Timestamp da requisição"
        },
        request_data: {
          bsonType: "object",
          description: "Dados completos da requisição",
          properties: {
            ClienteId: { bsonType: "string" },
            Ativos: { bsonType: "array" },
            ClassificacaoDeclarada: { bsonType: "string" }
          }
        },
        response_data: {
          bsonType: "object",
          description: "Dados completos da resposta",
          properties: {
            ClienteIdSolicitado: { bsonType: "string" },
            PerfilCalculadoCliente: { bsonType: "string" },
            PerfilDeclaradoCliente: { bsonType: "string" },
            ClusterAssignado: { bsonType: "int" },
            TipoAtivoSugeridoAjuste: { bsonType: ["string", "null"] },
            AtivosRecomendados: { bsonType: "array" },
            Timestamp: { bsonType: "string" }
          }
        },
        processing_time_ms: {
          bsonType: "double",
          description: "Tempo de processamento em milissegundos"
        },
        api_version: {
          bsonType: "string",
          description: "Versão da API que processou a requisição"
        },
        status: {
          bsonType: "string",
          enum: ["success", "error"],
          description: "Status da requisição"
        },
        error_details: {
          bsonType: ["object", "null"],
          description: "Detalhes do erro se status = error"
        }
      }
    }
  }
});

// Collection secundária: audit_metrics
// Para métricas agregadas e estatísticas
db.createCollection('audit_metrics', {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["metric_type", "date", "value"],
      properties: {
        metric_type: {
          bsonType: "string",
          enum: ["daily_requests", "cluster_distribution", "profile_mismatch", "processing_time_avg"],
          description: "Tipo da métrica"
        },
        date: {
          bsonType: "date",
          description: "Data da métrica"
        },
        value: {
          bsonType: ["double", "int", "object"],
          description: "Valor da métrica"
        },
        metadata: {
          bsonType: ["object", "null"],
          description: "Metadados adicionais da métrica"
        }
      }
    }
  }
});

// Criar índices para performance

// Índices para recomendacoes_audit
db.recomendacoes_audit.createIndex({ "client_id": 1 });
db.recomendacoes_audit.createIndex({ "timestamp": -1 });
db.recomendacoes_audit.createIndex({ "client_id": 1, "timestamp": -1 });
db.recomendacoes_audit.createIndex({ "response_data.ClusterAssignado": 1 });
db.recomendacoes_audit.createIndex({ "response_data.PerfilCalculadoCliente": 1 });
db.recomendacoes_audit.createIndex({ "response_data.PerfilDeclaradoCliente": 1 });
db.recomendacoes_audit.createIndex({ "status": 1 });
db.recomendacoes_audit.createIndex({ "audit_id": 1 }, { unique: true });

// Índices para audit_metrics
db.audit_metrics.createIndex({ "metric_type": 1, "date": -1 });
db.audit_metrics.createIndex({ "date": -1 });

// Criar índice TTL para limpeza automática (opcional - 2 anos de retenção)
db.recomendacoes_audit.createIndex({ "timestamp": 1 }, { expireAfterSeconds: 63072000 }); // 2 anos

print('Collections de auditoria criadas com sucesso!');
print('Índices configurados para otimização de consultas.');
print('Schema validation ativado para garantir integridade dos dados.');
