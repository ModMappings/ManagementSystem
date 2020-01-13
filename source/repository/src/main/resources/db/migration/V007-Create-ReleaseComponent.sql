create table release_component
(
	"id" uuid not null
		constraint "PK_release_component"
			primary key,
	"releaseId" uuid not null
		constraint "FK_release_component_release_releaseId"
			references "release"
				on delete cascade,
	"mappingId" uuid not null
		constraint "FK_release_component_mapping_mappingId"
			references "mapping"
				on delete cascade,

    unique ("releaseId", "mappingId")
);

create index "IX_release_component_mappingId"
	on release_component ("mappingId");

create index "IX_release_component_releaseId"
	on release_component ("releaseId");

