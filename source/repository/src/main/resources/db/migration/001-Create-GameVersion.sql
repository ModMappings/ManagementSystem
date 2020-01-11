create table "game_version"
(
	"id" uuid not null
		constraint "PK_game_version"
			primary key,
	"name" text not null,
	"createdOn" timestamp not null,
	"createdBy" uuid not null,
	"isPreRelease" boolean not null,
	"isSnapshot" boolean not null
);

alter table "game_version" owner to "dbo";

create unique index "IX_game_version_name"
	on "game_version" ("name");

