create table "release"
(
	"id" uuid not null
		constraint "PK_release"
			primary key,
	"name" text not null,
	"createdOn" timestamp not null,
	"createdBy" uuid not null,
	"gameVersionId" uuid not null
		constraint "FK_release_game_version_gameVersionId"
			references "game_version"
				on delete cascade,
	"mappingTypeId" uuid not null
		constraint "FK_release_mapping_type_mappingTypeId"
			references "mapping_type"
				on delete cascade,
	"isSnapshot" boolean not null,

	unique ("name", "mappingTypeId")
);

create index "IX_release_gameVersionId"
	on "release" ("gameVersionId");

create index "IX_release_mappingTypeId"
	on "release" ("mappingTypeId");

create index "IX_release_name"
	on "release" ("name");

