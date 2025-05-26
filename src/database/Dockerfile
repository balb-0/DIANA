FROM postgres:16

ENV POSTGRES_DB=btg_db \
    POSTGRES_USER=btg_user \
    POSTGRES_PASSWORD=btg_pass

COPY 01_create_database.sql /docker-entrypoint-initdb.d/
COPY 02_initial_data.sql /docker-entrypoint-initdb.d/

EXPOSE 5432
