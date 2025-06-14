# Sistema de Auditoria para API de Recomendações

## OBJETIVO DO SISTEMA DE AUDITORIA
**Meta**: Auditar todas as requisições da rota `/recomendacoes` da API, persistindo dados completos de entrada (request) e saída (response) no MongoDB para análise posterior, compliance e rastreabilidade.

## Benefícios da Auditoria
- **Compliance**: Rastreamento completo de recomendações fornecidas
- **Análise de Padrões**: Identificar tendências e comportamentos
- **Debug e Suporte**: Histórico para investigação de problemas
- **Métricas de Negócio**: Performance e qualidade das recomendações

## IMPLEMENTAÇÃO REALIZADA

### Estrutura de Auditoria
- **Pasta**: `src/auditoria/`
- **Orquestração**: Integrada no docker-compose geral do projeto
- **Database**: MongoDB dedicado para auditoria
- **Network**: btg_network (compartilhada)

# API de Recomendações - Pontos Importantes

## Arquitetura da API
- **Framework**: FastAPI (Python)
- **Porta**: 8000
- **Container**: btg_api_recomendacao
- **Estrutura**: Modular com separação de responsabilidades

## Endpoint Principal para Auditoria
- **Rota**: `POST /recomendacoes`
- **Função**: `api_gerar_recomendacoes_simplificado()`
- **Localização**: `src/main.py` (linha 68-86)
- **Response Model**: `RecomendacaoResponse`

## Estrutura de Request
```python
class RecomendacaoRequest:
    ClienteId: str
    Ativos: List[AtivoClienteInput]  # CodigoAtivo + ValorInvestido
    ClassificacaoDeclarada: str  # Conservador, Moderado ou Sofisticado
```

## Estrutura de Response (dados a auditar)
```python
class RecomendacaoResponse:
    ClienteIdSolicitado: str
    PerfilCalculadoCliente: str
    PerfilDeclaradoCliente: str
    ClusterAssignado: int
    TipoAtivoSugeridoAjuste: Optional[str]
    AtivosRecomendados: List[AtivoRecomendadoOutput]
    Timestamp: str  # ISO format
```

## Lógica de Negócio
1. **Análise de Perfil**: Calcula perfil RF/RV baseado nos ativos do cliente
2. **Clustering**: Usa modelo K-Means para classificar cliente em clusters (0, 1, 2)
3. **Recomendações**: Usa matrizes de similaridade item-item por cluster
4. **Ajuste de Portfolio**: Sugere tipo de ativo para alinhar perfil declarado vs calculado

## Dados Utilizados
- **Barema**: Classificação de ativos (RF/RV, subgrupos)
- **Matrizes de Similaridade**: Por cluster (CSV files)
- **Modelo K-Means**: Arquivo joblib serializado
- **Distribuição de Subgrupos**: Para classificação em clusters

## Dependências Relevantes
- FastAPI, Pandas, NumPy, Scikit-learn, Joblib, Pydantic
- Arquivos CSV de dados processados
- Modelo ML pré-treinado

## Pontos de Integração para Auditoria
- Response completo da rota `/recomendacoes` deve ser persistido
- Timestamp já incluído na response
- ClienteId disponível para rastreamento
- Dados de entrada (request) e saída (response) completos
- Informações de cluster e perfil para análise posterior

## Docker Configuration
- Container isolado no docker-compose
- Volume para dados: `./recomendationAPI/data:/app/data`
- Network: btg_network (compartilhada com outros serviços)