-- Consultas para teste de carga com pgbench - BTG Pactual
-- Este arquivo contém queries representativas das operações mais comuns do sistema

-- ==============================================================================
-- QUERIES PARA TESTE DE CARGA SIMULANDO OPERAÇÕES REAIS
-- ==============================================================================

-- Query 1: Busca perfil de risco do cliente (operação mais comum - 40% do tráfego)
\set client_num random(1, 10000)
\set client_id 'TEST_CLIENT_' || lpad(:client_num::text, 6, '0')
SELECT risco
FROM historico_risco_declarado
WHERE clientes_id = :'client_id'
ORDER BY created_at DESC
LIMIT 1;

-- Query 2: Busca investimentos do cliente com detalhes (25% do tráfego)
\set client_num random(1, 10000)
\set client_id 'TEST_CLIENT_' || lpad(:client_num::text, 6, '0')
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
WHERE i.clientes_id = :'client_id'
ORDER BY i.data DESC
LIMIT 20;

-- Query 3: Busca clientes por assessor (15% do tráfego)
\set assessor_num random(1, 100)
\set assessor_id 'ASSESS_TEST_' || lpad(:assessor_num::text, 6, '0')
SELECT 
    c.id,
    c.nome,
    c.categoria,
    c.is_active
FROM clientes c
WHERE c.assessores_id = :'assessor_id'
AND c.is_active = true;

-- Query 4: Agregação de investimentos por tipo (10% do tráfego)
\set client_num random(1, 10000)
\set client_id 'TEST_CLIENT_' || lpad(:client_num::text, 6, '0')
SELECT 
    a.tipo,
    COUNT(*) as quantidade_investimentos,
    SUM(i.valorTotal) as valor_total,
    AVG(i.valorTotal) as valor_medio
FROM investimento i
JOIN ativos a ON i.ativos_id = a.id
WHERE i.clientes_id = :'client_id'
GROUP BY a.tipo
ORDER BY valor_total DESC;

-- Query 5: Verificação de compliance (5% do tráfego) 
\set client_num random(1, 10000)
\set client_id 'TEST_CLIENT_' || lpad(:client_num::text, 6, '0')
SELECT 
    c.id,
    c.nome,
    c.categoria,
    hrd.risco as risco_declarado,
    hrc.risco as risco_calculado,
    CASE 
        WHEN hrd.risco = hrc.risco THEN 'CONFORME'
        ELSE 'DIVERGENTE'
    END as status_compliance
FROM clientes c
LEFT JOIN LATERAL (
    SELECT risco
    FROM historico_risco_declarado h
    WHERE h.clientes_id = c.id
    ORDER BY created_at DESC
    LIMIT 1
) hrd ON true
LEFT JOIN LATERAL (
    SELECT risco
    FROM historico_risco_calculado h
    WHERE h.clientes_id = c.id
    ORDER BY created_at DESC
    LIMIT 1
) hrc ON true
WHERE c.id = :'client_id';

-- Query 6: Busca por CPF (3% do tráfego)
\set cpf_suffix random(100000000, 110000000)
SELECT 
    id,
    nome,
    categoria,
    is_active
FROM clientes
WHERE cpf = :cpf_suffix::text;

-- Query 7: Relatório de volume por unidade BTG (2% do tráfego)
SELECT 
    a.unidade_btg,
    COUNT(DISTINCT c.id) as total_clientes,
    SUM(i.valorTotal) as volume_total
FROM assessores a
JOIN clientes c ON a.id = c.assessores_id
JOIN investimento i ON c.id = i.clientes_id
WHERE i.data >= CURRENT_DATE - INTERVAL '30 days'
AND a.id LIKE 'ASSESS_TEST_%'
GROUP BY a.unidade_btg
ORDER BY volume_total DESC;
