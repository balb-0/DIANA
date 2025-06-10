-- Script para preparar dados de teste para pgbench - BTG Pactual
-- Este script cria dados de massa para simular condições de alto uso

-- ==============================================================================
-- SETUP PARA TESTE DE CARGA COM PGBENCH
-- ==============================================================================

\echo 'Iniciando setup de dados para teste de carga...'

-- Limpar dados de teste anteriores se existirem
DELETE FROM investimento WHERE clientes_id LIKE 'TEST_%';
DELETE FROM historico_risco_declarado WHERE clientes_id LIKE 'TEST_%';
DELETE FROM historico_risco_calculado WHERE clientes_id LIKE 'TEST_%';
DELETE FROM clientes WHERE id LIKE 'TEST_%';
DELETE FROM assessores WHERE id LIKE 'ASSESS_TEST_%';
DELETE FROM ativos WHERE id LIKE 'ATIVO_TEST_%';

\echo 'Dados anteriores limpos.'

-- ==============================================================================
-- CRIAÇÃO DE DADOS DE MASSA PARA TESTE
-- ==============================================================================

-- 1. Inserir assessores de teste (100 assessores)
\echo 'Criando assessores de teste...'
INSERT INTO assessores (id, nome, unidade_btg)
SELECT 
    'ASSESS_TEST_' || LPAD(generate_series::text, 6, '0'),
    'Assessor Teste ' || generate_series,
    CASE (generate_series % 4) 
        WHEN 0 THEN 'São Paulo'
        WHEN 1 THEN 'Rio de Janeiro'
        WHEN 2 THEN 'Brasília'
        ELSE 'Belo Horizonte'
    END
FROM generate_series(1, 100);

\echo 'Assessores criados: 100'

-- 2. Inserir ativos de teste (500 ativos)
\echo 'Criando ativos de teste...'
INSERT INTO ativos (id, nome, tipo, subgrupo)
SELECT 
    'ATIVO_TEST_' || LPAD(generate_series::text, 6, '0'),
    'Ativo Teste ' || generate_series,
    CASE (generate_series % 4)
        WHEN 0 THEN 'Renda Fixa'
        WHEN 1 THEN 'Renda Variável'
        WHEN 2 THEN 'Fundo'
        ELSE 'Derivativo'
    END,
    CASE (generate_series % 8)
        WHEN 0 THEN 'CDB'
        WHEN 1 THEN 'Ações'
        WHEN 2 THEN 'FII'
        WHEN 3 THEN 'Tesouro'
        WHEN 4 THEN 'Debênture'
        WHEN 5 THEN 'COE'
        WHEN 6 THEN 'Futuro'
        ELSE 'Opção'
    END
FROM generate_series(1, 500);

\echo 'Ativos criados: 500'

-- 3. Inserir clientes de teste (10.000 clientes)
\echo 'Criando clientes de teste...'
INSERT INTO clientes (id, nome, cpf, rg, endereco, data_nascimento, categoria, assessores_id, is_active)
SELECT 
    'TEST_CLIENT_' || LPAD(generate_series::text, 6, '0'),
    'Cliente Teste ' || generate_series,
    LPAD((100000000 + generate_series)::text, 11, '0'),
    LPAD((1000000 + generate_series)::text, 9, '0'),
    'Endereço Teste ' || generate_series,
    DATE '1970-01-01' + (generate_series % 15000)::integer,
    CASE (generate_series % 3)
        WHEN 0 THEN 'Gold'::categoria_enum
        WHEN 1 THEN 'Prata'::categoria_enum
        ELSE 'Platinum'::categoria_enum
    END,
    'ASSESS_TEST_' || LPAD(((generate_series % 100) + 1)::text, 6, '0'),
    CASE WHEN generate_series % 20 = 0 THEN false ELSE true END
FROM generate_series(1, 10000);

\echo 'Clientes criados: 10.000'

-- 4. Inserir histórico de risco declarado (25.000 registros)
\echo 'Criando histórico de risco declarado...'
INSERT INTO historico_risco_declarado (id, risco, created_at, clientes_id)
SELECT 
    'HIST_DECL_' || LPAD(seq::text, 8, '0'),
    CASE (random() * 3)::int
        WHEN 0 THEN 'Conservador'::risco_enum
        WHEN 1 THEN 'Moderado'::risco_enum
        ELSE 'Sofisticado'::risco_enum
    END,
    NOW() - (random() * INTERVAL '365 days'),
    'TEST_CLIENT_' || LPAD(((seq % 10000) + 1)::text, 6, '0')
FROM generate_series(1, 25000) seq;

\echo 'Histórico de risco declarado criado: 25.000 registros'

