create table "ReleaseComponents"
(
	"Id" uuid not null
		constraint "PK_ReleaseComponents"
			primary key,
	"ReleaseId" uuid not null
		constraint "FK_ReleaseComponents_Releases_ReleaseId"
			references "Releases"
				on delete cascade,
	"MappingId" uuid not null
		constraint "FK_ReleaseComponents_LiveMappingEntries_MappingId"
			references "LiveMappingEntries"
				on delete cascade
);

alter table "ReleaseComponents" owner to "mcp-migrations";

create index "IX_ReleaseComponents_MappingId"
	on "ReleaseComponents" ("MappingId");

create index "IX_ReleaseComponents_ReleaseId"
	on "ReleaseComponents" ("ReleaseId");

