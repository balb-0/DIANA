-- Inserindo assessores
INSERT INTO assessores (
  id, nome, sobrenome, email, telefone, cpf, rg, endereco, data_nascimento, categoria, unidade_btg
) VALUES 
  ('01HXQK6GQZDSGTVKX6PGXH7N9A', 'João', 'Silva', 'joao@btg.com', '11999999999', '12345678900', 'MG1234567', 'Rua A, 123', '1985-07-14', 'Gold', 'Unidade SP'),
  ('01HXQK6N8YH2EKEKG7M6WMP8VV', 'Mariana', 'Souza', 'mariana@btg.com', '21988888888', '98765432100', 'RJ7654321', 'Av. B, 456', '1990-05-22', 'Platinum', 'Unidade RJ');

-- Inserindo clientes
INSERT INTO clientes (
  id, nome, sobrenome, email, telefone, cpf, rg, endereco, data_nascimento, saldo_carteira, assessores_id
) VALUES 
  ('01HXQK6Q1YXKH2QKSR89NR9A1W', 'Carlos', 'Pereira', 'carlos@email.com', '11977777777', '11122233344', 'SP1122334', 'Rua C, 789', '1995-09-10', 25000.00, '01HXQK6GQZDSGTVKX6PGXH7N9A'),
  ('01HXQK6T82TTGM9QTEZH4FD7KM', 'Ana', 'Oliveira', 'ana@email.com', '21966666666', '55566677788', 'RJ5566778', 'Av. D, 101', '1988-12-03', 50000.00, '01HXQK6N8YH2EKEKG7M6WMP8VV');

-- Inserindo ativos
INSERT INTO ativos (
  id, nome, tipo, subgrupo
) VALUES 
  ('01HXQK6V7JH6S65YAGMSQ83PYH', 'Fundo Imobiliário XPTO', 'Fundo Imobiliário', 'FIIs'),
  ('01HXQK6X3C6H56MXMY5B26YYTJ', 'Ação Petrobras', 'Ação', 'Petrobras'),
  ('01HXQK6ZVBD8QDR1N67A2GYX4R', 'Tesouro IPCA 2035', 'Renda Fixa', 'Tesouro Direto');

-- Inserindo investimentos
INSERT INTO investimento (
  id, ativos_id, clientes_id, quantidade, valorTotal
) VALUES 
  ('01HXQK71JK8S89ENMQ4Z2VNHNP', '01HXQK6V7JH6S65YAGMSQ83PYH', '01HXQK6Q1YXKH2QKSR89NR9A1W', 100, 10500.00),
  ('01HXQK73KEEKPC4X6HCB9RTBF7', '01HXQK6X3C6H56MXMY5B26YYTJ', '01HXQK6Q1YXKH2QKSR89NR9A1W', 50, 13000.00),
  ('01HXQK75B9RM4BX16E7XT2D49C', '01HXQK6ZVBD8QDR1N67A2GYX4R', '01HXQK6T82TTGM9QTEZH4FD7KM', 30, 26500.00);

-- Inserindo histórico de risco declarado
INSERT INTO historico_risco_declarado (
  id, risco, created_at, clientes_id
) VALUES 
  ('01HXQK7765XZDKZSR0ZCWG68Q9', '{"perfil": "moderado", "respostas": [3, 2, 4, 5]}', NOW(), '01HXQK6Q1YXKH2QKSR89NR9A1W'),
  ('01HXQK78SPPZEF5EYQ3MR5X2VH', '{"perfil": "agressivo", "respostas": [5, 5, 4, 4]}', NOW(), '01HXQK6T82TTGM9QTEZH4FD7KM');

-- Inserindo histórico de risco calculado
INSERT INTO historico_risco_calculado (
  id, risco, created_at, clientes_id
) VALUES 
  ('01HXQK7B2A83AVJQ9MMS2PY9EP', '{"perfil": "moderado", "score": 62}', NOW(), '01HXQK6Q1YXKH2QKSR89NR9A1W'),
  ('01HXQK7F7TBD3FRHAG86Z53P90', '{"perfil": "agressivo", "score": 85}', NOW(), '01HXQK6T82TTGM9QTEZH4FD7KM');
