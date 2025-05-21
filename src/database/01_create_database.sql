-- Caminho: src/database/01_create_database.sql

-- Criação das tabelas

CREATE TABLE assessores (
    id VARCHAR(26) PRIMARY KEY,
    email VARCHAR(45),
    telefone VARCHAR(45),
    nome VARCHAR(45),
    sobrenome VARCHAR(70),
    cpf VARCHAR(45),
    rg VARCHAR(45),
    endereco VARCHAR(45),
    data_nascimento DATE,
    categoria VARCHAR(10) CHECK (categoria IN ('Gold', 'Prata', 'Platinum')),
    unidade_btg VARCHAR(45)
);

CREATE TABLE clientes (
    id VARCHAR(26) PRIMARY KEY,
    email VARCHAR(255),
    telefone VARCHAR(255),
    nome VARCHAR(45),
    sobrenome VARCHAR(70),
    cpf VARCHAR(45),
    rg VARCHAR(45),
    endereco VARCHAR(255),
    data_nascimento DATE,
    saldo_carteira DOUBLE PRECISION,
    assessores_id VARCHAR(26),
    CONSTRAINT fk_assessor FOREIGN KEY (assessores_id) REFERENCES assessores(id)
);

CREATE TABLE ativos (
    id VARCHAR(26) PRIMARY KEY,
    nome VARCHAR(255),
    tipo VARCHAR(255),
    subgrupo VARCHAR(45)
);

CREATE TABLE investimento (
    id VARCHAR(26) PRIMARY KEY,
    ativos_id VARCHAR(26) NOT NULL,
    clientes_id VARCHAR(26) NOT NULL,
    quantidade INT,
    valorTotal DOUBLE PRECISION,
    CONSTRAINT fk_investimento_ativo FOREIGN KEY (ativos_id) REFERENCES ativos(id),
    CONSTRAINT fk_investimento_cliente FOREIGN KEY (clientes_id) REFERENCES clientes(id)
);

CREATE TABLE historico_risco_declarado (
    id VARCHAR(26) PRIMARY KEY,
    risco JSON,
    created_at TIMESTAMP,
    clientes_id VARCHAR(26) NOT NULL,
    CONSTRAINT fk_risco_declarado_cliente FOREIGN KEY (clientes_id) REFERENCES clientes(id)
);

CREATE TABLE historico_risco_calculado (
    id VARCHAR(26) PRIMARY KEY,
    risco JSON,
    created_at TIMESTAMP,
    clientes_id VARCHAR(26) NOT NULL,
    CONSTRAINT fk_risco_calculado_cliente FOREIGN KEY (clientes_id) REFERENCES clientes(id)
);
