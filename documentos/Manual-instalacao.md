# Manual de Instalação - DIANA

<Table>
  <tr>
    <td><a href= "https://www.btgpactual.com/"><img src="img/logo-btg.png" alt="BTG Pactual" border="0"></td>
    <td>
      <a href= "https://www.inteli.edu.br/"><img src="img/logo-Inteli.png" alt="Inteli - Instituto de Tecnologia e Liderança" border="0"></a>
    </td>
  </tr>
</table>


# Nome do Projeto: Diana.

## Nome do Grupo: Watermelon

## Integrantes:

- <a href="https://www.linkedin.com/in/anacdejesus/">Ana Carolina de Jesus Pacheco da Silva</a>
- <a href="https://www.linkedin.com/in/felipe-zillo-72b367247/">Felipe Zillo</a>
- <a href="https://www.linkedin.com/in/gabriel-nascimento-563382243/">Gabriel Nascimento</a>
- <a href="https://www.linkedin.com/in/joao-souza-campos/">João Victor de Souza Campos</a>
- <a href="https://www.linkedin.com/in/lucas-tort/">Lucas Cozzolino Tort</a>
- <a href="https://www.linkedin.com/in/natalycunha/">Nataly Cunha</a>
- <a href="https://www.linkedin.com/in/thiagovolcati/">Thiago Volcati</a>
- <a href="https://www.linkedin.com/in/vitor-balbo/">Vitor Balbo</a>

# Sumário

