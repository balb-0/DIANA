<Table>
  <tr>
   <td><a href= "https://www.btgpactual.com/"><img src="../../documentos/img/logo-btg.png" alt="Logo BTG Pactual" border="0"></td>
       <td>
      <a href= "https://www.inteli.edu.br/"><img src="../../documentos/img/logo-Inteli.png" alt="Inteli - Instituto de Tecnologia e Liderança" border="0"></a>
    </td>
  </tr>
</table>

# Projeto: DIANA

## Grupo: Watermelon - G3

### Dicionário do Banco de Dados 

&emsp;O dicionário de dados é um documento que descreve detalhadamente os aspectos e características dos atributos de um banco de dados, apresentando uma descrição de cada coluna, valores permitidos, observações pontuais e entre outras informações necessárias para um entendimento mais profundo. A partir disso, segue o dicionário de dados do projeto desenvolvido para o BTG Pactual:

### Tabela: `clientes`

| Variável           | Nome Variável        | Tipo de Variável | Descrição                                              | Valores Permitidos | Possui Valores nulos? | Anotações                                           |
|--------------------|----------------------|------------------|--------------------------------------------------------|--------------------|-----------------------|-----------------------------------------------------|
| id                 | ID do Cliente        | ULID             | Identificador único do cliente                         | -                  | não                   | Chave primária, auto-increment (PK)                                 |
| nome               | Nome                 | categórica       | Primeiro nome do cliente                              | -                  | não                   | -                                                   |
| cpf                | CPF                  | categórica       | Cadastro de Pessoa Física                             | -                  | não                   | -                  |
| rg                 | RG                   | categórica       | Registro Geral                                         | -                  | sim                   | -                               |
| endereco           | Endereço             | categórica       | Endereço completo do cliente                          | -                  | sim                   | -                                                   |
| data_nascimento    | Data de Nascimento   | data             | Data de nascimento do cliente                         | -                  | não                   | -                                                   |
| categoria        | Categoria            | enum       | Categoria do cliente                   | - | não                   | Definido pela organização                   |
| isActive        | Status de atividade do cliente            | booleana       | Status se o cliente está ativo ou inativo no sistema       | true, false| não                   | Definido pela organização                   |
| assessores_id      | ID do Assessor       | numérica         | Referência ao assessor responsável pelo cliente       | 1,2,3…             | sim                   | Chave estrangeira para `assessores.id`              |

---

### Tabela: `assessores`

| Variável         | Nome Variável       | Tipo de Variável | Descrição                                               | Valores Permitidos         | Possui Valores nulos? | Anotações                                        |
|------------------|---------------------|------------------|---------------------------------------------------------|----------------------------|-----------------------|--------------------------------------------------|
| id               | ID do Assessor      | ULID             | Identificador único do assessor                         | -                          | não                   | Chave primária, auto-increment (PK)                              |
| nome             | Nome                | categórica       | Primeiro nome do assessor                              | -                          | não                   | -                                                |
| unidade_btg      | Unidade BTG         | categórica       | Unidade ou time de alocação                            | -                          | sim                   | Texto livre ou, futuramente, uma associação com unidades do BTG        |

---

### Tabela: `ativos`

| Variável      | Nome Variável        | Tipo de Variável | Descrição                                      | Valores Permitidos             | Possui Valores nulos? | Anotações                        |
|---------------|----------------------|------------------|------------------------------------------------|-------------------------------|-----------------------|----------------------------------|
| id            | ID do Ativo          | ULID             | Identificador único do ativo                   | -                             | não                   | Chave primária, auto-increment (PK)             |
| nome          | Código do Ativo        | categórica       | Código do ativo             | -                             | não                   | Ex: 'PEJA11 IPCA', 'CRI CDIE'...    |
| tipo          | Tipo de Ativo        | categórica       | Categoria geral do ativo                       | - | não                   | Alinhado com classificação da CVM, ex: 'TITULOS RF PRIVADOS BRASIL', 'DEBENTURES BRASIL'... |
| subgrupo      | Subgrupo do Ativo    | categórica       | Subtipo de ativos com base em regra de negócio interna para perfis de risco   | -                             | sim                   | Ex: 'RF - Títulos Públicos Nacionais', 'RV - Ações Brasil'     |

