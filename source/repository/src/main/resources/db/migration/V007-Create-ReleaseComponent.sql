create table release_component
(
	"id" uuid not null
		constraint "PK_release_component"
			primary key,
	"release_id" uuid not null
		constraint "FK_release_component_release_releaseId"
			references "release"
				on delete cascade,
	"mapping_id" uuid not null
		constraint "FK_release_component_mapping_mappingId"
			references "mapping"
				on delete cascade,

    unique ("release_id", "mapping_id")
);

create index "IX_release_component_mappingId"
	on release_component ("mapping_id");

create index "IX_release_component_releaseId"
	on release_component ("release_id");

