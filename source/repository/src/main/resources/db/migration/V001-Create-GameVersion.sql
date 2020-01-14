create table "game_version"
(
	"id" uuid not null
		constraint "PK_game_version"
			primary key,
	"createdOn" timestamp not null,
	"createdBy" uuid not null,
	"name" text not null,
	"isPreRelease" boolean not null,
	"isSnapshot" boolean not null
);

create unique index "IX_game_version_name"
	on "game_version" ("name");

