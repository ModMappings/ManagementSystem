### Development version of Keycloak
This directory contains a development version of keycloak (our secure token service).
This service is to be used during development and allows for the validation of the access rights in the business layer.
#### Requirements
To use this version you will need: 
* Docker
* Docker-compose
* Port 8081 free
#### Starting keycloak.
1. Simply run `docker-compose up -d` in the shell of your choice.

Now you have an instance of keycloak running under `http://localhost:8081/`
#### Updating the data contained in the init script:
This keycloak instance comes with the correct settings for using it during development baked in.
These settings and one test user are created in the database during the start of the system using the
`init.sql` script that is located in this directory.

If you need to update this script (because you added or changed information inside keycloak) run the following commands
with your keycloak instance running:
##### Linux/MacOs (Shell)
1. `rm init.sql`
2. `docker exec -t keycloak_postgres_1 pg_dumpall -c -U keycloak > init.sql`
##### Windows (CMD)
1. `del init.sql`
2. `docker exec -t keycloak_postgres_1 pg_dumpall -c -U keycloak > init.sql`
##### Windows/Linux/MacOS (PowerShell)
1. `Remove-Item init.sql`
2. `docker exec -t keycloak_postgres_1 pg_dumpall -c -U keycloak > init.sql`