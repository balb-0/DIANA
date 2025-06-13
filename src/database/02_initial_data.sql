-- Dados iniciais para a tabela assessores
INSERT INTO assessores (
  id, nome, unidade_btg
) VALUES 
  ('01HXQK6T7F9D78XK9F0WJ79PZ5', 'Maria Souza', 'BTG São Paulo'),
  ('rfvQP8smGPS0MUnBAWwfXt4FgvU2', 'Zillinho', 'BTG Marília');

-- Dados iniciais para a tabela clientes
INSERT INTO clientes (
  id, nome, cpf, rg, endereco, data_nascimento, categoria, assessores_id, is_active, inactive_datetime
) VALUES 
  ('01HXQK6Q1YXKH2QKSR89NR9A1W', 'João da Silva', '12345678900', 'MG1234567', 'Rua Exemplo, 123', '1985-10-10', 'Gold', '01HXQK6T7F9D78XK9F0WJ79PZ5', true, NULL),
  ('01JX2RB6JD6QCZAXNA3M7KERAV', 'Ricardo Castro', '22222233345', 'SP1234567', 'Rua Exemplo, 344', '1995-11-12', 'Gold', 'rfvQP8smGPS0MUnBAWwfXt4FgvU2', true, NULL),
  ('01JX2REHEB5Y7Y7N28R94MH0W5', 'Maria Santos', '12345678910', 'MG1233347', 'Rua Exemplo, 800', '2000-10-12', 'Gold', 'rfvQP8smGPS0MUnBAWwfXt4FgvU2', true, NULL);

-- Dados iniciais para a tabela ativos
INSERT INTO ativos (
  id, nome, tipo, subgrupo
) VALUES 
  ('01HFAKEATIVO00', 'CRA CDIE', 'Renda Fixa', 'TITULOS RF PRIVADOS BRASIL'),
  ('01HFAKEATIVO01', 'CRA IPCA', 'Renda Fixa', 'TITULOS RF PRIVADOS BRASIL'),
  ('01HFAKEATIVO02', 'RRRP13 IPCA', 'Renda Fixa', 'DEBENTURES BRASIL'),
  ('01HFAKEATIVO03', 'EEELA1 IPCA', 'Renda Fixa', 'DEBENTURES BRASIL'),
  ('01HFAKEATIVO04', 'TRRG12 IPCA', 'Renda Fixa', 'DEBENTURES BRASIL'),
  ('01HFAKEATIVO05', 'CRA IPCA', 'Renda Fixa', 'TITULOS RF PRIVADOS BRASIL'),
  ('01HFAKEATIVO06', 'CRA IPCA', 'Renda Fixa', 'TITULOS RF PRIVADOS BRASIL'),
  ('01HFAKEATIVO07', 'CRA IPCA', 'Renda Fixa', 'TITULOS RF PRIVADOS BRASIL');

-- Dados iniciais para a tabela investimento
INSERT INTO investimento (
  id, ativos_id, clientes_id, quantidade, valorTotal, data
) VALUES 
  ('01HFAKEINVEST00', '01HFAKEATIVO00', '01HXQK6Q1YXKH2QKSR89NR9A1W', 25, 25081.62, '2025-05-23 16:45:54'),
  ('01HFAKEINVEST01', '01HFAKEATIVO01', '01HXQK6Q1YXKH2QKSR89NR9A1W', 49, 47046.25, '2025-05-23 16:45:54'),
  ('01HFAKEINVEST02', '01HFAKEATIVO02', '01HXQK6Q1YXKH2QKSR89NR9A1W', 35, 36331.48, '2025-05-23 16:45:54'),
  ('01HFAKEINVEST03', '01HFAKEATIVO03', '01HXQK6Q1YXKH2QKSR89NR9A1W', 36, 38057.15, '2025-05-23 16:45:54'),
  ('01HFAKEINVEST04', '01HFAKEATIVO04', '01HXQK6Q1YXKH2QKSR89NR9A1W', 10, 10365.00, '2025-05-23 16:45:54'),
  ('01HFAKEINVEST05', '01HFAKEATIVO05', '01HXQK6Q1YXKH2QKSR89NR9A1W', 16, 16583.55, '2025-05-23 16:45:54'),
  ('01HFAKEINVEST06', '01HFAKEATIVO06', '01HXQK6Q1YXKH2QKSR89NR9A1W', 31, 31621.63, '2025-05-23 16:45:54'),
  ('01HFAKEINVEST07', '01HFAKEATIVO07', '01HXQK6Q1YXKH2QKSR89NR9A1W', 11, 11050.88, '2025-05-23 16:45:54');

-- Dados iniciais para a tabela historico_risco_declarado
INSERT INTO historico_risco_declarado (
  id, risco, created_at, clientes_id
) VALUES 
  ('01HXQK7765XZDKZSR0ZCWG68Q9', 'Moderado', NOW(), '01HXQK6Q1YXKH2QKSR89NR9A1W'),
  ('01JX2RGAKJBZCRJ7VGMK833VQ8', 'Sofisticado', NOW(), '01JX2RB6JD6QCZAXNA3M7KERAV'),
  ('01JX2RGFA66GBAYZKCGJPCCS90', 'Conservador', NOW(), '01JX2REHEB5Y7Y7N28R94MH0W5');


-- Dados iniciais para a tabela historico_risco_calculado
INSERT INTO historico_risco_calculado (
  id, risco, RV_percentual, RF_percentual, valorTotal, created_at, clientes_id
) VALUES 
  ('01HXQK7B2A83AVJQ9MMS2PY9EP', 'Moderado', 28.0, 72.0, 216137.56, NOW(), '01HXQK6Q1YXKH2QKSR89NR9A1W'),
  ('01JX2RGM0NFN03YWJVDHC9VMA4', 'Sofisticado', 50.0, 90.0, 782147.56, NOW(), '23701bcf-a294-4883-b6ab-aa7de47ce89d'),
  ('01JX2RGR0GA58G1DE8GD7WNWCF', 'Conservador', 10.0, 104.0, 19924.56, NOW(), 'b3638bb4-b780-44ff-9922-ccdd10ad7ca6');
