create table "VersionedComponents"
(
	"Id" uuid not null
		constraint "PK_VersionedComponents"
			primary key,
	"GameVersionId" uuid not null
		constraint "FK_VersionedComponents_GameVersions_GameVersionId"
			references "GameVersions"
				on delete cascade,
	"CreatedBy" uuid not null,
	"CreatedOn" timestamp not null,
	"ComponentId" uuid not null
		constraint "FK_VersionedComponents_Components_ComponentId"
			references "Components"
				on delete cascade
);

alter table "VersionedComponents" owner to "mcp-migrations";

create index "IX_VersionedComponents_ComponentId"
	on "VersionedComponents" ("ComponentId");

create index "IX_VersionedComponents_GameVersionId"
	on "VersionedComponents" ("GameVersionId");

