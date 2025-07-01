<Table>
  <tr>
   <td><a href= "https://www.btgpactual.com/"><img src="documentos/img/logo-btg.png" alt="Centro Paula Souza" border="0"></td>
       <td>
      <a href= "https://www.inteli.edu.br/"><img src="documentos/img/logo-Inteli.png" alt="Inteli - Instituto de Tecnologia e Liderança" border="0"></a>
    </td>
  </tr>
</table>

# Nome do Projeto: DIANA

## Nome do Grupo: Watermelon

## Integrantes:

- <a href="https://www.linkedin.com/in/anacdejesus/">Ana Carolina Pacheco</a>
- <a href="https://www.linkedin.com/in/felipe-zillo-72b367247/">Felipe Zillo</a>
- <a href="https://www.linkedin.com/in/gabriel-nascimento-563382243/">Gabriel Nascimento</a>
- <a href="https://www.linkedin.com/in/joao-souza-campos/">João Victor de Souza Campos</a>
- <a href="https://www.linkedin.com/in/lucas-tort/">Lucas Cozzolino Tort</a>
- <a href="https://www.linkedin.com/in/natalycunha/">Nataly Cunha</a>
- <a href="https://www.linkedin.com/in/thiagovolcati/">Thiago Volcati</a>
- <a href="https://www.linkedin.com/in/vitor-balbo/">Vitor Balbo</a>

## Professores:

### Orientador(a)

- <a href="https://www.linkedin.com/in/vanunes/">Vanessa Nunes</a>

### Instrutores

- <a href="https://www.linkedin.com/in/professor1/">Computação - Jefferson de Oliveira</a>
- <a href="https://www.linkedin.com/in/professor1/">Matemática e Física - Diogo Morais</a>
- <a href="https://www.linkedin.com/in/professor1/">Negócios - Lisane Valdo</a>
- <a href="https://www.linkedin.com/in/professor1/">Design - Guilherme Cestari</a>
- <a href="https://www.linkedin.com/in/professor1/">Liderança - Filipe Gonçalves</a>

## 📝 Descrição

Este projeto é uma aplicação mobile desenvolvida para assessores de investimento do BTG Pactual. A solução tem como objetivo principal **analisar carteiras de clientes** e **verificar a conformidade entre o perfil de risco declarado e o perfil calculado**, com base nos ativos investidos.

O app permite que os assessores acompanhem de forma rápida e intuitiva quais clientes estão com carteiras adequadas ao seu perfil (conforme) e quais apresentam inconsistências (divergente), ajudando na **redução de riscos regulatórios** e na **melhoria da governança de suitability**.

Além da funcionalidade de análise, o projeto também está integrado a um CRM estratégico, que apoia o time comercial na **expansão da base de assessores**, utilizando uma abordagem **outbound omnichannel** (ligação ativa, email, reuniões e onboarding).

A interface é desenvolvida com foco em **usabilidade, agilidade e clareza**, visando atender um público com rotina intensa e necessidade de decisões rápidas.

## 📝 LINKS

<a href="/documentos/manual-instalacao.md">Link para o Manual de Instruções</a> que explica como montar, conectar e utilizar o protótipo.

<a href="/documentos/index.md">Link para a documentação geral</a> do projeto.

## 📁 Estrutura de pastas

```
📁 documentos/
   ├── img/                              # Imagens utilizadas na documentação
   ├── GestaoConfiguracao.md            # Documentação de gestão de configuração
   ├── GestaoProjeto.md                 # Documentação de gestão do projeto
   ├── GuiaPadroesDesenvolvimento.md    # Guia de padrões de desenvolvimento
   ├── Index.md                         # Índice da documentação
   ├── Manual-instalacao.md             # Manual de instalação do projeto
   └── Projeto.md                       # Documentação principal do projeto

📁 src/
   ├── auditoria/                       # Sistema de auditoria com MongoDB
   ├── backend/                         # Backend em .NET Core
   │   ├── Controllers/                 # Controllers da API
   │   ├── Models/                      # Modelos de dados
   │   ├── Services/                    # Serviços de negócio
   │   └── Tests/                       # Testes unitários e de integração
   ├── database/                        # Scripts e configurações do banco de dados
   │   └── performance/                 # Scripts de teste de performance
   ├── frontend/                        # Frontend em React Native
   │   ├── app/                         # Código fonte do frontend
   │   ├── components/                  # Componentes reutilizáveis
   │   └── tests/                       # Testes do frontend
   ├── models/                          # Modelos de machine learning
   │   ├── notebooks/                   # Jupyter notebooks de análise
   │   └── data/                        # Datasets e dados processados
   └── recomendationAPI/                # API de recomendação em FastAPI

📄 readme.md                            # Este arquivo
```

