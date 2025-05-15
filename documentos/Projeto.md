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

- <a href="https://www.linkedin.com/in/anacdejesus/">Ana Carolina Pacheco</a>
- <a href="https://www.linkedin.com/in/felipe-zillo-72b367247/">Felipe Zillo</a>
- <a href="https://www.linkedin.com/in/gabriel-nascimento-563382243/">Gabriel Nascimento</a>
- <a href="https://www.linkedin.com/in/joao-souza-campos/">João Victor de Souza Campos</a>
- <a href="https://www.linkedin.com/in/lucas-tort/">Lucas Cozzolino Tort</a>
- <a href="https://www.linkedin.com/in/natalycunha/">Nataly Cunha</a>
- <a href="https://www.linkedin.com/in/thiagovolcati/">Thiago Volcati</a>
- <a href="https://www.linkedin.com/in/vitor-balbo/">Vitor Balbo</a>

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
  - [2.9 Estimativa de Investimento](#29-estimativa-de-investimento)
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
  - [5.4 Tecnologias e Linguagens](#54-tecnologias-e-linguagens)
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

&emsp;Nesta subseção reúnem-se as abreviações de palavras, termos comuns a serem discutidos na documentação e, por fim, siglas.

### Siglas e abreviações
| Sigla/Abreviação | Significado |
|------------------|-------------|
| **API**          | *Application Programming Interface* (Interface de Programação de Aplicações) |
| **BPM**          | *Business Process Management* (Gerenciamento de Processos de Negócio) |
| **BTG**          | BTG Pactual |
| **CSV**          | *Comma-Separated Values* (Formato de arquivo separado por vírgulas) |
| **CVM**          | Comissão de Valores Mobiliários |
| **IDE**          | *Integrated Development Environment* (Ambiente de Desenvolvimento Integrado) |
| **ISO/IEC 25010**| Norma internacional de requisitos de qualidade de software |
| **KPI**          | *Key Performance Indicator* (Indicador-chave de desempenho) |
| **ML**           | *Machine Learning* (Aprendizado de Máquina) |
| **MFA**          | *Multi-Factor Authentication* (Autenticação Multifator) |
| **PO**           | *Product Owner* |
| **QA**           | *Quality Assurance* (Garantia de Qualidade) |
| **RF** / **RFs** | Requisito(s) Funcional(is) |
| **RNF** / **RNFs**| Requisito(s) Não Funcional(is) |
| **SSL**          | *Secure Sockets Layer* |
| **Swagger**      | Ferramenta para documentação de APIs |
| **TLS**          | *Transport Layer Security* |
| **UML**          | *Unified Modeling Language* |
| **UX/UI**        | *User Experience* / *User Interface* |

### Termos Técnicos e de Projeto

| Termo                        | Significado |
|-----------------------------|-------------|
| **Adequação Regulátoria**   | Conformidade com normas e regras de órgãos reguladores como a CVM |
| **AS-IS / TO-BE**           | Estado atual / Estado proposto de um processo |
| **Cloud / Nuvem**           | Infraestrutura de computação remota |
| **Compliance**              | Conformidade com políticas, leis e regulamentos |
| **Dashboard**               | Painel de visualização interativa de dados |
| **Feature**                 | Funcionalidade de um sistema |
| **Heurísticas de Nielsen**  | Princípios de usabilidade para interfaces |
| **Mockup**                  | Protótipo visual com detalhes de design |
| **Modularidade**            | Organização em partes independentes |
| **Painel Interativo**       | Visualização dinâmica de dados com filtros e status |
| **Personas**                | Perfis fictícios de usuários representativos |
| **Sprint**                  | Iteração de desenvolvimento em metodologia ágil |
| **Suitability**             | Adequação entre produto e perfil do investidor |
| **User Story**              | Descrição de funcionalidade sob o ponto de vista do usuário |
| **White Label**             | Produto que pode ser personalizado e utilizado por terceiros |
| **Wireframe**               | Esboço inicial da estrutura da interface |

## 1.2 Objetivo do Documento

&emsp;Este documento visa consolidar o entendimento do parceiro e do projeto, orientando seu desenvolvimento técnico com base nas necessidades do parceiro e dos usuários finais, bem como detalhar os planejamentos e execuções do desenvolvimento do *aplicativo mobile* e testes associados.

# 2. Entendimento do Projeto e do Negócio

&emsp;Para garantir que o aplicativo desenvolvido atenda com precisão às necessidades do BTG Pactual, esta seção apresenta uma análise aprofundada do setor de atuação, do problema compreendido pela equipe, da proposta de solução e dos perfis dos usuários envolvidos. O objetivo é alinhar o entendimento técnico com a realidade e os processos de negócio, bem como fornecer uma base sólida para o desenvolvimento orientado à geração de valor.

## 2.1 Contexto da Indústria do Parceiro

&nbsp;&nbsp; O setor de bancos de investimento e gestão de ativos no Brasil é uma das áreas mais dinâmicas e estratégicas do sistema financeiro, movimentando diariamente centenas de bilhões de reais em operações de subscrição de títulos, fusões e aquisições, ofertas públicas e gestão de carteiras. Atualmente, mais de 2,5 milhões de investidores pessoa física mantêm carteiras ativas em plataformas de investimento no país, somados a grandes investidores institucionais, como fundos de pensão e family offices, que canalizam recursos para o mercado de capitais e private equity. A indústria conta com cerca de 8 mil fundos registrados na Comissão de Valores Mobiliários (CVM), cujo patrimônio líquido ultrapassa os R$ 3 trilhões, sendo que fundos de ações e multimercados representam mais de 40% desse total, refletindo a busca constante por diversificação e por retornos superiores aos da renda fixa, especialmente em um cenário de juros ainda elevados.

&nbsp;&nbsp; O ambiente competitivo e regulatório em que esses bancos operam é complexo e desafiador. A regulação da CVM, com normativas recentes como a Resolução 175, enfatiza a necessidade de transparência, rigor e adequação dos produtos financeiros aos perfis de risco dos investidores, reforçando a importância da governança e do compliance. Paralelamente, a crescente digitalização do setor vem transformando a forma como os produtos são recomendados e distribuídos, com fintechs e plataformas digitais, como a XP, ganhando espaço ao oferecer soluções baseadas em inteligência artificial e machine learning para recomendações personalizadas e ajuste dinâmico das carteiras. Essa inovação tecnológica, por sua vez, pressiona os bancos tradicionais a investirem em sistemas de recomendação cada vez mais sofisticados, capazes de analisar em tempo real o comportamento e o perfil dos investidores, evitando desalinhamentos que podem levar à perda de clientes e a riscos regulatórios.

&nbsp;&nbsp; Nesse cenário, o BTG Pactual se destaca como o maior banco de investimentos independente da América Latina, consolidando sua posição por meio de uma atuação focada em operações complexas, como subscrição de dívidas corporativas, fusões e aquisições e gestão de recursos de alta renda. Embora concorra diretamente com grandes bancos como Itaú BBA, Bradesco BBI e Santander, além da crescente concorrência de fintechs, o BTG diferencia-se pelo acesso a mercados globais e pela capacidade de estruturar produtos exclusivos e sofisticados, alinhados com o perfil dos investidores mais exigentes. Sua posição reforça a importância de modelos de recomendação eficientes, que integrem dados robustos e permitam ajustar carteiras conforme mudanças de mercado e do comportamento dos clientes, mantendo-os alinhados ao seu perfil de risco.

&nbsp;&nbsp; O setor enfrenta desafios relevantes, como o aumento dos ataques cibernéticos, que cresceram cerca de 30% entre 2022 e 2023, o que demanda investimentos contínuos em segurança da informação para proteger dados sensíveis e garantir a integridade dos sistemas de recomendação. Além disso, a educação financeira dos investidores ainda é uma lacuna importante, considerando que um grande número de investidores iniciantes não compreende completamente os riscos envolvidos em produtos mais complexos, o que reforça a necessidade de processos de recomendação transparentes e responsáveis. Juntas, essas exigências moldam um mercado que precisa equilibrar inovação tecnológica, cumprimento regulatório e atendimento personalizado, onde o papel dos assessores financeiros continua sendo essencial para guiar o investidor em sua jornada.

&nbsp;&nbsp; Assim, o sucesso na indústria brasileira de bancos de investimento e gestão de ativos depende da capacidade dos players em combinar tecnologia avançada, profundo conhecimento do perfil do investidor brasileiro e sólida governança. Essa combinação é fundamental para garantir a recomendação precisa de produtos, a fidelização dos clientes e a mitigação de riscos no ambiente regulatório e competitivo. É nesse contexto que o BTG Pactual demonstra sua força, ao integrar inovação tecnológica com experiência de mercado para manter sua liderança em um segmento que evolui rapidamente e exige constante adaptação.

## 2.2 Problema

&emsp;Entre os produtos do BTG Pactual, existem as carteiras de investimento, uma opção vantajosa para clientes que, por exemplo, não possuem disponibilidade ou interesse em realizar a gestão ativa de suas operações de compra e venda. Essas carteiras são administradas por gestores de fundos de investimento, realizando operações no em nome do investidor. Nesse contexto, outro importante papel é o do assessor, que elabora e comunica estratégias de investimento personalizadas, considerando as necessidades do cliente e as oportunidades do mercado financeiro, mantendo um contato direto com o investidor final.

&emsp;Cada cliente que inicia a sua jornada de investimentos preenche um formulário para a compreensão de seu grupo de risco, classificando-o como conservador, moderado ou sofisticado. O perfil conservador prioriza segurança e liquidez, evitando o máximo de riscos; o moderado busca equilíbrio entre risco e retorno, aceitando variações moderadas; já o sofisticado (ou arrojado) visa alta rentabilidade e tolera grandes oscilações no capital. Vale ressaltar que um investidor pode possuir ativos associados a diferentes perfis de risco sem que isso, por si só, altere seu perfil cadastrado ou impeça o acesso a determinados produtos.

&emsp;Atualmente, o banco oferece um sistema de recomendação de produtos de carteiras já disponíveis com base nos diferentes perfis de investimento/grupo de risco, entretanto, em variados casos, uma carteira de um cliente — após determinado período de operações diferentes — pode possuir investimentos que o inclinem para um perfil de risco distinto daquele originalmente atribuído. Nesse sentido, não há, no momento, uma solução automatizada capaz de realizar a leitura da carteira atual, reclassificar o perfil do investidor de forma automática e baseada nas regras definidas pela CVM (como estabelecido na Resolução CVM 175 e outros normativos aplicáveis), além de, se necessário, recomendar ao assessor uma atualização de perfil. Essa recomendação seria direcionada exclusivamente ao assessor ou gestor, e não ao cliente final, respeitando as diretrizes da CVM que proíbem induções diretas de perfil ao investidor.

&emsp;O desenvolvimento de tal solução representa uma oportunidade muito relevante para o BTG Pactual em diversas frentes. Em primeiro lugar, contribui para a redução do risco regulatório ao identificar possíveis descasamentos entre os perfis de risco cadastrados e a alocação real das carteiras, promovendo maior conformidade com as normativas da Comissão de Valores Mobiliários (CVM). Além disso, ao automatizar a leitura e análise de milhares de carteiras, o sistema proporciona agilidade nos processos de auditoria interna, facilitando a verificação contínua e sistemática da aderência às diretrizes regulatórias. A solução também fortalece a governança de suitability, ao oferecer suporte direto ao assessor no monitoramento da adequação entre os produtos recomendados e o perfil de cada cliente.

&emsp;Outro benefício relevante é a melhoria da transparência no relacionamento com os investidores, já que a atuação do assessor passa a ser embasada por dados objetivos de conformidade. Por fim, a ferramenta poderá servir como base para sistemas futuros de recomendação automatizada de realocação de carteiras, bem como pode ser oferecida como White Label a um mercado de assessoria de investimentos, indicando caminhos de ajuste que mantenham o investidor dentro de seu perfil declarado, sem comprometer as oportunidades de crescimento e retorno.

## 2.3 Visão do Projeto e do Produto

&nbsp;&nbsp;&nbsp;&nbsp;O projeto desenvolvido para o time do BTG Pactual consiste em um aplicativo direcionado para gestores e assessores que será capaz de analisar os perfis dos clientes e compará-los com suas carteiras de investimento atuais. Com isso, será possível identificar eventuais inconsistências entre o perfil do investidor e os ativos que ele possui, permitindo uma sugestão de adequação quando necessário. Em decorrência disso, esse projeto gera valor tanto para os assessores, que terão um controle mais preciso sobre a carteira de seus clientes e poderão orientá-los de forma mais eficiente, quanto para o investidor final, que terá uma carteira mais alinhada ao seu perfil, promovendo uma melhor gestão de risco e maior potencial de retorno de acordo com suas preferências e tolerâncias.

&nbsp;&nbsp;&nbsp;&nbsp;O produto tem como principal objetivo analisar o perfil e a carteira de investimentos dos clientes. Caso sejam encontradas incompatibilidades, o sistema irá sugerir ajustes para garantir que os ativos estejam coerentes com o perfil do investidor.

&nbsp;&nbsp;&nbsp;&nbsp;A seguir, um template com informações sobre o que o nosso sistema é, o que não é, o que faz e o que não faz.

<br>
<div align="center">
<sub>Figura x - Faz ou não faz</sub>
<br>
<br>
<img src="../documentos/img/faz-naofaz.jpg" alt='Imagem com a descrição do que o nosso aplicativo faz ou não faz' width="100%">
<br>
<br>
<sup>Fonte: Material produzido pelos autores (2025)</sup>
</div>
<br>

## 2.4 Personas e Jornada do Usuário

A definição de personas e a construção das respectivas jornadas de usuário são etapas fundamentais no processo de design centrado no ser humano. Elas ajudam a guiar as decisões de desenvolvimento, garantindo que o produto atenda de forma eficaz às reais necessidades dos usuários finais.

### Persona Assessor

<img src="../documentos/img/persona-assessor.svg" width="100%">

### Persona Cliente

<img src="../documentos/img/persona-cliente.svg" width="100%">

### Jornada de usuário do Assessor

<img src="../documentos/img/antes-01.svg" width="100%">
<img src="../documentos/img/depois01.svg" width="100%">


### Jornada de usuário da Cliente

<img src="../documentos/img/antes02.svg" width="100%">
<img src="../documentos/img/depois02.svg" width="100%">



## 2.5 Modelagem do Fluxo de Negócio

&emsp;Realizar a modelagem dos fluxos de negócio permite compreender como as atividades se organizam atualmente e como podem ser otimizadas com a solução proposta. Nesta seção, são apresentados o mapeamento da cadeia de valor, o fluxo atual (AS-IS) e o fluxo futuro desejado (TO-BE), evidenciando ganhos operacionais, pontos críticos e oportunidades de automação que sustentam o valor da proposta para o parceiro.

## 2.5.1 Cadeia de Valor

&nbsp;&nbsp; Esta seção detalha a cadeia de valor do aplicativo de verificação de perfil de investidor desenvolvido para o BTG Pactual. O objetivo é mapear e compreender as principais atividades envolvidas na geração de valor para o cliente, desde a entrada de dados até a entrega de uma análise precisa de conformidade regulatória. A estrutura apresentada considera os processos principais, de suporte e de gestão, bem como os indicadores de desempenho e o alinhamento com o ciclo de BPM (Business Process Management).

### Cadeia de Valor – App de Verificação de Perfil de Investidor (BTG Pactual)

&nbsp;&nbsp; O processo principal inicia com a importação dos dados de clientes e suas carteiras de ativos. Esses dados alimentam o classificador automático do aplicativo, que calcula o perfil do investidor com base na regulamentação da CVM (Resolução 175). A seguir, o sistema compara esse perfil calculado com o perfil cadastrado, apontando divergências e sugerindo readequações. Todo esse fluxo é apresentado de forma visual em um painel interativo, facilitando a análise e a tomada de decisão. O objetivo final é garantir a aderência entre o portfólio do cliente e seu perfil, promovendo transparência, governança e segurança regulatória.

---

#### Principais etapas do processo:
- Importação de dados via CSV, XLSX ou API  
- Classificação automática do perfil de investidor conforme a CVM 175  
- Verificação de conformidade entre o perfil cadastrado e o calculado  
- Geração de recomendações de ajuste por cliente  
- Exibição em painel visual com filtros por risco, categoria e status  

---

#### Processos de suporte:
- Infraestrutura de TI segura e escalável para garantir confidencialidade e desempenho  
- Interpretação jurídica das normas regulatórias para assegurar aderência normativa  
- Gestão dos dados com controle de acesso, backup e integridade  

---

#### Processos de gestão:
- Coordenação estratégica do projeto com definição de escopo, entregáveis e cronograma  
- Matriz RACI com papéis bem definidos entre stakeholders  
- Monitoramento da performance do sistema, análise de conformidade e coleta de feedback  

---

#### Indicadores de valor (KPI's sugeridos):
- Redução no número de descasamentos de perfil por cliente  
- Tempo médio de verificação de conformidade antes e depois do uso do app  
- Taxa de conformidade regulatória das carteiras monitoradas  
- Nível de confiança percebido pelo cliente (em pesquisas internas)  

---

#### Valor para o cliente/investidor:
- Mais segurança e transparência no alinhamento entre seu perfil e seus investimentos  
- Acesso facilitado às informações sobre adequação da carteira  
- Redução do risco de investir em produtos incompatíveis com seu perfil  
- Fortalecimento da relação com o BTG por meio de uma experiência digital confiável  

---

#### Relação com o ciclo de BPM:
- **Descoberta do processo**: mapeamento da jornada de verificação de perfil  
- **Análise**: identificação de falhas nos processos manuais ou desatualizados  
- **Redesenho**: automação com base nas regras da CVM  
- **Implementação**: desenvolvimento e testes do app com dados simulados  
- **Monitoramento**: uso de painéis com indicadores de conformidade por cliente  

---

#### Potencial de expansão e integração futura:
- Integração com sistemas internos do BTG, como CRM e plataformas de alocação  
- Criação de APIs para validação externa por entidades ou auditores  
- Escalabilidade para avaliação de outros produtos financeiros ou perfis avançados  

---

<br>
<div align="center">
<sub>Figura x - Cadeia de valor</sub>
<br>
<br>
<img src="../documentos/img/cadeiadevalor.png" alt='Imagem com a cadeia de valor' width="100%">
<br>
<br>
<sup>Fonte: Material produzido pelos autores (2025)</sup>
</div>
<br>

&nbsp;&nbsp; A análise da cadeia de valor demonstra como o aplicativo entrega benefícios tangíveis tanto para o cliente quanto para o BTG Pactual, promovendo conformidade regulatória, eficiência operacional e uma experiência digital confiável. Ao automatizar e visualizar o processo de verificação do perfil de investidor, a solução contribui para decisões mais seguras e alinhadas, além de abrir caminho para integrações e expansões futuras que aumentam ainda mais seu impacto estratégico.

## 2.5.2 Fluxo de Negócio Proposto (AS-IS e TO-BE)


A modelagem dos fluxos de negócio AS-IS e TO-BE, apresentada a seguir via BPMN, é fundamental para visualizar as limitações do processo atual e o valor agregado pela solução proposta.

[Link de acesso aos fluxos no Miro](https://miro.com/app/board/uXjVIGy2aYQ=/?share_link_id=918758245621)

**Fluxo AS-IS (Processo Atual):**
<div align="center">
<sup>Figura X - Fluxo de Negócios Atual</sup>
<img src="./img/fluxo_atual.jpg">
<sub>Fonte: Material produzido pelos autores (2025).</sub>
</div>
<br>
O fluxo atual de recomendação e adequação de investimentos depende significativamente da análise manual do assessor. Este processo apresenta desafios como:

* **Contexto Limitado:** O assessor muitas vezes opera com informações de perfil do investidor que podem não refletir completamente a adequação da carteira atual do cliente às normativas vigentes da CVM (como a Resolução 175).
* **Análise Manual Intensiva:** A verificação da adequação e a filtragem de produtos compatíveis são realizadas manualmente, consumindo tempo e tornando difícil garantir consistência e personalização em escala.
* **Risco de Desalinhamento:** Há um risco inerente de desalinhamento entre o perfil formal do cliente e sua carteira real, o que pode levar a problemas de conformidade regulatória e suitability.
<br>
---

**Fluxo TO-BE (Processo Proposto com a Solução):**
<div align="center">
<sup>Figura X - Fluxo de Negócios Atual</sup>
<img src="./img/fluxo_proposto.jpg">
<sub>Fonte: Material produzido pelos autores (2025).</sub>
</div>
<br>
A introdução do aplicativo móvel redesenha o fluxo de negócio para superar as limitações atuais. Os principais ganhos incluem:


* **Recomendações Contextualizadas:** A solução considera o histórico do cliente e utiliza filtros para sugerir ajustes ou novas recomendações de investimento que sejam verdadeiramente personalizadas e aderentes ao perfil de risco e objetivos do investidor.
* **Proatividade e Eficiência:** O assessor ganha eficiência com a automação da análise e pode atuar de forma mais proativa na gestão da carteira dos clientes, apoiado por informações e sugestões geradas pelo sistema.
* **Adequação com Interesse REAL do cliente:** Facilita no processo de retorno ao perfil de investidor do cliente. Caso um cliente tenha feitos investimentos fora do seu feitio de maneira pontual, o sistema entrega produtos que vão de encontro com seu perfil original, caso seja de interesse do cliente.


## 2.6 Matriz de Risco do Projeto

&emsp;Durante o planejamento inicial do projeto, foi realizada uma análise dos principais riscos que poderiam comprometer a entrega da solução, considerando tanto a probabilidade de ocorrência, quanto o impacto de cada situação sobre o andamento e a efetividade do aplicativo.

<div align="center">
<sub>Figura x - Matriz de riscos</sub><br>
<img src="img/matrizRiscos.png"><br>
<sup>Fonte: Material produzido pelos autores (2025) </sup>
</div>

&emsp;Entre os riscos com maior criticidade está a possibilidade de erros nas regras de lógica do sistema de recomendação, o que comprometeria diretamente a confiabilidade da ferramenta perante os assessores. Como possíveis ações para mitigar esse risco, planejam-se ser implementados testes automatizados e revisões constantes da lógica com apoio técnico especializado e dados simulados quando necessário.

&emsp;Outro ponto relevante é o risco de não alcançar todos os requisitos inicialmente definidos, dado o escopo amplo e o tempo restrito de desenvolvimento. Essa preocupação será enfrentada por meio de uma abordagem incremental, com foco nas funcionalidades prioritárias validadas junto ao parceiro. Ainda em termos de cronograma, o risco de atraso na entrega das funcionalidades principais foi mapeado como relevante, podendo ser mitigado por meio do mantimento da organização ágil do time durante as Sprints.

&emsp;No que diz respeito à experiência do usuário, foi identificada a possibilidade de uma interface pouco intuitiva dificultar o uso contínuo da ferramenta. Para reduzir esse risco, serão conduzidos testes de usabilidade e sessões de co-criação com stakeholders ao longo do desenvolvimento. Já o risco de baixa adesão por falta de tempo do assessor foi considerado de baixo impacto, mas ainda assim será endereçado com uma interface enxuta, rápida e com curva de aprendizado reduzida.

&emsp;Essa análise de riscos contribui para uma gestão de projeto mais iterativa, estratégica e preventiva, permitindo que as decisões técnicas e de design estejam alinhadas aos desafios reais da operação e às expectativas dos usuários finais.

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


&nbsp;&nbsp;&nbsp;&nbsp;O Canvas MVP é uma ferramenta visual utilizada para estruturar, de forma rápida e simples, os principais elementos de um MVP (Mínimo Produto Viável). O seu objetivo é alinhar a equipe quanto à proposta da solução, as funcionalidades prioritárias, os públicos-alvo atendidos, jornadas de usuário, expectativas e métricas que validarão as hipóteses do negócio.

&nbsp;&nbsp;&nbsp;&nbsp;Segue abaixo, o canvas MVP da aplicação web desenvolvido a partir das atividades realizadas durante o workshop com o parceiro.

<div align="center">
<sub>Figura x - Canvas MVP do projeto</sub><br>
<img src="img/canvas-mvp.jpg"><br>
<sup>Fonte: Material produzido pelos autores (2025) </sup>
</div>

## 2.9 Estimativa de Investimento

&emsp;O texto à seguir, apresenta uma estimativa de investimento para o desenvolvimento e manutenção, pelo período de 1 ano, de um aplicativo de suporte à análise de carteiras de investimento, destinado aos assessores do banco de investimento BTG Pactual.

&emsp;O foco da estimativa considera um perfil de custo padrão, utilizando informações atualizadas do mercado (abril de 2025), levando em conta salários médios da área de tecnologia, custos de software e preços de infraestrutura em nuvem.

&emsp;A seguir são detalhados os valores estimados para o desenvolvimento e manutenção do projeto durante 12 meses, considerando as categorias da Pessoas, Software e Infraestrutura.

### Pessoas
| Cargo                   | Salário Mensal (R$) | Quantidade | Custo em 12 meses (R$) |
|-------------------------|---------------------|------------|------------------------|
| Gestor do Projeto       | 10.000              | 1          | 120.000                |
| Desenvolvedor Backend   | 8.000               | 3          | 192.000                |
| Desenvolvedor Frontend  | 7.000               | 2          | 168.000                |
| UX/UI Designer          | 6.000               | 1          | 36.000                 |
| Product Owner (PO)      | 10.000              | 1          | 60.000                 |
| Analista de QA          | 6.500               | 1          | 39.000                 |
| **Total Pessoas**       |                     |            | **615.000**            |

### Software
| Item                               | Custo Mensal (R$) | Custo em 12 meses (R$) |
|------------------------------------|-------------------|------------------------|
| Licença Figma Professional (UX/UI) | 80                | 960                    |
| IDE JetBrains (ex: WebStorm)       | 40                | 480                    |
| API Externa (se necessário)        | 200               | 2.400                  |
| Plataforma GitHub Teams            | 20                | 240                    |
| **Total Software**                 |                   | **4.080**              |

### Infraestrutura
| Item                                               | Custo Mensal (R$) | Custo em 12 meses (R$) |
|----------------------------------------------------|-------------------|------------------------|
| AWS EC2 (2 Instâncias Médias com Auto Scaling)     | 1.200             | 14.400                 |
| AWS RDS (Banco de Dados Alta Disponibilidade)      | 800               | 9.600                  |
| AWS S3 (Storage com Redundância)                   | 200               | 2.400                  |
| Load Balancer + CloudFront (Distribuição global)   | 300               | 3.600                  |
| Certificado SSL + Domínio + DNS Gerenciado         | 100               | 1.200                  |
| Monitoramento e Alertas (Datadog, UptimeRobot Pro) | 300               | 3.600                  |
| Backup Automático e Logs Centralizados (CloudWatch)| 200               | 2.400                  |
| **Total Infraestrutura**                           |                   | **37.200**             |


### Custo Total Estimado
| Categoria        | Total (R$) |
|------------------|------------|
| Pessoas          | 615.000    |
| Software         | 4.080      |
| Infraestrutura   | 37.200     |
| **Total Geral**  | **656.280** |

&emsp;A estimativa de investimento apresentada acima foi construída com base em valores de mercado atualizados até o mês de abril de 2025, considerando a realidade brasileira para o setor de tecnologia e desenvolvimento de software. Os profissionais de UX/UI Designer, Product Owner e Analista de QA estão alocados em meio período no projeto, o que justifica o salário proporcional.

# 3. Requisitos do Projeto
&emsp;Esta seção tem como objetivo identificar, organizar e descrever os requisitos do sistema proposto, com base na análise do problema, nas necessidades do parceiro BTG Pactual e nas expectativas dos principais usuários do sistema (personas Fábio e Mariana).

&emsp;Os requisitos aqui definidos norteiam o desenvolvimento do aplicativo e garantem que o produto final atenda às funcionalidades esperadas, bem como aos atributos de qualidade exigidos em um contexto sensível, que envolve dados financeiros e adequação regulatória.

&emsp;O conjunto de requisitos foi dividido em:

&emsp;Requisitos Funcionais (RFs): descrevem as funcionalidades específicas que o sistema deve executar para atingir seus objetivos, representadas por user stories vinculadas às personas.

&emsp;Requisitos Não Funcionais (RNFs): especificam as restrições, qualidades e condições que o sistema deve atender, com base nos atributos do modelo ISO/IEC 25010, tais como desempenho, segurança, manutenibilidade e usabilidade.

&emsp;Além disso, apresenta-se uma correlação entre os RFs e os RNFs, visando garantir a rastreabilidade entre as funcionalidades e os critérios de qualidade, assegurando a confiabilidade e consistência ao produto.

## 3.1 Requisitos Funcionais (RFs)

&emsp;Os Requisitos Funcionais (RFs) representam as funcionalidades que o sistema deve obrigatoriamente oferecer para atender às necessidades dos usuários e alcançar os objetivos de negócio. Eles descrevem o comportamento esperado do sistema diante de diferentes interações e contextos de uso, definindo claramente "o que" o sistema deve fazer. Neste projeto, os RFs foram derivados das features propostas, refinados por meio das personas e descritos tanto tecnicamente quanto na forma de user stories.

<div align="center">
<sub>Tabela X - Requisitos funcionais</sub><br>

| RF#   | Descrição                                                                                                                                      | Prioridade |
|-------|------------------------------------------------------------------------------------------------------------------------------------------------|------------|
| RF01  | O sistema deve importar e processar automaticamente os dados de carteira de clientes por meio de integração via API.                                         | Alta       |
| RF02  | O sistema deve calcular automaticamente o perfil de risco com base nos ativos da carteira.                                                    | Alta       |
| RF03  | O sistema deve verificar a conformidade entre o perfil calculado e o perfil cadastrado do cliente.                                            | Alta       |
| RF04  | O sistema deve exibir recomendações de readequação do perfil quando for identificado um desalinhamento.                                       | Alta       |
| RF05  | O sistema deve apresentar, de forma visual, o status de conformidade de todas as carteiras em um painel.                                      | Alta       |
| RF06  | O sistema deve oferecer filtros por status de conformidade (adequado/não adequado).                                                           | Média      |
| RF07  | O sistema deve permitir filtro de clientes por segmento (Prata, Ouro, Platinum).                                                              | Média      |
| RF08  | O sistema deve manter o histórico das sugestões de readequação de perfil por cliente.                                                         | Média      |
| RF09  | O sistema deve exibir graficamente a composição da carteira por tipo de ativo.                                                                | Baixa      |

<sup>Material produzido pelos autores (2025)</sup>
</div><br>

### Requisitos Funcionais Definidos como User Stories

<div align="center">
<sub>Tabela X - User Stories dos Requisitos Não Funcionais</sub><br>

| ID    | User Story |
|-------|------------|
| RF01  | Como Fábio, quero importar automaticamente os dados das carteiras de meus clientes via API, para garantir rapidez e integração contínua com os sistemas internos. |
| RF02  | Como Fábio, quero que o sistema calcule o perfil do cliente com base na carteira, para identificar facilmente desalinhamentos. |
| RF03  | Como Fábio, quero verificar se a carteira está aderente ao perfil cadastrado, para evitar riscos regulatórios. |
| RF04  | Como Fábio, quero sugestões automáticas de readequação de perfil, para apresentar ao cliente opções seguras rapidamente. |
| RF05  | Como Fábio, quero visualizar todas as carteiras em um painel com seus status, para identificar rapidamente onde intervir. |
| RF06  | Como Fábio, quero filtrar apenas carteiras fora do perfil, para focar nas situações que exigem ação imediata. |
| RF07  | Como Fábio, quero filtrar meus clientes por segmento, para priorizar atendimentos estratégicos. |
| RF08  | Como Fábio, quero visualizar o histórico de readequações sugeridas, para acompanhar decisões anteriores e justificar recomendações. |
| RF09  | Como Mariana, quero ver a distribuição da minha carteira por tipo de ativo de forma clara, para entender rapidamente onde estou investindo. |

<sup>Material produzido pelos autores (2025)</sup>
</div><br>

### Tabela de Testes para Requisitos Funcionais

<div align="center">
<sub>Tabela X - Testes associados aos Requisitos Funcionais</sub><br>

| Caso de Teste | RF#   | Descrição do Teste                                                                                      | Pré-condição                                                                 | Caso de Aceite                                                                                   | Caso de Recusa                                                                                   |
|---------------|-------|---------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------|
| CT-RF01       | RF01  | Testar importação automática de uma carteira via integração de API.                                   | Arquivo válido em formato CSV/XLSX presente no sistema.                      | Dados são processados corretamente e exibidos no sistema.                                        | Erro ao processar arquivo ou ausência de resposta do sistema.                                   |
| CT-RF02       | RF02  | Testar cálculo do perfil com base na composição da carteira de um cliente.                              | Carteira do cliente carregada com ativos e pesos.                            | Sistema calcula e exibe o perfil de risco com base nos ativos.                                  | Perfil não calculado ou resultado inconsistente com os dados.                                   |
| CT-RF03       | RF03  | Verificar conformidade entre o perfil calculado e o perfil cadastrado.                                  | Perfil cadastrado e carteira disponíveis no sistema.                         | Sistema exibe mensagem indicando se o cliente está ou não conforme.                             | Sistema não exibe resultado ou mostra status incorreto.                                          |
| CT-RF04       | RF04  | Testar geração de recomendações para perfis não conformes.                                               | Cliente fora do perfil e perfil calculado já disponível.                     | Sistema exibe sugestão de readequação de perfil.                                                 | Nenhuma sugestão é apresentada ou mensagem incorreta.                                            |
| CT-RF05       | RF05  | Visualizar painel com status de conformidade das carteiras dos clientes.                                | Base de clientes carregada e analisada.                                      | Painel exibe lista de clientes e status visual de conformidade.                                 | Painel vazio, incompleto ou com dados incorretos.                                                |
| CT-RF06       | RF06  | Aplicar filtro para exibir apenas carteiras fora do perfil.                                             | Base de dados com clientes em diferentes estados de conformidade.            | Lista é filtrada corretamente para mostrar apenas os casos não conformes.                       | Filtro não funciona ou exibe resultados incorretos.                                              |
| CT-RF07       | RF07  | Aplicar filtro por segmento (Prata, Ouro, Platinum).                                                    | Clientes cadastrados com segmentação definida.                              | Sistema filtra e exibe clientes corretamente por segmento.                                       | Lista incorreta ou ausência de filtragem.                                                        |
| CT-RF08       | RF08  | Verificar exibição do histórico de sugestões de readequação por cliente.                                | Cliente com histórico de sugestões anteriores no sistema.                    | Sistema exibe linha do tempo ou registros anteriores com clareza.                               | Histórico ausente ou incompleto.                                                                 |
| CT-RF09       | RF09  | Testar visualização gráfica da composição da carteira por tipo de ativo.                                | Carteira importada com ativos e classificações definidas.                   | Gráfico (pizza, barras) é exibido corretamente com dados proporcionais.                         | Gráfico ausente, distorcido ou com dados incorretos.                                             |

<sup>Fonte: Material produzido pelos autores (2025)</sup>
</div>

## 3.2 Requisitos Não Funcionais (RNFs)

&emsp;Os Requisitos Não Funcionais (RNFs) especificam as qualidades e restrições que o sistema deve respeitar, como desempenho, segurança, usabilidade, manutenibilidade, entre outros. Diferentemente dos RFs, os RNFs não dizem respeito ao comportamento direto do sistema, mas à forma como esse comportamento deve ser entregue. Neste projeto, os RNFs foram definidos com base nas diretrizes da norma ISO/IEC 25010 e abordam critérios técnicos essenciais para garantir robustez, escalabilidade e boa experiência de uso.

<div align="center">
<sub>Tabela X - Requisitos não funcionais</sub><br>


| **RNF#** | **Descrição**                                                                                                                   | **Aspecto de Qualidade (ISO 25010)** | **Prioridade** |
|---------|-----------------------------------------------------------------------------------------------------------------------------------|--------------------------------------|----------------|
| RNF01   | O sistema deve responder às requisições de recomendação baseadas em machine learning em até 2 segundos, mesmo sob condições normais de operação. | Desempenho e Eficiência              | Média          |
| RNF02   | O sistema deve suportar pelo menos 3.000 acessos simultâneos, mantendo o tempo de resposta médio de requisições padrão (não-ML) abaixo de 1 segundo por requisição. | Desempenho e Eficiência              | Média          |
| RNF03   | O sistema deve ser containerizado com Docker e projetado para rodar exclusivamente em ambientes de nuvem, garantindo portabilidade e escalabilidade. | Portabilidade                        | Alta           |
| RNF04   | O app deve ser desenvolvido exclusivamente para Android (versão 10 ou superior).                                                 | Compatibilidade                      | Baixa          |
| RNF05   | O sistema deve usar APIs RESTful bem documentadas e versionadas através do Swagger, com autenticação via provedor de terceiros (ex: Auth0 ou Firebase), garantindo segurança e compatibilidade entre os componentes. | Compatibilidade                      | Média          |
| RNF06   | A estrutura do aplicativo deve ser organizada de forma modular, com responsabilidades claras e desacopladas entre os componentes, permitindo a evolução do código com baixo impacto em outras partes do sistema. | Manutenibilidade                     | Alta           |
| RNF07   | O sistema de recomendação (ML) deve rodar em infraestrutura separada do backend do app, sendo acessado exclusivamente via API. Essa separação deve permitir manutenção independente entre os dois sistemas. | Manutenibilidade                     | Alta           |
| RNF08   | A comunicação entre o app, o backend e o sistema de recomendação deve ser feita via HTTPS com TLS 1.2 ou superior.               | Segurança                            | Alta           |
| RNF09   | A autenticação de acesso ao app deve ser feita com uso de autenticação multifator (MFA).                                         | Segurança                            | Baixa          |
| RNF10   | Dados sensíveis eventualmente armazenados localmente devem estar criptografados utilizando criptografia simétrica forte.         | Segurança                            | Alta           |
| RNF11   | Em falha do sistema de recomendação, o app deve mostrar mensagem clara e oferecer ao menos 1 sugestão padrão, além de opção para tentar novamente. | Confiabilidade                       | Alta           |
| RNF12   | O sistema deve ter disponibilidade de no mínimo 99,5% mensal.                                                                    | Confiabilidade                       | Baixa          |
| RNF13   | O modelo de Machine Learning deve ser re-treinado automaticamente com base em indicadores matemáticos de variação dos dados e/ou de desempenho, de forma a garantir sua atualização eficiente.                    | Funcionalidade Adequada              | Baixa          |
| RNF14   | A exibição das recomendações deve seguir boas práticas de usabilidade (como as heurísticas de Nielsen) e ser validada com testes de usabilidade com usuários reais. | Usabilidade                          | Média          |

<sup>Material produzido pelos autores (2025)</sup>
</div><br>

### Requisitos Não Funcionais Definidos como User Stories

<div align="center">
<sub>Tabela X - User Stories dos Requisitos Não Funcionais</sub><br>

| **ID**  | **User Story** |
|---------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| RNF01   | Como Fábio, quero que as recomendações baseadas em machine learning sejam respondidas em até 2 segundos, para garantir uma experiência rápida e eficiente durante o uso cotidiano. |
| RNF02   | Como Fábio, quero que o sistema suporte pelo menos 3.000 acessos simultâneos, mantendo o tempo médio de resposta abaixo de 1 segundo, para garantir estabilidade e rapidez mesmo em horários de pico. |
| RNF03   | Como administrador do sistema, quero que a aplicação seja containerizada com Docker e executada exclusivamente em ambientes de nuvem, para garantir portabilidade e escalabilidade. |
| RNF04   | Como Fábio, quero que o app esteja disponível apenas para Android versão 10 ou superior, para garantir compatibilidade com dispositivos modernos. |
| RNF05   | Como desenvolvedor, quero que o sistema utilize APIs RESTful bem documentadas e versionadas via Swagger, com autenticação de terceiros, para facilitar a integração segura e manter a compatibilidade entre componentes. |
| RNF06   | Como desenvolvedor, quero que o aplicativo tenha uma estrutura modular e com responsabilidades bem definidas, para facilitar a manutenção e evolução do código com baixo impacto. |
| RNF07   | Como administrador do sistema, quero que o sistema de recomendação de ML rode em infraestrutura separada e seja acessado apenas via API, para permitir manutenção e atualizações independentes entre os sistemas. |
| RNF08   | Como Fábio, quero que toda comunicação entre app, backend e sistema de recomendação seja feita através de HTTPS com TLS 1.2 ou superior, para garantir a segurança dos dados transmitidos. |
| RNF09   | Como Fábio, quero autenticação multifator (MFA) no acesso ao app, para aumentar a segurança da minha conta. |
| RNF10   | Como Fábio, quero que dados sensíveis armazenados localmente sejam criptografados com uma criptografia simétrica forte, para proteger minhas informações mesmo em caso de perda ou roubo do dispositivo. |
| RNF11   | Como Fábio, quero que o app exiba uma mensagem clara e forneça uma sugestão padrão caso o sistema de recomendação falhe, para que eu possa continuar usando o app sem frustração. |
| RNF12   | Como Fábio, quero que o sistema esteja disponível no mínimo 99,5% do tempo mensal, para garantir que o serviço esteja quase sempre acessível quando eu precisar. |
| RNF13   | Como Fábio, quero que o modelo de ML seja re-treinado automaticamente baseado em variação de dados e desempenho, para manter a qualidade e atualidade das recomendações. |
| RNF14   | Como Fábio, quero que a exibição das recomendações siga boas práticas de usabilidade e seja validada com testes reais, para que a interface seja intuitiva e fácil de usar. |


<sup>Material produzido pelos autores (2025)</sup>
</div><br>


### Tabela de Testes para Requisitos Não Funcionais

<div align="center">
<sub>Tabela X - Testes associados aos Requisitos Não Funcionais</sub><br>

| **Caso de Teste**       | **RNF#** | **Descrição do Teste**                                                                                      | **Pré-condição**                                              | **Caso de Aceite**                                                                 | **Caso de Recusa**                                                                     |
|-------------------------|---------|-------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------|------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------|
| CT-RNF01                | RNF01   | Medir o tempo de resposta de uma recomendação via ML sob carga normal de uso.                              | Servidor ativo com modelo treinado e base de dados populada.  | Tempo de resposta ≤ 2 segundos.                                                   | Tempo de resposta > 2 segundos.                                                        |
| CT-RNF02                | RNF02   | Simular 3.000 acessos simultâneos e medir o tempo médio de resposta das requisições padrão (não-ML).          | Sistema em ambiente de teste com suporte a concorrência.      | Tempo médio de resposta < 1 segundo por requisição.                               | Tempo médio de resposta ≥ 1 segundo.                                                   |
| CT-RNF03                | RNF03   | Verificar se todos os serviços rodam em containers e estão hospedados em ambiente de nuvem.                 | Sistema implantado.                                            | Todos os containers ativos em ambiente cloud.                                     | Componentes rodando fora de containers ou fora da nuvem.                               |
| CT-RNF04                | RNF04   | Verificar se o app é compatível apenas com Android 10 ou superior.                                          | APK instalado em diferentes versões do Android.               | Funciona em Android 10+, falha em versões anteriores.                            | Funciona em Android < 10 ou não funciona em Android ≥ 10                                                            |
| CT-RNF05                | RNF05   | 	Verificar se as APIs REST estão documentadas utilizando Swagger padrão, versionadas e autenticadas por provedor externo (Auth0 ou Firebase).              | Ambiente de desenvolvimento com APIs expostas.                 | Documentação disponível via Swagger, uso de Auth0 ou Firebase ativo e versionamento das APIs.                         | Ausência de documentação via Swagger, falta de versionamento ou autenticação manual.                 |
| CT-RNF06                | RNF06   | 	Verificar a modularidade do sistema desativando um dos containers de serviço e validando se os demais componentes continuam operando normalmente.                          | Sistema containerizado em execução, com acesso de controle individual aos containers.                         | Componentes restantes continuam funcionando corretamente, evidenciando a separação de responsabilidades.                                       | Queda ou falha generalizada no sistema ao desativar um único componente, indicando alto acoplamento.                            |
| CT-RNF07                | RNF07   | 	Verificar se o sistema de recomendação (ML) está em infraestrutura separada, enviando uma requisição direta à sua API e validando a resposta sem passar pelo backend.                | Sistema de ML ativo e exposto via API.                                           | Requisição direta à API do sistema de ML respondendo corretamente, comprovando a separação entre backend e ML.          | Sistema de ML inacessível diretamente ou dependente do backend para funcionamento.                       |
| CT-RNF08                | RNF06   | Testar comunicação entre app e backend/sistema de ML com Wireshark e inspecionar se é feita via HTTPS com TLS ≥ 1.2.    | Servidores configurados com TLS.                              | Certificado válido e conexão segura.                                             | Conexão HTTP ou TLS inferior a 1.2.                                                     |
| CT-RNF09                | RNF09   | Testar o fluxo de autenticação com MFA ativado.                                                             | Usuário com MFA configurado.                                  | MFA solicitado e validado corretamente.                                          | Login sem solicitação de segundo fator ou falha de verificação.                        |
| CT-RNF10                | RNF10   | Simular armazenamento local de dados sensíveis e verificar criptografia.                                   | Dispositivo com dados armazenados.                            | Dados criptografados com algoritmo simétrico forte (ex: AES-256).               | Dados em texto plano ou criptografia fraca.                                            |
| CT-RNF11                | RNF11   | Simular falha no sistema de recomendação e observar comportamento do app.                                   | Sistema de ML offline.                                        | App exibe mensagem amigável e sugestão padrão, com botão "Tentar novamente".     | App trava ou exibe erro genérico.                                                      |
| CT-RNF12                | RNF12   | Verificar logs de uptime e incidentes no último mês.                                                        | Sistema em produção.                                           | Uptime ≥ 99,5% no período analisado.                                             | Uptime < 99,5%.                                                                        |
| CT-RNF13                | RNF13   | Verificar, por meio dos logs ou registros do sistema, se o re-treinamento do modelo de Machine Learning é disparado automaticamente com base em indicadores matemáticos de variação dos dados e/ou de desempenho.                             | Base de dados e sistema de monitoramento de indicadores ativos.                      | Re-treinamentos registrados após detecção de variação relevante nos indicadores, sem falhas no processo.                                          | Re-treinamento não é disparado quando necessário, é disparado sem necessidade, ou ocorre com falhas.                          |
| CT-RNF14                | RNF14   | Avaliar a interface de recomendações com base nas heurísticas de Nielsen e testes com pelo menos 5 usuários reais.       | Protótipo funcional e grupo de usuários disponíveis.            | Feedback positivo ≥ 80% em testes.                                               | Reclamações recorrentes ou reprovação nos testes.                                      |

<sup>Fonte: Material produzido pelos autores (2025)</sup>
</div>


## 3.3 Correlação RFs e RNFs

<div align="center">
<sub>Tabela X - Tabela de Correlação RFs e RNFs</sub><br>

| Requisito Funcional | RNFs Correlatos                  | Justificativa da Correlação |
|---------------------|----------------------------------|-----------------------------|
| RF01                | RNF03, RNF05, RNF08              | A importação e processamento de dados exigem APIs bem estruturadas, ambiente cloud escalável e comunicação segura. |
| RF02                | RNF01, RNF07, RNF13              | O cálculo do perfil deve ser rápido (desempenho), separado do backend (modularidade) e atualizado com re-treinamento automático. |
| RF03                | RNF01, RNF02                     | A verificação de conformidade exige desempenho sob carga e resposta rápida mesmo com múltiplos acessos. |
| RF04                | RNF11, RNF13                     | A recomendação deve lidar com falhas de sistema (resiliência) e refletir atualização contínua do modelo de ML. |
| RF05                | RNF06, RNF14                     | A exibição no painel depende de boa estrutura modular e interface intuitiva e validada com usuários. |
| RF06                | RNF14                            | Os filtros por conformidade devem seguir boas práticas de usabilidade para facilitar a navegação. |
| RF07                | RNF06                            | O filtro por segmento depende de arquitetura modular que permita fácil evolução do sistema. |
| RF08                | RNF10, RNF12                     | O histórico de sugestões envolve armazenamento seguro de dados sensíveis e alta disponibilidade do sistema. |
| RF09                | RNF14                            | A visualização gráfica deve seguir heurísticas de usabilidade para facilitar a interpretação da carteira por usuários não técnicos. |

<sup>Fonte: Material produzido pelos autores (2025)</sup>
</div>

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

## 5.4 Tecnologias e Linguagens

### 5.4.1 Back-end

&emsp;Para o back-end da aplicação, será utilizada a linguagem de programação **C#**, em conjunto com o framework **.NET**. Essa escolha se deve à robustez, segurança e excelente desempenho do .NET, além de sua forte integração com bibliotecas de machine learning através do **ML.NET**, permitindo a incorporação de modelos treinados diretamente no ambiente do servidor.

### 5.4.2 Machine Learning e Tratamento de Dados

&emsp;Para o tratamento de dados e preparação de modelos de aprendizado de máquina, será utilizada a linguagem **Python**. Python é amplamente adotada na área de ciência de dados devido à sua simplicidade, vasta gama de bibliotecas (como Pandas, NumPy, e Scikit-learn) e grande comunidade. O processo de retreinamento dos modelos será gerenciado com o auxílio do **MLflow**, que permite o controle de versões, monitoramento de experimentos e gerenciamento de modelos de forma eficiente.

### 5.4.3 Front-end

&emsp;O front-end da aplicação será desenvolvido com **React Native**, uma tecnologia que permite a criação de aplicações móveis multiplataforma com desempenho próximo ao nativo. Apesar de o projeto estar sendo desenvolvido inicialmente para Android, a escolha pelo React Native se dá justamente por sua natureza multiplataforma, o que facilitará a adaptação futura para outros sistemas operacionais, como o iOS, além de proporcionar uma interface moderna, responsiva e de fácil manutenção.

### 5.4.4 Banco de Dados

&emsp;O sistema utilizará o **PostgreSQL** como sistema gerenciador de banco de dados relacional. O PostgreSQL foi escolhido por ser uma solução open-source poderosa, altamente escalável, que oferece suporte a consultas complexas, integridade referencial e extensões úteis para aplicações que trabalham com dados estruturados de forma intensiva.

### 5.4.5 Autenticação

&emsp;Para o gerenciamento da autenticação de usuários, será utilizado o Firebase Authentication. O Firebase oferece uma solução segura, escalável e de fácil integração com aplicações móveis, reduzindo significativamente o esforço de desenvolvimento e aumentando a segurança da aplicação. Além disso, a plataforma disponibiliza uma cota gratuita com capacidade de armazenamento razoavelmente grande, o que contribui para tornar o desenvolvimento menos custoso, especialmente nas fases iniciais do projeto.
<br><br>



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