---

&emsp;Nota: o atributo `subgrupo` possui valores calculados a partir do notebook presente e documentado em `/models/notebooks/1.classificacao_de_ativos`, sendo categorizado com base em afinidades de ativos. A criação das categorias é uma regra de negócio que pode ser evoluída futuramente, de acordo com desejos e necessidades do BTG Pactual.

### Tabela: `investimento`

| Variável      | Nome Variável       | Tipo de Variável | Descrição                                             | Valores Permitidos | Possui Valores nulos? | Anotações                                            |
|---------------|---------------------|------------------|-------------------------------------------------------|--------------------|-----------------------|------------------------------------------------------|
| id            | ID do Investimento  | ULID             | Identificador único do investimento                  | -                  | não                   | Chave primária, auto-increment (PK)                                 |
| ativos_id     | ID do Ativo         | numérica         | Referência ao ativo investido                        | 1,2,3…             | não                   | Chave estrangeira para `ativos.id`                  |
| clientes_id   | ID do Cliente       | numérica         | Referência ao cliente que fez o investimento         | 1,2,3…             | não                   | Chave estrangeira para `clientes.id`                |
| quantidade    | Quantidade          | numérica         | Quantidade de cotas ou títulos adquiridos            | ≥ 0                | não                   | Inteiro                                              |
| valorTotal    | Valor Total (R$)    | numérica         | Valor total investido na carteira (em R$)               | ≥ 0                | não                   | Pode ser usado para cálculo de carteira ponderada    |
| data   | Data de Criação      | data/hora        | Data e hora do investimento                   | -              | não                   | -                             |

---

### Tabela: `historico_risco_declarado`

| Variável      | Nome Variável        | Tipo de Variável | Descrição                                             | Valores Permitidos     | Possui Valores nulos? | Anotações                                        |
|---------------|----------------------|------------------|-------------------------------------------------------|------------------------|-----------------------|--------------------------------------------------|
| id            | ID do Registro       | ULID             | Identificador único do registro                      | -                      | não                   | Chave primária, auto-increment (PK)                             |
| risco         | Perfil Calculado      | enum             | Classificação baseada na carteira | 'Conservador', 'Moderado', 'Sofisticado' | não           | -     |
| created_at    | Data de Criação      | data/hora        | Data e hora da declaração de risco                   | ISO 8601               | não                   | Histórico versionado                             |
| clientes_id   | ID do Cliente        | numérica         | Referência ao cliente que declarou o risco           | 1,2,3…                 | não                   | FK para `clientes.id`                            |
---

### Tabela: `historico_risco_calculado`

| Variável      | Nome Variável         | Tipo de Variável | Descrição                                             | Valores Permitidos     | Possui Valores nulos? | Anotações                                         |
|---------------|-----------------------|------------------|-------------------------------------------------------|------------------------|-----------------------|---------------------------------------------------|
| id            | ID do Registro        | ULID             | Identificador único do cálculo de risco              | -                      | não                   | Chave primária, auto-increment (PK)                              |
| risco         | Perfil Calculado      | enum             | Classificação baseada na carteira | 'Conservador', 'Moderado', 'Sofisticado' | não           | -     |
| created_at    | Data de Criação       | data/hora        | Data e hora do cálculo do perfil                      | ISO 8601               | não                   | Histórico versionado                              |
| clientes_id   | ID do Cliente         | numérica         | Referência ao cliente cuja carteira foi analisada                      | 1,2,3…                 | não                   | FK para `clientes.id`                             |
| RF_percentual  | Percentual de Renda Fixa        | numérica         | Percentual de investimentos Renda Fixa na carteira | 0 ≥ x ≤ 100 | não               | Deve ser decimal                            |
| RV_percentual  | Percentual de Renda Variável        | numérica         | Percentual de investimentos Renda Variável na carteira | 0 ≥ x ≤ 100 | não               | Deve ser decimal                            |
| valorTotal    | Valor Total (R$)    | numérica         | -    | -    | não                   | -   |


&emsp;A partir deste dicionário, é possível entender as características e regras de negócio consideradas para a modelagem do banco de dados.

# Estrutura de arquivos e instruções

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