-- 5. Inserir histórico de risco calculado (25.000 registros)
\echo 'Criando histórico de risco calculado...'
INSERT INTO historico_risco_calculado (id, risco, RV_percentual, RF_percentual, valorTotal, created_at, clientes_id)
SELECT 
    'HIST_CALC_' || LPAD(seq::text, 8, '0'),
    CASE (random() * 3)::int
        WHEN 0 THEN 'Conservador'::risco_enum
        WHEN 1 THEN 'Moderado'::risco_enum
        ELSE 'Sofisticado'::risco_enum
    END,
    random() * 100,
    random() * 100,
    (random() * 1000000 + 10000)::numeric(15,2),
    NOW() - (random() * INTERVAL '365 days'),
    'TEST_CLIENT_' || LPAD(((seq % 10000) + 1)::text, 6, '0')
FROM generate_series(1, 25000) seq;

\echo 'Histórico de risco calculado criado: 25.000 registros'

-- 6. Inserir investimentos de teste (50.000 registros)
\echo 'Criando investimentos de teste...'
INSERT INTO investimento (id, ativos_id, clientes_id, quantidade, valorTotal, data)
SELECT 
    'INV_TEST_' || LPAD(seq::text, 8, '0'),
    'ATIVO_TEST_' || LPAD(((seq % 500) + 1)::text, 6, '0'),
    'TEST_CLIENT_' || LPAD(((seq % 10000) + 1)::text, 6, '0'),
    (random() * 1000 + 1)::int,
    (random() * 100000 + 1000)::numeric(15,2),
    NOW() - (random() * INTERVAL '730 days')
FROM generate_series(1, 50000) seq;

\echo 'Investimentos criados: 50.000 registros'

-- ==============================================================================
-- CRIAÇÃO DE ÍNDICES PARA OTIMIZAÇÃO
-- ==============================================================================

\echo 'Criando índices para otimização...'

-- Índices principais para performance
CREATE INDEX CONCURRENTLY IF NOT EXISTS idx_historico_risco_declarado_cliente_data 
ON historico_risco_declarado(clientes_id, created_at DESC);

CREATE INDEX CONCURRENTLY IF NOT EXISTS idx_investimento_cliente_data 
ON investimento(clientes_id, data DESC);

CREATE INDEX CONCURRENTLY IF NOT EXISTS idx_clientes_assessor_ativo 
ON clientes(assessores_id, is_active) WHERE is_active = true;

CREATE INDEX CONCURRENTLY IF NOT EXISTS idx_historico_risco_calculado_cliente_data 
ON historico_risco_calculado(clientes_id, created_at DESC);

CREATE INDEX CONCURRENTLY IF NOT EXISTS idx_investimento_ativos 
ON investimento(ativos_id);

-- Índice para busca por CPF
CREATE INDEX CONCURRENTLY IF NOT EXISTS idx_clientes_cpf 
ON clientes(cpf);

\echo 'Índices criados.'

-- ==============================================================================
-- ATUALIZAÇÃO DE ESTATÍSTICAS
-- ==============================================================================

\echo 'Atualizando estatísticas do PostgreSQL...'

-- Força atualização das estatísticas para o otimizador
ANALYZE clientes;
ANALYZE assessores;
ANALYZE ativos;
ANALYZE investimento;
ANALYZE historico_risco_declarado;
ANALYZE historico_risco_calculado;

\echo 'Estatísticas atualizadas.'

-- ==============================================================================
-- INFORMAÇÕES DO SETUP
-- ==============================================================================

\echo ''
\echo '=== RESUMO DOS DADOS CRIADOS ==='

-- Mostra o resumo dos dados criados
SELECT 'assessores' as tabela, COUNT(*) as registros FROM assessores WHERE id LIKE 'ASSESS_TEST_%'
UNION ALL
SELECT 'ativos', COUNT(*) FROM ativos WHERE id LIKE 'ATIVO_TEST_%'
UNION ALL
SELECT 'clientes', COUNT(*) FROM clientes WHERE id LIKE 'TEST_%'
UNION ALL
SELECT 'historico_risco_declarado', COUNT(*) FROM historico_risco_declarado WHERE clientes_id LIKE 'TEST_%'
UNION ALL
SELECT 'historico_risco_calculado', COUNT(*) FROM historico_risco_calculado WHERE clientes_id LIKE 'TEST_%'
UNION ALL
SELECT 'investimento', COUNT(*) FROM investimento WHERE clientes_id LIKE 'TEST_%'
ORDER BY registros DESC;

\echo ''
\echo '=== TAMANHO DAS TABELAS ==='

-- Mostra o tamanho das tabelas após inserção
SELECT 
    tablename,
    pg_size_pretty(pg_total_relation_size(tablename)) as total_size,
    pg_size_pretty(pg_relation_size(tablename)) as table_size,
    pg_size_pretty(pg_total_relation_size(tablename) - pg_relation_size(tablename)) as index_size
FROM pg_tables
WHERE tablename IN ('clientes', 'assessores', 'ativos', 'investimento', 
                   'historico_risco_declarado', 'historico_risco_calculado')
ORDER BY pg_total_relation_size(tablename) DESC;

\echo ''
\echo 'Setup de dados para teste de carga concluído!'
\echo 'Total de registros criados: ~110.600 registros'
\echo 'Pronto para executar pgbench!'
