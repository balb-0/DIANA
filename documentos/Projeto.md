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
_conteúdo_

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
_conteúdo_
Obs.: Seção dedicada a discussão sobre ideias e priorização de features.

## 2.8 Canvas do Projeto
_conteúdo_

# 3. Requisitos do Projeto
_conteúdo_

## 3.1 Requisitos Funcionais (RFs)
_conteúdo_ 

## 3.2 Requisitos Não Funcionais (RNFs)

<div align="center">
<sub>Tabela X - Requisitos não funcionais</sub><br>


| **RNF#** | **Descrição**                                                                                                                   | **Aspecto de Qualidade (ISO 25010)** | **Prioridade** |
|---------|-----------------------------------------------------------------------------------------------------------------------------------|--------------------------------------|----------------|
| RNF01   | O sistema deve responder às requisições de recomendação baseadas em machine learning em até 2 segundos, mesmo sob condições normais de operação. | Desempenho e Eficiência              | Média          |
| RNF02   | O sistema deve suportar pelo menos 100 acessos simultâneos, mantendo o tempo de resposta médio de requisições padrão (não-ML) abaixo de 1 segundo por requisição. | Desempenho e Eficiência              | Média          |
| RNF03   | O sistema deve expor métricas de desempenho por meio de um endpoint dedicado. | Desempenho e Eficiência              | Média          |
| RNF04   | O sistema deve ser containerizado e projetado para rodar exclusivamente em ambientes de nuvem, garantindo portabilidade e escalabilidade. | Portabilidade                        | Alta           |
| RNF05   | O app deve ser desenvolvido exclusivamente para Android (versão 10 ou superior).                                                 | Compatibilidade                      | Baixa          |
| RNF06   | O sistema deve usar APIs RESTful bem documentadas e versionadas, com autenticação via provedor de terceiros (como Auth0 ou Firebase), garantindo segurança e compatibilidade entre os componentes. | Compatibilidade                      | Média          |
| RNF07   | A estrutura do aplicativo deve ser organizada de forma modular, com responsabilidades claras e desacopladas entre os componentes, permitindo a evolução do código com baixo impacto em outras partes do sistema. | Manutenibilidade                     | Alta           |
| RNF08   | O sistema de recomendação (ML) deve rodar em infraestrutura separada do backend do app, sendo acessado exclusivamente via API. Essa separação deve permitir manutenção independente entre os dois sistemas. | Manutenibilidade                     | Alta           |
| RNF09   | A comunicação entre o app, o backend e o sistema de recomendação deve ser feita via HTTPS com TLS 1.2 ou superior.               | Segurança                            | Alta           |
| RNF10   | A autenticação de acesso ao app deve ser feita com uso de autenticação multifator (MFA).                                         | Segurança                            | Média          |
| RNF11   | Dados sensíveis eventualmente armazenados localmente devem estar criptografados utilizando criptografia simétrica forte.         | Segurança                            | Alta           |
| RNF12   | Em falha do sistema de recomendação, o app deve mostrar mensagem clara e oferecer ao menos 1 sugestão padrão, além de opção para tentar novamente. | Confiabilidade                       | Alta           |
| RNF13   | O sistema deve ter disponibilidade de no mínimo 99,5% mensal.                                                                    | Confiabilidade                       | Baixa          |
| RNF14   | O modelo de ML deve ser re-treinado semanalmente por meio de um cron job automatizado com dados atualizados.                    | Funcionalidade Adequada              | Baixa          |
| RNF15   | A exibição das recomendações deve seguir boas práticas de usabilidade (como as heurísticas de Nielsen) e ser validada com testes de usabilidade com usuários reais. | Usabilidade                          | Média          |

<sup>Material produzido pelos autores (2025)</sup>
</div><br>

### Tabela de Testes para Requisitos Não Funcionais

<div align="center">
<sub>Tabela X - Testes associados aos Requisitos Não Funcionais</sub><br>

