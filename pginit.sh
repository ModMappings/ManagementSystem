#!/bin/sh

# Cleanup and init of postgres

docker stop mmms_postgres_1
docker rm mmms_postgres_1
docker volume rm -f mmms_postgresdb

tmp=$(docker run -d -v mmms_postgresdb:/var/lib/postgresql/data -v ${PWD}/dbinitscripts/:/docker-entrypoint-initdb.d/ -e POSTGRES_PASSWORD=postgres postgres:12)
sleep 20
docker stop $tmp
docker logs $tmp
docker rm $tmp
