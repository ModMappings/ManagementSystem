create table "Components"
(
	"Id" uuid not null
		constraint "PK_Components"
			primary key,
	"Type" integer not null,
	"CreatedBy" uuid default '00000000-0000-0000-0000-000000000000'::uuid not null,
	"CreatedOn" timestamp default '0001-01-01 00:00:00'::timestamp without time zone not null
);

alter table "Components" owner to "mcp-migrations";

