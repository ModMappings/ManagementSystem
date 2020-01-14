create table "release"
(
	"id" uuid not null
		constraint "PK_release"
			primary key,
	"name" text not null,
	"created_on" timestamp not null,
	"created_by" uuid not null,
	"game_version_id" uuid not null
		constraint "FK_release_game_version_gameVersionId"
			references "game_version"
				on delete cascade,
	"mapping_type_id" uuid not null
		constraint "FK_release_mapping_type_mappingTypeId"
			references "mapping_type"
				on delete cascade,
	"is_snapshot" boolean not null,

	unique ("name", "mapping_type_id")
);

create index "IX_release_gameVersionId"
	on "release" ("game_version_id");

create index "IX_release_mappingTypeId"
	on "release" ("mapping_type_id");

create index "IX_release_name"
	on "release" ("name");

