-- 02_initial_data.sql
-- Example seed data for integration tests

-- Insert some clients
INSERT INTO clientes (id, nome, email) VALUES
  ('01HXQK6Q1YXKH2QKSR89NR9A1W', 'Cliente Teste', 'cliente@teste.com');

-- Insert some assets
INSERT INTO ativos (id, nome, tipo, subgrupo) VALUES
  ('asset1', 'Ação XYZ', 'Ação', 'Blue Chip');

-- Insert some investments
INSERT INTO investimentos (id, ativos_id, clientes_id, quantidade, valor_total, data, tipo) VALUES
  ('inv1', 'asset1', '01HXQK6Q1YXKH2QKSR89NR9A1W', 100, 5000.00, NOW(), 'RendaFixa');

-- Insert some risk profile history
INSERT INTO historico_risco_declarado (id, clientes_id, risco, created_at) VALUES
  ('risk1', '01HXQK6Q1YXKH2QKSR89NR9A1W', 'Conservador', NOW());
