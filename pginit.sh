#!/bin/sh

# Cleanup and init of postgres

docker stop mmms_postgres_1
docker volume rm -f mmms_postgresdb

tmp=$(docker run -d --rm -v mmms_postgresdb:/var/lib/postgresql/data -v ${pwd}/dbinitscripts/:/docker-entrypoint-initdb.d/ -e POSTGRES_PASSWD=postgres postgres:12)
docker stop $tmp
