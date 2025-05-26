# Nome do Projeto: Diana

## Nome do Grupo: Watermeloon

## Visão Geral

Este diretório contém os scripts e arquivos de configuração necessários para a implementação d4. **Trigger não funciona**:
   - Verifique se o script 03_procedures.sql foi executado: `\df recalcular_perfil_risco`
   - Confira os logs para erros na criação da função ou triggeranco de dados PostgreSQL utilizado na aplicação de recomendação de investimentos do BTG Pactual. O banco armazena informações sobre clientes, assessores, ativos financeiros, investimentos e histórico de perfil de risco.

## Estrutura de Arquivos

- `01_create_database.sql`: Script de criação das tabelas e estruturas do banco de dados
- `02_initial_data.sql`: Script de população inicial do banco com dados de exemplo
- `03_procedures.sql`: Script com triggers e procedures para automação do banco
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

### Automação de Perfil de Risco
- Implementação de triggers que monitoram alterações na tabela de investimentos
- Recálculo automático do perfil de risco sempre que um investimento é adicionado, modificado ou removido
- Cálculo dos percentuais de Renda Fixa e Renda Variável na carteira total
- Classificação do perfil baseada em regras de negócio:
  - **Conservador**: ≥ 90% em Renda Fixa
  - **Sofisticado**: ≥ 70% em Renda Variável
  - **Moderado**: Todos os demais casos
- Registro histórico de todas as alterações de perfil com timestamp

### Dados de Teste

O banco é populado com dados de teste para demonstrar diferentes perfis de investimento:

1. **Cliente 1 - João da Silva (Perfil Moderado)**
   - Combinação de Renda Fixa (72%) e Renda Variável (28%)
   - Investimentos diversificados em CRAs, Debêntures e algumas ações

2. **Cliente 2 - Ana Pereira (Perfil Conservador)**
   - Concentração em Renda Fixa (91.9%)
   - Apenas 8.1% em Renda Variável (ETF)
   - Foco em segurança com baixa exposição a riscos

3. **Cliente 3 - Roberto Almeida (Perfil Sofisticado)**
   - Alta concentração em Renda Variável (86.3%)
   - Diversidade de ações (INBR32, ONCO3, BBAS3, PETR4, VALE3)
   - Apenas 13.7% em Renda Fixa
   - Foco em maior rentabilidade com tolerância a riscos

Os dados estão estruturados para permitir testar a funcionalidade de recálculo automático de perfil conforme regras estabelecidas.

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
   
   -- Verificar os perfis de risco calculados para cada cliente
   SELECT c.nome, hrc.risco, hrc.RV_percentual, hrc.RF_percentual, hrc.valorTotal, hrc.created_at
   FROM historico_risco_calculado hrc
   JOIN clientes c ON hrc.clientes_id = c.id
   WHERE hrc.id IN (
       SELECT MAX(id) FROM historico_risco_calculado 
       GROUP BY clientes_id
   )
   ORDER BY c.nome;
   
   -- Testar o trigger de recálculo de perfil (Exemplo para cliente conservador)
   -- 1. Adicionar investimento em renda variável para alterar o perfil
   INSERT INTO investimento (id, ativos_id, clientes_id, quantidade, valorTotal, data)
   VALUES ('99TESTEINVEST1', '01HFAKEATIVO09', '01HXQK6Q1YXKH2QKSR89NR9A2W', 100, 50000.00, NOW());
   
   -- 2. Verificar se o perfil mudou de Conservador para Moderado
   SELECT c.nome, hrc.risco, hrc.RV_percentual, hrc.RF_percentual, hrc.valorTotal
   FROM historico_risco_calculado hrc
   JOIN clientes c ON hrc.clientes_id = c.id
   WHERE c.id = '01HXQK6Q1YXKH2QKSR89NR9A2W'
   ORDER BY hrc.created_at DESC LIMIT 2;
   
   -- Testar o trigger com alteração de investimento
   UPDATE investimento SET valorTotal = 100000.00 
   WHERE id = '99TESTEINVEST1';
   
   -- Verificar a atualização do perfil
   SELECT c.nome, hrc.risco, hrc.RV_percentual, hrc.RF_percentual, hrc.valorTotal
   FROM historico_risco_calculado hrc
   JOIN clientes c ON hrc.clientes_id = c.id
   WHERE c.id = '01HXQK6Q1YXKH2QKSR89NR9A2W'
   ORDER BY hrc.created_at DESC LIMIT 1;
   
   -- Testar o trigger com exclusão de investimento
   DELETE FROM investimento WHERE id = '99TESTEINVEST1';
   
   -- Verificar o retorno ao perfil original
   SELECT c.nome, hrc.risco, hrc.RV_percentual, hrc.RF_percentual, hrc.valorTotal
   FROM historico_risco_calculado hrc
   JOIN clientes c ON hrc.clientes_id = c.id
   WHERE c.id = '01HXQK6Q1YXKH2QKSR89NR9A2W'
   ORDER BY hrc.created_at DESC LIMIT 1;
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
   psql -U btg_user -d btg_db -f 03_procedures.sql
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