### 📑 Descrição dos Diretórios Principais

#### 📁 `documentos/`

Documentação completa do projeto, incluindo:

- Manuais técnicos e guias de desenvolvimento
- Documentação de gestão e configuração
- Documentação do projeto e seus processos

#### 📁 `src/`

Código-fonte do projeto, dividido em módulos:

- `backend/`: API REST em .NET Core com controllers, models e testes
- `frontend/`: Aplicativo mobile em React Native com componentes reutilizáveis
- `recomendationAPI/`: Serviço de recomendação em FastAPI com modelos ML
- `database/`: Scripts SQL e testes de performance
- `auditoria/`: Sistema de logging e auditoria em MongoDB
- `models/`: Notebooks de análise e datasets processados

#### 📄 `readme.md`

Este arquivo, contendo:

- Visão geral do projeto
- Instruções de instalação
- Estrutura do repositório
- Links para documentação detalhada

## 💻 Configuração para desenvolvimento

Para configurar o desenvolvimento da aplicação, [instale o git](https://git-scm.com/downloads) e clone esse repositório em seu computador através do comando:

```
git clone https://github.com/Inteli-College/2025-1B-T13-ES06-G03
```

Em seguida siga as instruções presentes em nosso <a href="/documentos/manual-instalacao.md">manual de instalação</a>.

## 🗃 Histórico de lançamentos

- 0.1.0 - 30/04/2024
  - Artefato
  - Artefato
- 0.2.0 - 16/05/2024
  - Artefato
  - Artefato
- 0.3.0 - 30/05/2024
  - Artefato
  - Artefato
- 0.4.0 - 13/06/2024
  - Artefato
  - Artefato
- 0.5.0 - 26/06/2024
  - Artefato
  - Artefato

## 📋 Licença/License

<p xmlns:cc="http://creativecommons.org/ns#" xmlns:dct="http://purl.org/dc/terms/"><a property="dct:title" rel="cc:attributionURL" href="https://github.com/ENDEREÇOGRUPO>"><nome do projeto></a> by <a rel="cc:attributionURL dct:creator" property="cc:attributionName" href="https://github.com/ENDEREÇOGRUPO">INTELI, <a href="https://www.linkedin.com/in/anacdejesus/">Ana Carolina Pacheco</a>, <a href="https://www.linkedin.com/in/felipe-zillo-72b367247/">Felipe Zillo</a>, <a href="https://www.linkedin.com/in/gabriel-nascimento-563382243/">Gabriel Nascimento</a>, <a href="https://www.linkedin.com/in/joao-souza-campos/">João Victor de Souza Campos</a>, <a href="https://www.linkedin.com/in/lucas-tort/">Lucas Cozzolino Tort</a>, <a href="https://www.linkedin.com/in/natalycunha/">Nataly Cunha</a>, <a href="https://www.linkedin.com/in/thiagovolcati/">Thiago Volcati</a>, <a href="https://www.linkedin.com/in/vitor-balbo/">Vitor Balbo</a> is licensed under <a href="http://creativecommons.org/licenses/by/4.0/?ref=chooser-v1" target="_blank" rel="license noopener noreferrer" style="display:inline-block;">Attribution 4.0 International<img style="height:22px!important;margin-left:3px;vertical-align:text-bottom;" src="https://mirrors.creativecommons.org/presskit/icons/cc.svg?ref=chooser-v1"><img style="height:22px!important;margin-left:3px;vertical-align:text-bottom;" src="https://mirrors.creativecommons.org/presskit/icons/by.svg?ref=chooser-v1"></a></p>
