# Sistema de Auditoria MongoDB

Este diretório contém a configuração do MongoDB para auditoria das requisições da API de recomendações.

## Estrutura

- `init-mongo.js`: Script de inicialização que cria as collections, índices e usuários necessários
- Collections criadas:
  - `recommendation_logs`: Armazena todos os logs de requisições da rota /recomendacoes
  - `recommendation_metrics`: Para métricas agregadas e análises

## Collections

### recommendation_logs
Estrutura para armazenar cada requisição:
```json
{
  "timestamp": "Data/hora da requisição",
  "client_id": "ID do cliente",
  "request_data": {
    "ClienteId": "string",
    "Ativos": "array",
    "ClassificacaoDeclarada": "string"
  },
  "response_data": {
    "ClienteIdSolicitado": "string",
    "PerfilCalculadoCliente": "string", 
    "PerfilDeclaradoCliente": "string",
    "ClusterAssignado": "int",
    "TipoAtivoSugeridoAjuste": "string|null",
    "AtivosRecomendados": "array",
    "Timestamp": "string"
  },
  "processing_time_ms": "number|null",
  "api_version": "string|null",
  "request_ip": "string|null"
}
```

### recommendation_metrics
Estrutura para métricas agregadas diárias:
```json
{
  "date": "Data das métricas",
  "total_requests": "Total de requisições no dia",
  "requests_by_profile": "Contadores por perfil",
  "requests_by_cluster": "Contadores por cluster",
  "avg_processing_time_ms": "Tempo médio de processamento",
  "unique_clients": "Clientes únicos no dia"
}
```

## Índices Criados
- `timestamp` (descendente) - Para consultas cronológicas
- `client_id` - Para buscar histórico de cliente específico
- `response_data.ClusterAssignado` - Para análises por cluster
- `response_data.PerfilCalculadoCliente` - Para análises por perfil
- Índice composto `timestamp + client_id` - Para consultas otimizadas

## Usuário
- **Usuário**: auditoria_user
- **Senha**: auditoria_pass_2025
- **Permissões**: readWrite no banco auditoria_db
   docker-compose up -d --build
   ```

3. **Verificar se está funcionando**:
   ```bash
   docker-compose logs mongodb_audit
   ```

4. **Conectar ao MongoDB**:
   ```bash
   docker exec -it btg_mongodb_audit mongosh -u admin -p btg_audit_2025 --authenticationDatabase admin
   ```

## Credenciais

- **Admin**: admin / btg_audit_2025
- **Audit User**: audit_user / audit_pass_2025
- **Database**: btg_auditoria

## Collections Criadas

### 1. recomendacoes_audit
- **Propósito**: Armazenar todas as requisições/respostas da rota `/recomendacoes`
- **Schema**: Validação completa dos dados de entrada e saída
- **Índices**: Otimizados para consultas por cliente, data, cluster, perfil

### 2. audit_metrics
- **Propósito**: Métricas agregadas e estatísticas
- **Tipos**: daily_requests, cluster_distribution, profile_mismatch, processing_time_avg

## Funcionalidades

- ✅ Schema validation para integridade dos dados
- ✅ Índices otimizados para performance
- ✅ TTL para limpeza automática (2 anos de retenção)
- ✅ Usuário específico para auditoria
- ✅ Health check configurado
- ✅ Volumes persistentes

## Parar e Limpar

```bash
# Parar
docker-compose down

# Parar e remover volumes (CUIDADO: apaga dados)
docker-compose down -v
```
