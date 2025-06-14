FROM mongo:7.0

# Definir variáveis de ambiente
ENV MONGO_INITDB_ROOT_USERNAME=admin
ENV MONGO_INITDB_ROOT_PASSWORD=btg_audit_2025
ENV MONGO_INITDB_DATABASE=btg_auditoria

# Criar diretório para scripts de inicialização
RUN mkdir -p /docker-entrypoint-initdb.d

# Copiar scripts de inicialização
COPY init-mongo.js /docker-entrypoint-initdb.d/
COPY init-collections.js /docker-entrypoint-initdb.d/

# Expor a porta padrão do MongoDB
EXPOSE 27017

# Configurar volume para persistência
VOLUME ["/data/db"]

# O comando padrão do MongoDB será executado automaticamente
