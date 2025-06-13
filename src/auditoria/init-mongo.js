// Inicialização do MongoDB para Sistema de Auditoria
// Este script cria as collections e índices necessários para auditoria

// Conectar ao banco de dados de auditoria
db = db.getSiblingDB('auditoria_db');

// Criar collection para logs de recomendações
db.createCollection('recommendation_logs', {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["timestamp", "client_id", "request_data", "response_data"],
            properties: {
                timestamp: {
                    bsonType: "date",
                    description: "Timestamp da requisição"
                },
                client_id: {
                    bsonType: "string",
                    description: "ID do cliente que fez a requisição"
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
                    bsonType: ["number", "null"],
                    description: "Tempo de processamento em milissegundos"
                },
                api_version: {
                    bsonType: ["string", "null"],
                    description: "Versão da API que processou a requisição"
                },
                request_ip: {
                    bsonType: ["string", "null"],
                    description: "IP de origem da requisição"
                }
            }
        }
    }
});

// Criar índices para otimizar consultas
db.recommendation_logs.createIndex({ "timestamp": -1 });
db.recommendation_logs.createIndex({ "client_id": 1 });
db.recommendation_logs.createIndex({ "response_data.ClusterAssignado": 1 });
db.recommendation_logs.createIndex({ "response_data.PerfilCalculadoCliente": 1 });
db.recommendation_logs.createIndex({ "timestamp": -1, "client_id": 1 });

// Criar collection para métricas agregadas (opcional, para análises futuras)
db.createCollection('recommendation_metrics', {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["date", "total_requests"],
            properties: {
                date: {
                    bsonType: "date",
                    description: "Data das métricas (agregado diário)"
                },
                total_requests: {
                    bsonType: "int",
                    description: "Total de requisições no dia"
                },
                requests_by_profile: {
                    bsonType: "object",
                    description: "Contadores por perfil de cliente"
                },
                requests_by_cluster: {
                    bsonType: "object", 
                    description: "Contadores por cluster"
                },
                avg_processing_time_ms: {
                    bsonType: ["number", "null"],
                    description: "Tempo médio de processamento"
                },
                unique_clients: {
                    bsonType: "int",
                    description: "Número de clientes únicos no dia"
                }
            }
        }
    }
});

db.recommendation_metrics.createIndex({ "date": -1 }, { unique: true });

// Criar usuário específico para auditoria
db.createUser({
    user: "auditoria_user",
    pwd: "auditoria_pass_2025",
    roles: [
        { role: "readWrite", db: "auditoria_db" }
    ]
});

print("MongoDB inicializado com sucesso!");
print("Collections criadas:");
print("- recommendation_logs: Para logs de requisições");
print("- recommendation_metrics: Para métricas agregadas");
print("Usuário criado: auditoria_user");
print("Índices criados para otimização de consultas");
