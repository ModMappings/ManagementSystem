create table "MappingTypes"
(
	"Id" uuid not null
		constraint "PK_MappingTypes"
			primary key,
	"Name" text not null,
	"CreatedBy" uuid not null,
	"CreatedOn" timestamp not null
);

alter table "MappingTypes" owner to "mcp-migrations";

