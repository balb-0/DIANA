# Nome do Projeto: Diana

## Nome do Grupo: Watermeloon

## Visão Geral

Este diretório contém os scripts e arquivos de configuração necessários para a implementação do banco de dados PostgreSQL utilizado na aplicação de recomendação de investimentos do BTG Pactual. O banco armazena informações sobre clientes, assessores, ativos financeiros, investimentos e histórico de perfil de risco.

## Estrutura de Arquivos

- `01_create_database.sql`: Script de criação das tabelas e estruturas do banco de dados
- `02_initial_data.sql`: Script de população inicial do banco com dados de exemplo
- `Dockerfile`: Configuração para criação da imagem Docker do PostgreSQL
- `docker-compose.yml`: Arquivo para orquestração do container do banco de dados

## Implementação do Banco

A implementação do banco de dados PostgreSQL foi realizada com atenção especial a:

### Escolha de Tipos de Dados
- Uso de tipos ENUM (`categoria_enum`, `risco_enum`) para garantir consistência nos valores aceitos
- VARCHAR(26) para IDs (compatível com formato ULID)
- TIMESTAMP para registros temporais precisos, permitindo auditoria de alterações
- DOUBLE PRECISION para valores monetários, garantindo precisão adequada

### Relacionamentos e Integridade Referencial
- Chaves estrangeiras definidas com ON DELETE SET NULL (clientes→assessores)
- Chaves estrangeiras sem cláusulas especiais (investimento→ativos/clientes)
- Relacionamentos pensados para minimizar redundância e maximizar integridade

### Estratégia de Persistência
- Armazenamento de perfis de risco como histórico (tabelas dedicadas)
- Separação entre risco declarado e risco calculado
- Manutenção de timestamp para cada registro de perfil

### Considerações de Performance
- Índices implícitos em chaves primárias
- Campo is_active para filtros rápidos, evitando exclusão física
- Timestamp de inativação para manter histórico de clientes

## Modelo Físico

O banco de dados segue a seguinte estrutura:

### Dicionário dos dados

Dicioniários dos dados aqui.

## Requisitos de Instalação

### Pré-requisitos

- Docker Desktop (v20.10.x ou superior)
- Docker Compose (v2.x ou superior)
- 2GB de memória RAM disponível
- 1GB de espaço em disco

### Implantação com Docker

1. **Clone o repositório**:
   ```bash
   cd 2025-1B-T13-ES06-G03/src
   ```

2. **Inicie o container do banco**:
   ```bash
   docker-compose up -d
   ```

3. **Verifique se o container está rodando**:
   ```bash
   docker ps
   ```

4. **Acesse o banco de dados**:
   ```bash
   docker exec -it btg_postgres psql -U btg_user -d btg_db
   ```

5. **Comandos úteis dentro do PostgreSQL**:
   ```sql
   -- Listar todas as tabelas
   \dt
   
   -- Visualizar estrutura de uma tabela específica
   \d nome_da_tabela
   
   -- Consultar dados de exemplo
   SELECT * FROM clientes;
   SELECT * FROM assessores;
   SELECT * FROM ativos LIMIT 5;
   ```

### Configuração Manual (sem Docker)

Se preferir instalar manualmente o PostgreSQL:

1. **Instale o PostgreSQL 16**:
   - Windows: [Download PostgreSQL para Windows](https://www.postgresql.org/download/windows/)
   - Linux: `sudo apt-get install postgresql-16`
   - macOS: `brew install postgresql@16`

2. **Configure o usuário e banco**:
   ```bash
   sudo -u postgres psql
   CREATE USER btg_user WITH PASSWORD 'btg_pass';
   CREATE DATABASE btg_db OWNER btg_user;
   \q
   ```

3. **Execute os scripts**:
   ```bash
   psql -U btg_user -d btg_db -f 01_create_database.sql
   psql -U btg_user -d btg_db -f 02_initial_data.sql
   ```

## Manutenção e Backup

### Backup do Banco de Dados

Para realizar o backup dos dados:

```bash
docker exec -t btg_postgres pg_dump -U btg_user btg_db > backup_$(date +%Y%m%d).sql
```

### Restauração do Backup

Para restaurar um backup:

```bash
cat backup_YYYYMMDD.sql | docker exec -i btg_postgres psql -U btg_user -d btg_db
```

### Limpeza do Banco

Para resetar o banco de dados:

```bash
docker-compose down -v
docker-compose up -d
```

## Troubleshooting

### Problemas Comuns

1. **Erro de conexão**:
   - Verifique se o container está rodando: `docker ps`
   - Teste a conexão: `docker exec -it btg_postgres pg_isready`

2. **Erros nos scripts**:
   - Veja os logs: `docker logs btg_postgres`
   - Execute os scripts manualmente dentro do container

3. **Dados não aparecem**:
   - Verifique a ordem de inserção nos scripts
   - Confira se há erros de chave estrangeira nos logs

## Informações Adicionais

- **Versão do PostgreSQL**: 16.9
- **Codificação**: UTF-8
- **Locale**: en_US.utf8
- **Porta padrão**: 5432
- **Credenciais padrão**: 
  - Usuário: btg_user
  - Senha: btg_pass
  - Banco: btg_db
