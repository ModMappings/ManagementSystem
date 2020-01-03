create table "GameVersions"
(
	"Id" uuid not null
		constraint "PK_GameVersions"
			primary key,
	"Name" text not null,
	"CreatedOn" timestamp not null,
	"CreatedBy" uuid not null,
	"IsPreRelease" boolean not null,
	"IsSnapshot" boolean not null
);

alter table "GameVersions" owner to "mcp-migrations";

create unique index "IX_GameVersions_Name"
	on "GameVersions" ("Name");

