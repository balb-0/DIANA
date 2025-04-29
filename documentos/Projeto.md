<Table>
  <tr>
    <td><a href= "https://www.btgpactual.com/"><img src="img/logo-btg.png" alt="Centro Paula Souza" border="0"></td>
    <td>
      <a href= "https://www.inteli.edu.br/"><img src="img/logo-Inteli.png" alt="Inteli - Instituto de Tecnologia e Liderança" border="0"></a>
    </td>
  </tr>
</table>

# Nome do Projeto: <nome do projeto>

## Nome do Grupo: <nome do grupo>

## Integrantes:

- <a href="https://www.linkedin.com/in/username/">Nome</a>
- <a href="https://www.linkedin.com/in/username/">Nome</a>
- <a href="https://www.linkedin.com/in/username/">Nome</a>
- <a href="https://www.linkedin.com/in/username/">Nome</a>
- <a href="https://www.linkedin.com/in/username/">Nome</a>
- <a href="https://www.linkedin.com/in/username/">Nome</a>
- <a href="https://www.linkedin.com/in/username/">Nome</a>
- <a href="https://www.linkedin.com/in/username/">Nome</a>

# Sumário

- [1. Introdução](#1-introdução)
  - [1.1 Termos e Abreviações](#11-termos-e-abreviações)
  - [1.2 Objetivo do Documento](#12-objetivo-do-documento)
- [2. Entendimento do Projeto e do Negócio](#2-entendimento-do-projeto-e-do-negócio)
  - [2.1 Contexto da Indústria do Parceiro](#21-contexto-da-indústria-do-parceiro)
  - [2.2 Problema](#22-problema)
  - [2.3 Visão do Projeto e do Produto](#23-visão-do-projeto-e-do-produto)
  - [2.4 Personas e Jornada do Usuário](#24-personas-e-jornada-do-usuário)
  - [2.5 Modelagem do Fluxo de Negócio](#25-modelagem-do-fluxo-de-negócio)
    - [2.5.1 Cadeia de Valor](#251-cadeia-de-valor)
    - [2.5.2 Fluxo de Negócio Proposto (AS-IS e TO-BE)](#252-fluxo-de-negócio-proposto-as-is-e-to-be)
  - [2.6 Matriz de Risco do Projeto](#26-matriz-de-risco-do-projeto)
  - [2.7 Ideação](#27-ideação)
  - [2.8 Canvas do Projeto](#28-canvas-do-projeto)
- [3. Requisitos do Projeto](#3-requisitos-do-projeto)
  - [3.1 Requisitos Funcionais (RFs)](#31-requisitos-funcionais-rfs)
  - [3.2 Requisitos Não Funcionais (RNFs)](#32-requisitos-não-funcionais-rnfs)
  - [3.3 Correlação RFs e RNFs](#33-correlação-rfs-e-rnfs)
  - [3.4 Casos de Uso](#34-casos-de-uso)
  - [3.5 Casos de Uso x Requisitos Funcionais](#35-casos-de-uso-x-requisitos-funcionais)
- [4. Modelagem de Dados](#4-modelagem-de-dados)
  - [4.1 Especificação da Base de Dados para Modelo de Recomendação](#41-especificação-da-base-de-dados-para-modelo-de-recomendação)
  - [4.2 Modelo Conceitual de Dados](#42-modelo-conceitual-de-dados)
  - [4.3 Modelo Lógico de Dados](#43-modelo-lógico-de-dados)
  - [4.4 Modelo Físico de Dados](#44-modelo-físico-de-dados)
- [5. Solução Técnica (Design)](#5-solução-técnica-design)
  - [5.1 Diagrama de Classes](#51-diagrama-de-classes)
  - [5.2 Diagrama de Componentes da UML](#52-diagrama-de-componentes-da-uml)
  - [5.3 Diagramas de Sequência da UML](#53-diagramas-de-sequência-da-uml)
- [6. Mapeamento Técnico de Infraestrutura e Implantação](#6-mapeamento-técnico-de-infraestrutura-e-implantação)
  - [6.1 Diagrama de Implantação da UML](#61-diagrama-de-implantação-da-uml)
  - [6.2 Justificativa das Escolhas de Implantação](#62-justificativa-das-escolhas-de-implantação)
  - [6.3 Considerações sobre Desempenho e Segurança](#63-considerações-sobre-desempenho-e-segurança)
- [7. Projeto Visual da Solução](#7-projeto-visual-da-solução)
  - [7.1 Desenvolvimento de Wireframes](#71-desenvolvimento-de-wireframes)
  - [7.2 Desenvolvimento de Mockups](#72-desenvolvimento-de-mockups)
  - [7.3 Guia Visual](#73-guia-visual)
- [8. Desenvolvimento do Projeto](#8-desenvolvimento-do-projeto)
  - [8.1 Arquitetura de Codificação e Estrutura de Diretórios](#81-arquitetura-de-codificação-e-estrutura-de-diretórios)
  - [8.2 Modelo de Recomendação](#82-modelo-de-recomendação)
  - [8.3 Desenvolvimento de Features](#83-desenvolvimento-de-features)
    - [8.3.1 Sprint 3](#831-sprint-3)
    - [8.3.2 Sprint 4](#832-sprint-4)
    - [8.3.3 Sprint 5](#833-sprint-5)
  - [8.4 Testes Unitários e de Integração](#84-testes-unitários-e-de-integração)
  - [8.5 Documentações automáticas](#85-documentações-automáticas)
- [9. Planejamento e Execução de Testes](#9-planejamento-e-execução-de-testes)
  - [9.1 Testes Funcionais](#91-testes-funcionais)
    - [9.1.1 Planejamento](#911-planejamento)
    - [9.1.2 Resultados](#912-resultados)
  - [9.2 Testes de RNFs](#92-testes-de-rnfs)
    - [9.2.1 Planejamento](#921-planejamento)
    - [9.2.2 Resultados](#922-resultados)
  - [9.3 Testes de Usabilidade](#93-testes-de-usabilidade)
    - [9.3.1 Planejamento](#931-planejamento)
    - [9.3.2 Resultados](#932-resultados)
- [10. Procedimentos de Implantação](#10-procedimentos-de-implantação)
  - [10.1 Implantação e Configuração do Banco de Dados](#101-implantação-e-configuração-do-banco-de-dados)
  - [10.2 Implantação do Protótipo para uso por equipe de desenvolvimento](#102-implantação-do-protótipo-para-uso-por-equipe-de-desenvolvimento)
- [Referências](#referências)


# 1. Introdução
_conteúdo_

## 1.1 Termos e Abreviações
_conteúdo_

## 1.2 Objetivo do Documento
_conteúdo_

# 2. Entendimento do Projeto e do Negócio
_conteúdo_

## 2.1 Contexto da Indústria do Parceiro
_conteúdo_

## 2.2 Problema

&emsp;Entre os produtos do BTG Pactual, existem as carteiras de investimento, uma opção vantajosa para clientes que, por exemplo, não possuem disponibilidade ou interesse em realizar a gestão ativa de suas operações de compra e venda. Essas carteiras são administradas por gestores de fundos de investimento, realizando operações no em nome do investidor. Nesse contexto, outro importante papel é o do assessor, que elabora e comunica estratégias de investimento personalizadas, considerando as necessidades do cliente e as oportunidades do mercado financeiro, mantendo um contato direto com o investidor final.

&emsp;Cada cliente que inicia a sua jornada de investimentos preenche um formulário para a compreensão de seu grupo de risco, classificando-o como conservador, moderado ou sofisticado. O perfil conservador prioriza segurança e liquidez, evitando o máximo de riscos; o moderado busca equilíbrio entre risco e retorno, aceitando variações moderadas; já o sofisticado (ou arrojado) visa alta rentabilidade e tolera grandes oscilações no capital. Vale ressaltar que um investidor pode possuir ativos associados a diferentes perfis de risco sem que isso, por si só, altere seu perfil cadastrado ou impeça o acesso a determinados produtos.

&emsp;Atualmente, o banco oferece um sistema de recomendação de produtos de carteiras já disponíveis com base nos diferentes perfis de investimento/grupo de risco, entretanto, em variados casos, uma carteira de um cliente — após determinado período de operações diferentes — pode possuir investimentos que o inclinem para um perfil de risco distinto daquele originalmente atribuído. Nesse sentido, não há, no momento, uma solução automatizada capaz de realizar a leitura da carteira atual, reclassificar o perfil do investidor de forma automática e baseada nas regras definidas pela CVM (como estabelecido na Resolução CVM 175 e outros normativos aplicáveis), além de, se necessário, recomendar ao assessor uma atualização de perfil. Essa recomendação seria direcionada exclusivamente ao assessor ou gestor, e não ao cliente final, respeitando as diretrizes da CVM que proíbem induções diretas de perfil ao investidor.

&emsp;O desenvolvimento de tal solução representa uma oportunidade muito relevante para o BTG Pactual em diversas frentes. Em primeiro lugar, contribui para a redução do risco regulatório ao identificar possíveis descasamentos entre os perfis de risco cadastrados e a alocação real das carteiras, promovendo maior conformidade com as normativas da Comissão de Valores Mobiliários (CVM). Além disso, ao automatizar a leitura e análise de milhares de carteiras, o sistema proporciona agilidade nos processos de auditoria interna, facilitando a verificação contínua e sistemática da aderência às diretrizes regulatórias. A solução também fortalece a governança de suitability, ao oferecer suporte direto ao assessor no monitoramento da adequação entre os produtos recomendados e o perfil de cada cliente.

&emsp;Outro benefício relevante é a melhoria da transparência no relacionamento com os investidores, já que a atuação do assessor passa a ser embasada por dados objetivos de conformidade. Por fim, a ferramenta poderá servir como base para sistemas futuros de recomendação automatizada de realocação de carteiras, bem como pode ser oferecida como White Label a um mercado de assessoria de investimentos, indicando caminhos de ajuste que mantenham o investidor dentro de seu perfil declarado, sem comprometer as oportunidades de crescimento e retorno.

## 2.3 Visão do Projeto e do Produto
  _conteúdo_

  **Nota**: _Insira aqui informações sobre o que se trata o projeto e que valor ele vai entregar, Objetivos do Produto e O que o produto faz e não faz._

## 2.4 Personas e Jornada do Usuário
_conteúdo_

## 2.5 Modelagem do Fluxo de Negócio
_conteúdo_

## 2.5.1 Cadeia de Valor
_conteúdo_

## 2.5.2 Fluxo de Negócio Proposto (AS-IS e TO-BE)
_conteúdo_

## 2.6 Matriz de Risco do Projeto
_conteúdo_

## 2.7 Ideação

&emsp;Neste estágio inicial de planejamento da aplicação, foi feito um *brainstorm* de funcionalidades que podem ser oferecidas pelo sistema, sendo alinhadas com o entendimento do negócio e do projeto. Foram elencadas 10 funcionalidades, listadas por esforço e prioridade em relação às necessidades do BTG Pactual e o prazo de desenvolvimento da solução.

### Brainstorm de Features

| # | Feature | Descrição | Prioridade | Esforço |
|:---|:---|:---|:---| :---|
| 1 | Leitura de carteira | Leitura de componentes de carteira de investimentos | Alta | Médio/baixo |
| 2 | Análise de Conformidade da Carteira | Avaliação automática se a carteira está adequada ao perfil de risco do cliente. | Alta | Médio |
| 3 | Recomendação de Readequação de Perfil | Caso detectada divergência, gerar sugestão de novo perfil do cliente ao assessor. | Alta | Alto |
| 4 | Cálculo de Peso dos Ativos na Carteira | Cálculo ponderado dos tipos de investimento para identificação do perfil. | Alta | Médio |
| 5 | Painel de Visualização de Conformidade | Página para os assessores visualizarem status de todas as carteiras (adequadas ou não). | Alta | Médio |
| 6 | Filtro por Necessidade de Realocação | Recurso de filtragem por carteiras divergentes com grupo de risco original, que resultará na listagem de carteiras com necessidade de realocação de perfil. | Alta | Médio |
| 7 | Filtro por Segmento | Permitir ao usuário filtrar os clientes por segmento (exemplo: Prata, Ouro, Platinum). | Média | Baixo |
| 9 | Histórico de Readequações de Perfil | Registro das mudanças/sugestões de perfil ao longo do tempo para cada cliente. | Média | Alto |
| 10 | Visualização de Pesos dos Ativos | Exibição visual de distribuição percentual por classe de ativos (gráfico pizza/barras). | Baixa | Médio |

&emsp;Dessa forma, a ideação de features foi um passo importante para discussão e concretização de ideias iniciais a serem concretizadas como decisões da equipe. Além disso, a ideação orientará a elaboração dos Requisitos Funcionais e Não Funcionais, base do desenvolvimento do projeto.

## 2.8 Canvas do Projeto
_conteúdo_

# 3. Requisitos do Projeto
_conteúdo_

## 3.1 Requisitos Funcionais (RFs)
_conteúdo_ 

## 3.2 Requisitos Não Funcionais (RNFs)
_conteúdo_

## 3.3 Correlação RFs e RNFs
_conteúdo_

## 3.4 Casos de Uso
_conteúdo_

## 3.5 Casos de Uso x Requisitos Funcionais
_conteúdo_

# 4. Modelagem de Dados
_conteúdo_

## 4.1 Especificação da Base de Dados para Modelo de Recomendação
_conteúdo_
Obs.: Insira aqui informações sobre a construção do artefato da Sprint 1. 

## 4.2 Modelo Conceitual de Dados
_conteúdo_

## 4.3 Modelo Lógico de Dados
_conteúdo_

## 4.4 Modelo Físico de Dados
_conteúdo_

**Nota:** Insira uma explicação e direcionamento para o readme.md da pasta database.

# 5. Solução Técnica (Design)
_conteúdo_

## 5.1 Diagrama de Classes
_conteúdo_

## 5.2 Diagrama de Componentes da UML
_conteúdo_

## 5.3 Diagramas de Sequência da UML
_conteúdo_

# 6. Mapeamento Técnico de Infraestrutura e Implantação
_conteúdo_

## 6.1 Diagrama de Implantação da UML
_conteúdo_

## 6.2 Justificativa das Escolhas de Implantação
_conteúdo_

## 6.3 Considerações sobre Desempenho e Segurança
_conteúdo_

# 7. Projeto Visual da Solução
_conteúdo_

## 7.1 Desenvolvimento de Wireframes
_conteúdo_

## 7.2 Desenvolvimento de Mockups
_conteúdo_

## 7.3 Guia Visual
_conteúdo_

# 8. Desenvolvimento do Projeto
_conteúdo_

## 8.1 Arquitetura de Codificação e Estrutura de Diretórios
_conteúdo_

## 8.2 Modelo de Recomendação
_conteúdo_

## 8.3 Desenvolvimento de Features
_conteúdo_

**Nota:** Insira uma explicação de entregas em cada Sprint.

## 8.3.1 Sprint 3
_conteúdo_

## 8.3.2 Sprint 4
_conteúdo_

## 8.3.3 Sprint 5
_conteúdo_

## 8.4 Testes Unitários e de Integração
_conteúdo_
Obs.: Insira informações sobre estratégias para realização dos testes

## 8.5 Documentações automáticas
_conteúdo_
Obs.: Insira informações sobre quais frameworks foram usados e como acessar.

# 9. Planejamento e Execução de Testes
_conteúdo_

## 9.1 Testes Funcionais
_conteúdo_

## 9.1.1 Planejamento
_conteúdo_

## 9.1.2 Resultados
_conteúdo_

## 9.2 Testes de RNFs
_conteúdo_

## 9.2.1 Planejamento
_conteúdo_

## 9.2.2 Resultados
_conteúdo_

## 9.3 Testes de Usabilidade
_conteúdo_

## 9.3.1 Planejamento
_conteúdo_

## 9.3.2 Resultados
_conteúdo_

# 10. Procedimentos de Implantação
_conteúdo_

## 10.1 Implantação e Configuração do Banco de Dados
_conteúdo_

## 10.2 Implantação do Protótipo para uso por equipe de desenvolvimento
_conteúdo_

# Referências
_conteúdo_

