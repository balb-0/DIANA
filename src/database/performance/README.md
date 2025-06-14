# BTG Pactual - Testes de Carga do Banco de Dados

Este diretório contém scripts para realizar testes de carga no banco de dados PostgreSQL do sistema BTG Pactual usando pgbench.

## 📋 Arquivos do Sistema

### 🗄️ Scripts de Teste

- **`pgbench_setup.sql`** - Script de configuração e criação de dados de massa (110.600 registros)
- **`test_queries.sql`** - Consultas SQL otimizadas para teste de carga simulando operações reais
- **`run_pgbench.ps1`** - Script PowerShell principal para executar todos os testes
- **`explain_plans.sql`** - Scripts para análise de planos de execução e otimização
- **`README.md`** - Esta documentação

### 📊 Dados de Teste Criados

| Tabela                    | Registros | Descrição                                      |
| ------------------------- | --------- | ---------------------------------------------- |
| assessores                | 100       | Assessores de teste distribuídos em 4 unidades |
| ativos                    | 500       | Ativos diversos (RF, RV, Fundos, Derivativos)  |
| clientes                  | 10.000    | Clientes com categorias variadas               |
| historico_risco_declarado | 25.000    | Histórico de perfis declarados                 |
| historico_risco_calculado | 25.000    | Histórico de perfis calculados                 |
| investimento              | 50.000    | Investimentos distribuídos entre clientes      |

## 🚀 Como Executar os Testes

### Pré-requisitos

1. **Docker** rodando com PostgreSQL
2. **PostgreSQL client tools** (psql, pgbench) instalados
3. **PowerShell** (Windows)

### Verificação do Ambiente

```powershell
# 1. Verificar se o banco está rodando
docker ps | grep postgres

# 2. Se não estiver rodando, iniciar:
cd c:\Users\Inteli\Desktop\2025-1B-T13-ES06-G03\src
docker-compose up -d

# 3. Aguardar inicialização (30 segundos)
Start-Sleep 30
```

### Executar Testes

```powershell
# Navegar para o diretório
cd c:\Users\Inteli\Desktop\2025-1B-T13-ES06-G03\src\database\performance

# Opção 1: Menu interativo
.\run_pgbench.ps1

# Opção 2: Teste rápido (1 minuto)
.\run_pgbench.ps1 -Quick

# Opção 3: Bateria completa (15 minutos)
.\run_pgbench.ps1 -Full

# Opção 4: Apenas preparar dados
.\run_pgbench.ps1 -Setup
```

## 📈 Interpretando os Resultados

### Métricas Principais

#### TPS (Transactions Per Second)

- **Excelente:** >2000 TPS
- **Bom:** 1000-2000 TPS
- **Aceitável:** 500-1000 TPS
- **Problema:** <500 TPS

#### Latência (ms)

- **Excelente:** <10ms média
- **Bom:** 10-50ms média
- **Aceitável:** 50-100ms média
- **Problema:** >100ms média

#### Percentis de Resposta

- **P95:** 95% das transações abaixo deste tempo
- **P99:** 99% das transações abaixo deste tempo
- **Objetivo:** P95 <100ms, P99 <200ms

#### Taxa de Erro

- **Objetivo:** <0.1%
- **Aceitável:** <1%
- **Problema:** >1%

### Exemplo de Resultado Bom

```
transaction type: test_queries.sql
scaling factor: 1
query mode: simple
number of clients: 25
number of threads: 8
duration: 180 s
number of transactions actually processed: 45230
latency average = 9.923 ms
latency stddev = 15.242 ms
rate limit = 0.000000 (unlimited)
tps = 251.277776 (including connections establishing)
tps = 251.289543 (excluding connections establishing)
```

**Análise:**

- ✅ **TPS:** 251 - Aceitável para queries complexas
- ✅ **Latência:** 9.9ms - Excelente
- ✅ **Estabilidade:** Desvio padrão controlado

### Sinais de Problemas

#### 1. TPS Muito Baixo (<100)

```
Possíveis causas:
- Falta de índices adequados
- Queries mal otimizadas
- Recursos limitados (CPU/Memória)
- Problemas de I/O
```

