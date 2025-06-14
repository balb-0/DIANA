-- Scripts para análise de planos de execução (EXPLAIN ANALYZE)
-- Use estes scripts para otimizar queries após os testes de carga

-- ==============================================================================
-- ANÁLISE DE PERFORMANCE DAS QUERIES PRINCIPAIS
-- ==============================================================================

\echo '=== BTG PACTUAL - ANÁLISE DE PLANOS DE EXECUÇÃO ==='
\echo ''

-- 1. Query mais comum: Buscar perfil de risco do cliente
\echo '1. ANÁLISE: Busca de perfil de risco do cliente'
EXPLAIN (ANALYZE, BUFFERS, VERBOSE, FORMAT TEXT) 
SELECT risco
FROM historico_risco_declarado
WHERE clientes_id = 'TEST_CLIENT_000001'
ORDER BY created_at DESC
LIMIT 1;

\echo ''
\echo '2. ANÁLISE: Busca de investimentos do cliente com JOIN'
EXPLAIN (ANALYZE, BUFFERS, VERBOSE, FORMAT TEXT)
SELECT 
    i.id,
    i.quantidade,
    i.valorTotal,
    i.data,
    a.nome as ativo_nome,
    a.tipo,
    a.subgrupo
FROM investimento i
JOIN ativos a ON i.ativos_id = a.id
WHERE i.clientes_id = 'TEST_CLIENT_000001'
ORDER BY i.data DESC
LIMIT 20;

\echo ''
\echo '3. ANÁLISE: Busca de clientes por assessor'
EXPLAIN (ANALYZE, BUFFERS, VERBOSE, FORMAT TEXT)
SELECT 
    c.id,
    c.nome,
    c.categoria,
    c.is_active
FROM clientes c
WHERE c.assessores_id = 'ASSESS_TEST_000001'
AND c.is_active = true;

\echo ''
\echo '4. ANÁLISE: Agregação de investimentos por tipo (query complexa)'
EXPLAIN (ANALYZE, BUFFERS, VERBOSE, FORMAT TEXT)
SELECT 
    a.tipo,
    COUNT(*) as quantidade_investimentos,
    SUM(i.valorTotal) as valor_total,
    AVG(i.valorTotal) as valor_medio
FROM investimento i
JOIN ativos a ON i.ativos_id = a.id
WHERE i.clientes_id = 'TEST_CLIENT_000001'
GROUP BY a.tipo
ORDER BY valor_total DESC;

\echo ''
\echo '5. ANÁLISE: Query de compliance (risco declarado vs calculado)'
EXPLAIN (ANALYZE, BUFFERS, VERBOSE, FORMAT TEXT)
SELECT 
    c.id,
    c.nome,
    c.categoria,
    hrd.risco as risco_declarado,
    hrc.risco as risco_calculado,
    hrc.RV_percentual,
    hrc.RF_percentual,
    hrc.valorTotal
FROM clientes c
LEFT JOIN LATERAL (
    SELECT risco, created_at
    FROM historico_risco_declarado hrd
    WHERE hrd.clientes_id = c.id
    ORDER BY created_at DESC
    LIMIT 1
) hrd ON true
LEFT JOIN LATERAL (
    SELECT risco, RV_percentual, RF_percentual, valorTotal, created_at
    FROM historico_risco_calculado hrc
    WHERE hrc.clientes_id = c.id
    ORDER BY created_at DESC
    LIMIT 1
) hrc ON true
WHERE c.assessores_id = 'ASSESS_TEST_000001'
AND c.is_active = true;

-- ==============================================================================
-- ANÁLISE DE ÍNDICES E ESTATÍSTICAS
-- ==============================================================================

\echo ''
\echo '=== ESTATÍSTICAS DE ÍNDICES ==='

-- Verificar uso dos índices
SELECT 
    schemaname,
    tablename,
    indexname,
    idx_tup_read,
    idx_tup_fetch,
    idx_scan,
    ROUND(100.0 * idx_tup_fetch / NULLIF(idx_tup_read, 0), 2) as hit_rate
FROM pg_stat_user_indexes
WHERE tablename IN ('clientes', 'investimento', 'historico_risco_declarado', 'historico_risco_calculado', 'ativos')
ORDER BY idx_tup_read DESC;

\echo ''
\echo '=== TAMANHO DAS TABELAS E ÍNDICES ==='

-- Tamanho das tabelas
SELECT 
    tablename,
    pg_size_pretty(pg_total_relation_size(tablename)) as total_size,
    pg_size_pretty(pg_relation_size(tablename)) as table_size,
    pg_size_pretty(pg_total_relation_size(tablename) - pg_relation_size(tablename)) as index_size
FROM pg_tables
WHERE tablename IN ('clientes', 'investimento', 'historico_risco_declarado', 'historico_risco_calculado', 'ativos')
ORDER BY pg_total_relation_size(tablename) DESC;

\echo ''
\echo '=== CACHE HIT RATIO ==='

-- Cache hit ratio (deve ser > 95%)
SELECT 
    'Buffer Cache Hit Ratio' as metric,
    ROUND(100.0 * sum(blks_hit) / sum(blks_hit + blks_read), 2) as percentage
FROM pg_stat_database 
WHERE datname = current_database();

\echo ''
\echo '=== TABELAS COM MAIS SEQUENTIAL SCANS ==='

SELECT 
    schemaname,
    tablename,
    seq_scan,
    seq_tup_read,
    idx_scan,
    ROUND(100.0 * seq_tup_read / (seq_tup_read + idx_tup_fetch), 2) as seq_scan_ratio
FROM pg_stat_user_tables
WHERE seq_scan > 0
ORDER BY seq_tup_read DESC
LIMIT 10;

\echo ''
\echo '=== SUGESTÕES DE OTIMIZAÇÃO ==='
\echo ''
\echo 'Com base na análise acima, considere:'
\echo '1. Criar índices compostos para queries frequentes'
\echo '2. Analisar queries com seq scan em tabelas grandes'
\echo '3. Verificar se cache hit ratio está acima de 95%'
\echo '4. Considerar particionamento para tabelas muito grandes'
\echo '5. Atualizar estatísticas: ANALYZE tablename;'
\echo ''
