-- Dados iniciais para a tabela assessores (primeiro!)
INSERT INTO assessores (
  id, nome, unidade_btg
) VALUES 
  ('01HXQK6T7F9D78XK9F0WJ79PZ5', 'Maria Souza', 'BTG São Paulo');

-- Dados iniciais para a tabela clientes
INSERT INTO clientes (
  id, nome, cpf, rg, endereco, data_nascimento, categoria, assessores_id, is_active, inactive_datetime
) VALUES 
  ('01HXQK6Q1YXKH2QKSR89NR9A1W', 'João da Silva', '12345678900', 'MG1234567', 'Rua Exemplo, 123', '1985-10-10', 'Gold', '01HXQK6T7F9D78XK9F0WJ79PZ5', true, NULL);

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
  ('01HXQK7765XZDKZSR0ZCWG68Q9', 'Moderado', NOW(), '01HXQK6Q1YXKH2QKSR89NR9A1W');

-- Dados iniciais para a tabela historico_risco_calculado
INSERT INTO historico_risco_calculado (
  id, risco, RV_percentual, RF_percentual, valorTotal, created_at, clientes_id
) VALUES 
  ('01HXQK7B2A83AVJQ9MMS2PY9EP', 'Moderado', 28.0, 72.0, 216137.56, NOW(), '01HXQK6Q1YXKH2QKSR89NR9A1W');
