### Database context
This library is based on EFCore as an ORM.
The context defines all tables as properties in its class.
However to generate the proper SQL tables and changes to the SQL structure, it requires migrations.

### Migrations
Migrations are code based updates to the structure and signature of a database.
Its code needs to be generated at design time when changes to the structure and signature of the code are made.
To generate these changes a Database that holds the current structure and signature is created from the previous migrations
after which a diff is generated compared to the current signature and structure of the code.

#### Generating new migrations
 1) Startup the postgres database using the supplied docker-compose file:
    `docker-compose up -d`
 2) 