#### 2. Latência Alta (>100ms)

```
Possíveis causas:
- Sequential scans em tabelas grandes
- Falta de estatísticas atualizadas
- Joins mal otimizados
- Cache hit ratio baixo
```

#### 3. Alta Taxa de Erro

```
Possíveis causas:
- Timeouts de conexão
- Deadlocks
- Problemas de conectividade
- Recursos esgotados
```

## 🔧 Otimizações Sugeridas

### 1. Análise de Queries Lentas

```powershell
# Executar análise de planos de execução
psql -h localhost -U btg_user -d btg_db -f explain_plans.sql
```

### 2. Verificar Cache Hit Ratio

```sql
-- Deve ser >95%
SELECT round(100.0 * sum(blks_hit) / sum(blks_hit + blks_read), 2) as cache_hit_ratio
FROM pg_stat_database WHERE datname = 'btg_db';
```

### 3. Identificar Tabelas com Sequential Scans

```sql
SELECT tablename, seq_scan, seq_tup_read, idx_scan
FROM pg_stat_user_tables
WHERE seq_scan > 100
ORDER BY seq_scan DESC;
```

### 4. Otimizações de Índices

```sql
-- Criar índices compostos para queries frequentes
CREATE INDEX CONCURRENTLY idx_cliente_assessor_ativo
ON clientes(assessores_id, is_active) WHERE is_active = true;

-- Índice para histórico mais recente
CREATE INDEX CONCURRENTLY idx_risco_declarado_cliente_data
ON historico_risco_declarado(clientes_id, created_at DESC);
```

## 📊 Benchmark Esperado

### Configuração de Referência

- **Hardware:** CPU 4 cores, 8GB RAM, SSD
- **PostgreSQL:** Configuração padrão
- **Dados:** ~110k registros

### Resultados Esperados

| Teste     | Clientes | TPS  | Latência Média | P95     |
| --------- | -------- | ---- | -------------- | ------- |
| Warmup    | 5        | >500 | <20ms          | <50ms   |
| Low Load  | 10       | >400 | <25ms          | <60ms   |
| Moderate  | 25       | >250 | <100ms         | <150ms  |
| High Load | 50       | >150 | <300ms         | <500ms  |
| Stress    | 100      | >80  | <1000ms        | <2000ms |

## 🚨 Troubleshooting

### Problema: "pgbench: command not found"

```powershell
# Instalar PostgreSQL client tools
# Download: https://www.postgresql.org/download/windows/
# Ou via Chocolatey:
choco install postgresql
```

### Problema: "connection refused"

```powershell
# Verificar se container está rodando
docker ps | grep postgres

# Reiniciar se necessário
docker-compose restart db
```

### Problema: TPS muito baixo

```sql
-- Verificar configurações do PostgreSQL
SHOW shared_buffers;
SHOW work_mem;
SHOW effective_cache_size;

-- Atualizar estatísticas
ANALYZE;
```

### Problema: Muitos erros

```sql
-- Verificar logs do PostgreSQL
SELECT * FROM pg_stat_activity WHERE state = 'active';

-- Verificar deadlocks
SELECT * FROM pg_stat_database WHERE datname = 'btg_db';
```

## 📝 Logs e Relatórios

### Arquivos Gerados

- **`pgbench_*.log`** - Logs detalhados de cada teste
- **`relatorio_teste_carga_*.md`** - Relatório consolidado
- **Timestamps** - Todos os arquivos têm timestamp para controle de versão

### Análise de Logs

```powershell
# Ver resumo de um teste específico
Get-Content pgbench_btg_moderate_*.log | Select-String "tps\|latency"

# Comparar resultados de diferentes testes
Select-String "tps =" pgbench_*.log
```

## 🎯 Próximos Passos

1. **Executar teste rápido** para validação inicial
2. **Analisar resultados** e identificar gargalos
3. **Otimizar queries** com base nos explain plans
4. **Executar bateria completa** para validação final
5. **Implementar melhorias** no banco de dados
6. **Repetir testes** para medir melhorias
