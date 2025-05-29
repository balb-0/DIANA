-- Tipos ENUM
CREATE TYPE categoria_enum AS ENUM ('Gold', 'Prata', 'Platinum');
CREATE TYPE risco_enum AS ENUM ('Conservador', 'Moderado', 'Sofisticado');

-- Tabelas

CREATE TABLE assessores (
    id VARCHAR(26) PRIMARY KEY,
    nome VARCHAR(45),
    unidade_btg VARCHAR(45)
);

CREATE TABLE clientes (
    id VARCHAR(26) PRIMARY KEY,
    nome VARCHAR(100),
    cpf VARCHAR(45),
    rg VARCHAR(45),
    endereco VARCHAR(45),
    data_nascimento DATE,
    categoria categoria_enum,
    assessores_id VARCHAR(26),
    is_active BOOLEAN DEFAULT TRUE,
    inactive_datetime TIMESTAMP,
    FOREIGN KEY (assessores_id) REFERENCES assessores(id) ON DELETE SET NULL
);

CREATE TABLE ativos (
    id VARCHAR(26) PRIMARY KEY,
    nome VARCHAR(100),
    tipo VARCHAR(45),
    subgrupo VARCHAR(45)
);

CREATE TABLE investimento (
    id VARCHAR(26) PRIMARY KEY,
    ativos_id VARCHAR(26) NOT NULL,
    clientes_id VARCHAR(26) NOT NULL,
    quantidade INT,
    valorTotal DOUBLE PRECISION,
    data TIMESTAMP,
    FOREIGN KEY (ativos_id) REFERENCES ativos(id),
    FOREIGN KEY (clientes_id) REFERENCES clientes(id)
);

CREATE TABLE historico_risco_declarado (
    id VARCHAR(26) PRIMARY KEY,
    risco risco_enum,
    created_at TIMESTAMP,
    clientes_id VARCHAR(26) NOT NULL,
    FOREIGN KEY (clientes_id) REFERENCES clientes(id)
);

CREATE TABLE historico_risco_calculado (
    id VARCHAR(26) PRIMARY KEY,
    risco risco_enum,
    RV_percentual DOUBLE PRECISION,
    RF_percentual DOUBLE PRECISION,
    valorTotal DOUBLE PRECISION,
    created_at TIMESTAMP,
    clientes_id VARCHAR(26) NOT NULL,
    FOREIGN KEY (clientes_id) REFERENCES clientes(id)
);