- [1. Introdução](#1-introdução)
- [2. Pré-requisitos](#2-pré-requisitos)
- [3. Estrutura de Diretórios](#3-estrutura-de-diretórios)
- [4. Clonando o Repositório](#4-clonando-o-repositório)
- [5. Configurando o Backend](#5-configurando-o-backend)
- [6. Configurando o Frontend](#6-configurando-o-frontend)
- [7. Configurando a API de Recomendação](#7-configurando-a-api-de-recomendação)
- [8. Configurando o Banco de Dados](#8-configurando-o-banco-de-dados)
- [9. Executando Testes](#9-executando-testes)

# 1. Introdução

## 1.1 Objetivo do Documento

Este documento fornece instruções detalhadas para a instalação e configuração do sistema de recomendação de investimentos desenvolvido para o BTG Pactual. O manual destina-se a desenvolvedores e administradores de sistema que precisam configurar o ambiente de desenvolvimento ou implantar a aplicação em produção. Aqui você encontrará os requisitos do sistema, etapas de instalação e configuração necessárias para executar todos os componentes da aplicação.

---

## 2. Pré-requisitos

Antes de iniciar a instalação, certifique-se de que os seguintes softwares estão instalados:

- [Node.js (versão LTS recomendada)](https://nodejs.org/)
- [.NET SDK 7.0 ou superior](https://dotnet.microsoft.com/download)
- [Python 3.8 ou superior](https://www.python.org/downloads/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/downloads)
- Gerenciador de pacotes: `npm` (incluído com Node.js)

> 🔍 _Verifique suas versões executando no terminal:_
>
> ```bash
> node -v
> dotnet --version
> python --version
> docker --version
> git --version
> npm -v
> ```

---

## 3. Estrutura de Diretórios

A estrutura do projeto está organizada da seguinte forma:

```
2025-1B-T13-ES06-G03/
├── src/
│   ├── backend/           # API em .NET Core
│   ├── frontend/          # Interface em React Native
│   ├── recomendationAPI/  # API de Recomendação em Python
│   ├── database/          # Scripts SQL e configurações
│   └── auditoria/         # Sistema de Auditoria
├── documentos/            # Documentação do projeto
└── models/               # Modelos e notebooks de ML
```

---

## 4. Clonando o Repositório

Para obter o código-fonte do projeto em sua máquina local, execute:

```bash
git clone https://github.com/Inteli-College/2025-1B-T13-ES06-G03
cd 2025-1B-T13-ES06-G03
```

---

## 5. Configurando o Backend

### 5.1 Instalando dependências

O backend é desenvolvido em .NET Core. Para configurar:

```bash
cd src/backend
dotnet restore
```

### 5.2 Configurando variáveis de ambiente

Crie um arquivo `appsettings.Development.json` baseado no exemplo fornecido:

```bash
cp appsettings.json appsettings.Development.json
```

### 5.3 Executando o servidor

Para iniciar o servidor de desenvolvimento:

```bash
dotnet run
```

> 🔎 _O servidor estará disponível em `https://localhost:7071` por padrão_

---

## 6. Configurando o Frontend

### 6.1 Instalando dependências

O frontend é desenvolvido em React Native com Expo. Para configurar:

```bash
cd src/frontend
npm install
```

### 6.2 Rodando a aplicação

Para iniciar o servidor de desenvolvimento:

```bash
npx expo start
```

> 🌐 _Siga as instruções no terminal para abrir o app no emulador ou dispositivo físico_

---

## 7. Configurando a API de Recomendação

### 7.1 Instalando dependências Python

```bash
cd src/recomendationAPI
pip install -r requirements.txt
```

### 7.2 Executando a API

```bash
python src/main.py
```

> 🔎 _A API estará disponível em `http://localhost:8000`_

---

## 8. Configurando o Banco de Dados

### 8.1 Utilizando Docker Compose

O projeto utiliza Docker Compose para gerenciar os serviços de banco de dados. Execute:

```bash
cd src
docker-compose up -d
```

Este comando iniciará:

- PostgreSQL (Banco de dados principal)
- MongoDB (Sistema de auditoria)

### 8.2 Inicializando o banco de dados

Execute os scripts SQL iniciais:

```bash
cd src/database
# Os scripts serão executados automaticamente na inicialização do container
```

---

## 9. Executando Testes

### Backend (.NET)

```bash
cd src/backend
dotnet test
```

### Frontend (React Native)

```bash
cd src/frontend
npm test
```

### API de Recomendação (Python)

```bash
cd src/recomendationAPI
python -m pytest
```

---

## 📋 Licença/License

<p xmlns:cc="http://creativecommons.org/ns#" xmlns:dct="http://purl.org/dc/terms/"><a property="dct:title" rel="cc:attributionURL" href="https://github.com/Inteli-College/2025-1B-T13-ES06-G03">DIANA</a> by <a rel="cc:attributionURL dct:creator" property="cc:attributionName" href="https://github.com/Inteli-College/2025-1B-T13-ES06-G03">INTELI, <a href="https://www.linkedin.com/in/anacdejesus/">Ana Carolina Pacheco</a>, <a href="https://www.linkedin.com/in/felipe-zillo-72b367247/">Felipe Zillo</a>, <a href="https://www.linkedin.com/in/gabriel-nascimento-563382243/">Gabriel Nascimento</a>, <a href="https://www.linkedin.com/in/joao-souza-campos/">João Victor de Souza Campos</a>, <a href="https://www.linkedin.com/in/lucas-tort/">Lucas Cozzolino Tort</a>, <a href="https://www.linkedin.com/in/natalycunha/">Nataly Cunha</a>, <a href="https://www.linkedin.com/in/thiagovolcati/">Thiago Volcati</a>, <a href="https://www.linkedin.com/in/vitor-balbo/">Vitor Balbo</a> is licensed under <a href="http://creativecommons.org/licenses/by/4.0/?ref=chooser-v1" target="_blank" rel="license noopener noreferrer" style="display:inline-block;">Attribution 4.0 International<img style="height:22px!important;margin-left:3px;vertical-align:text-bottom;" src="https://mirrors.creativecommons.org/presskit/icons/cc.svg?ref=chooser-v1"><img style="height:22px!important;margin-left:3px;vertical-align:text-bottom;" src="https://mirrors.creativecommons.org/presskit/icons/by.svg?ref=chooser-v1"></a></p>