| **Caso de Teste**       | **RNF#** | **Descrição do Teste**                                                                                      | **Pré-condição**                                              | **Caso de Aceite**                                                                 | **Caso de Recusa**                                                                     |
|-------------------------|---------|-------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------|------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------|
| CT-RNF01                | RNF01   | Medir o tempo de resposta de uma recomendação via ML sob carga normal de uso.                              | Servidor ativo com modelo treinado e base de dados populada.  | Tempo de resposta ≤ 2 segundos.                                                   | Tempo de resposta > 2 segundos.                                                        |
| CT-RNF02                | RNF02   | Simular 100 acessos simultâneos e medir o tempo médio de resposta das requisições padrão (não-ML).          | Sistema em ambiente de teste com suporte a concorrência.      | Tempo médio de resposta < 1 segundo por requisição.                               | Tempo médio de resposta ≥ 1 segundo.                                                   |
| CT-RNF03                | RNF03   | Verificar se endpoint `/metrics` existe e retorna dados compatíveis com ferramentas de monitoramento.                       | O componente a ser testado está em execução e acessível na rede.                         | Endpoint acessível e com retorno estruturado.                                     | Endpoint inexistente ou com erro/estrutura inválida.                                   |
| CT-RNF04                | RNF04   | Verificar se todos os serviços rodam em containers e estão hospedados em ambiente de nuvem.                 | Sistema implantado.                                            | Todos os containers ativos em ambiente cloud.                                     | Componentes rodando fora de containers ou fora da nuvem.                               |
| CT-RNF05                | RNF05   | Verificar se o app é compatível apenas com Android 10 ou superior.                                          | APK instalado em diferentes versões do Android.               | Funciona em Android 10+, falha em versões anteriores.                            | Funciona em Android < 10 ou não funciona em Android ≥ 10                                                            |
| CT-RNF06                | RNF06   | Verificar se as APIs REST estão documentadas, versionadas e autenticadas por provedor externo.              | Ambiente de desenvolvimento com APIs expostas.                 | Documentação disponível, uso de Auth0 ou Firebase ativo.                         | Falta documentação, ausência de versionamento ou autenticação manual.                  |
| CT-RNF07                | RNF07   | Verificar a modularidade do sistema inspecionando a arquitetura e código-fonte.                             | Código-fonte acessível e estruturado.                          | Componentes independentes e bem separados.                                       | Código acoplado e sem separação clara de responsabilidades.                            |
| CT-RNF08                | RNF08   | Verificar se o sistema de recomendação está em infraestrutura separada e acessível via API.                 | Backend e ML ativos.                                           | Comunicação feita exclusivamente por API entre backend e sistema de ML.          | Integração direta sem separação física/lógica entre os sistemas.                       |
| CT-RNF09                | RNF09   | Testar comunicação entre app e backend/sistema de ML com Wireshark e inspecionar se é feita via HTTPS com TLS ≥ 1.2.    | Servidores configurados com TLS.                              | Certificado válido e conexão segura.                                             | Conexão HTTP ou TLS inferior a 1.2.                                                     |
| CT-RNF10                | RNF10   | Testar o fluxo de autenticação com MFA ativado.                                                             | Usuário com MFA configurado.                                  | MFA solicitado e validado corretamente.                                          | Login sem solicitação de segundo fator ou falha de verificação.                        |
| CT-RNF11                | RNF11   | Simular armazenamento local de dados sensíveis e verificar criptografia.                                   | Dispositivo com dados armazenados.                            | Dados criptografados com algoritmo simétrico forte (ex: AES-256).               | Dados em texto plano ou criptografia fraca.                                            |
| CT-RNF12                | RNF12   | Simular falha no sistema de recomendação e observar comportamento do app.                                   | Sistema de ML offline.                                        | App exibe mensagem amigável e sugestão padrão, com botão "Tentar novamente".     | App trava ou exibe erro genérico.                                                      |
| CT-RNF13                | RNF13   | Verificar logs de uptime e incidentes no último mês.                                                        | Sistema em produção.                                           | Uptime ≥ 99,5% no período analisado.                                             | Uptime < 99,5%.                                                                        |
| CT-RNF14                | RNF14   | Inspecionar logs do cron job de re-treinamento e verificar execução semanal.                                | Base de dados com histórico de execuções.                      | Execução semanal registrada sem falhas.                                          | Execução não ocorre ou ocorre com erro/frequência incorreta.                          |
| CT-RNF15                | RNF15   | Avaliar a interface de recomendações com base nas heurísticas de Nielsen e testes com pelo menos 5 usuários reais.       | Protótipo funcional e grupo de usuários disponíveis.            | Feedback positivo ≥ 80% em testes.                                               | Reclamações recorrentes ou reprovação nos testes.                                      |

<sup>Fonte: Material produzido pelos autores (2025)</sup>
</div>


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

