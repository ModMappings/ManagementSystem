create table "Releases"
(
	"Id" uuid not null
		constraint "PK_Releases"
			primary key,
	"Name" text not null,
	"CreatedOn" timestamp not null,
	"CreatedBy" uuid not null,
	"GameVersionId" uuid not null
		constraint "FK_Releases_GameVersions_GameVersionId"
			references "GameVersions"
				on delete cascade,
	"MappingTypeId" uuid not null
		constraint "FK_Releases_MappingTypes_MappingTypeId"
			references "MappingTypes"
				on delete cascade,
	"IsSnapshot" boolean not null
);

alter table "Releases" owner to "mcp-migrations";

create index "IX_Releases_GameVersionId"
	on "Releases" ("GameVersionId");

create index "IX_Releases_MappingTypeId"
	on "Releases" ("MappingTypeId");

create unique index "IX_Releases_Name"
	on "Releases" ("Name");

