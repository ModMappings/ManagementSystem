create table "LockingEntries"
(
	"Id" uuid not null
		constraint "PK_LockingEntries"
			primary key,
	"VersionedComponentId" uuid not null
		constraint "FK_LockingEntries_VersionedComponents_VersionedComponentId"
			references "VersionedComponents"
				on delete cascade,
	"MappingTypeId" uuid not null
		constraint "FK_LockingEntries_MappingTypes_MappingTypeId"
			references "MappingTypes"
				on delete cascade
);

alter table "LockingEntries" owner to "mcp-migrations";

create index "IX_LockingEntries_MappingTypeId"
	on "LockingEntries" ("MappingTypeId");

create index "IX_LockingEntries_VersionedComponentId"
	on "LockingEntries" ("VersionedComponentId");

