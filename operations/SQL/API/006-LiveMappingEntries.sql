create table "LiveMappingEntries"
(
	"Id" uuid not null
		constraint "PK_LiveMappingEntries"
			primary key,
	"VersionedComponentId" uuid not null
		constraint "FK_LiveMappingEntries_VersionedComponents_VersionedComponentId"
			references "VersionedComponents"
				on delete cascade,
	"CreatedOn" timestamp not null,
	"InputMapping" text not null,
	"OutputMapping" text not null,
	"Documentation" text,
	"Distribution" integer not null,
	"MappingTypeId" uuid not null
		constraint "FK_LiveMappingEntries_MappingTypes_MappingTypeId"
			references "MappingTypes"
				on delete cascade,
	"CreatedBy" uuid default '00000000-0000-0000-0000-000000000000'::uuid not null
);

alter table "LiveMappingEntries" owner to "mcp-migrations";

create index "IX_LiveMappingEntries_MappingTypeId"
	on "LiveMappingEntries" ("MappingTypeId");

create index "IX_LiveMappingEntries_VersionedComponentId"
	on "LiveMappingEntries" ("VersionedComponentId");

