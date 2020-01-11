create table "game_versions"
(
	"id" uuid not null
		constraint "PK_game_versions"
			primary key,
	"name" text not null,
	"createdOn" timestamp not null,
	"createdBy" uuid not null,
	"isPreRelease" boolean not null,
	"isSnapshot" boolean not null
);

alter table "game_versions" owner to "dbo";

create unique index "IX_game_verions_name"
	on "game_versions" ("name");

