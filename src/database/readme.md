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
| email              | E-mail               | categórica       | Endereço de e-mail do cliente                          | -                  | não                   | Deve ser único                                      |
| telefone           | Telefone             | categórica       | Telefone de contato                                    | -                  | sim                   | -                                                   |
| nome               | Nome                 | categórica       | Primeiro nome do cliente                              | -                  | não                   | -                                                   |
| sobrenome          | Sobrenome            | categórica       | Sobrenome completo                                     | -                  | não                   | -                                                   |
| cpf                | CPF                  | categórica       | Cadastro de Pessoa Física                             | -                  | não                   | Deve ser único                                      |
| rg                 | RG                   | categórica       | Registro Geral                                         | -                  | sim                   | Deve ser único                                                   |
| endereco           | Endereço             | categórica       | Endereço completo do cliente                          | -                  | sim                   | -                                                   |
| data_nascimento    | Data de Nascimento   | data             | Data de nascimento do cliente                         | -                  | não                   | -                                                   |
| saldo_carteira     | Saldo da Carteira    | numérica         | Valor total (R$) da carteira de investimentos         | -                | sim                   | Pode ser calculado com base nos investimentos       |
| assessores_id      | ID do Assessor       | numérica         | Referência ao assessor responsável pelo cliente       | 1,2,3…             | sim                   | Chave estrangeira para `assessores.id`              |

---

### Tabela: `assessores`

| Variável         | Nome Variável       | Tipo de Variável | Descrição                                               | Valores Permitidos         | Possui Valores nulos? | Anotações                                        |
|------------------|---------------------|------------------|---------------------------------------------------------|----------------------------|-----------------------|--------------------------------------------------|
| id               | ID do Assessor      | ULID             | Identificador único do assessor                         | -                          | não                   | Chave primária, auto-increment (PK)                              |
| email            | E-mail              | categórica       | Endereço de e-mail do assessor                          | -                          | não                   | Deve ser único                                   |
| telefone         | Telefone            | categórica       | Telefone de contato                                     | -                          | sim                   | -                                                |
| nome             | Nome                | categórica       | Primeiro nome do assessor                              | -                          | não                   | -                                                |
| sobrenome        | Sobrenome           | categórica       | Sobrenome completo do assessor                          | -                          | não                   | -                                                |
| cpf              | CPF                 | categórica       | Cadastro de Pessoa Física                               | -                          | não                   | Deve ser único                                   |
| rg               | RG                  | categórica       | Registro Geral                                          | -                          | sim                   | Deve ser único                                                |
| endereco         | Endereço            | categórica       | Endereço completo                                       | -                          | sim                   | -                                                |
| data_nascimento  | Data de Nascimento  | data             | Data de nascimento                                      | -                          | sim                   | -                                                |
| categoria        | Categoria            | categórica       | Nível do assessor dentro da empresa                     | 'Gold', 'Prata', 'Platinum'| não                   | Enum definido pela organização                   |
| unidade_btg      | Unidade BTG         | categórica       | Unidade ou time de alocação                            | -                          | sim                   | Texto livre ou, futuramente, uma associação com unidades do BTG        |

---

### Tabela: `ativos`

| Variável      | Nome Variável        | Tipo de Variável | Descrição                                      | Valores Permitidos             | Possui Valores nulos? | Anotações                        |
|---------------|----------------------|------------------|------------------------------------------------|-------------------------------|-----------------------|----------------------------------|
| id            | ID do Ativo          | ULID             | Identificador único do ativo                   | -                             | não                   | Chave primária, auto-increment (PK)             |
| nome          | Nome do Ativo        | categórica       | Código do ativo             | -                             | não                   | Ex: 'PEJA11 IPCA', 'CRI CDIE'...    |
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

---

### Tabela: `historico_risco_declarado`

| Variável      | Nome Variável        | Tipo de Variável | Descrição                                             | Valores Permitidos     | Possui Valores nulos? | Anotações                                        |
|---------------|----------------------|------------------|-------------------------------------------------------|------------------------|-----------------------|--------------------------------------------------|
| id            | ID do Registro       | ULID             | Identificador único do registro                      | -                      | não                   | Chave primária, auto-increment (PK)                             |
| risco         | Perfil Declarado     | JSON             | Estrutura JSON com a classificação declarada         | Ex: `{"perfil": "Moderado"}` | não         | Volátil. Pode conter atributos adicionais                |
| created_at    | Data de Criação      | data/hora        | Data e hora da declaração de risco                   | ISO 8601               | não                   | Histórico versionado                             |
| clientes_id   | ID do Cliente        | numérica         | Referência ao cliente que declarou o risco           | 1,2,3…                 | não                   | FK para `clientes.id`                            |

---

### Tabela: `historico_risco_calculado`

| Variável      | Nome Variável         | Tipo de Variável | Descrição                                             | Valores Permitidos     | Possui Valores nulos? | Anotações                                         |
|---------------|-----------------------|------------------|-------------------------------------------------------|------------------------|-----------------------|---------------------------------------------------|
| id            | ID do Registro        | ULID             | Identificador único do cálculo de risco              | -                      | não                   | Chave primária, auto-increment (PK)                              |
| risco         | Perfil Calculado      | JSON             | Estrutura JSON com a classificação baseada na carteira | Ex: `{"perfil": "Arrojado"}` | não           | Pode conter pesos, notas, justificativas         |
| created_at    | Data de Criação       | data/hora        | Data e hora do cálculo do perfil                      | ISO 8601               | não                   | Histórico versionado                              |
| clientes_id   | ID do Cliente         | numérica         | Referência ao cliente cuja carteira foi analisada                      | 1,2,3…                 | não                   | FK para `clientes.id`                             |

&emsp;A partir deste dicionário, é possível entender as características e regras de negócio consideradas para a modelagem do banco de dados.