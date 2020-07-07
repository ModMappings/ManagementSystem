#!/bin/sh
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE USER mmms with password 'mmms';
    ALTER USER mmms with SUPERUSER;
    CREATE DATABASE mmms;
    GRANT ALL PRIVILEGES ON DATABASE mmms TO mmms;
EOSQL