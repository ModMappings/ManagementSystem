create table "versioned_mappable"
(
	"id" uuid not null
		constraint "PK_versioned_mappable"
			primary key,
	"gameVersionId" uuid not null
		constraint "FK_versioned_mappable_game_version_gameVersionId"
			references "game_version"
				on delete cascade,
	"createdBy" uuid not null,
	"createdOn" timestamp not null,
	"mappableId" uuid not null
		constraint "FK_versioned_mappable_mappable_mappableId"
			references "mappable"
				on delete cascade,
	
	"parentPackageId" uuid
	    constraint "FK_versioned_mappable_versioned_mappable_parentPackageId"
	        references "versioned_mappable"
	            on delete restrict,
	"parentClassId" uuid
	    constraint "FK_versioned_mappable_versioned_mappable_parentClassId"
	        references "versioned_mappable"
	            on delete restrict,
	"parentMethodId" uuid
	    constraint "FK_versioned_mappable_versioned_mappable_parentMethodId"
	        references "versioned_mappable"
	            on delete restrict,

	"visibility" integer,
	"isStatic" boolean,
	"type" text,
	"descriptor" text
);

create index "IX_versioned_mappable_mappableId"
	on "versioned_mappable" ("mappableId");

create index "IX_versioned_mappable_gameVersionId"
	on "versioned_mappable" ("gameVersionId");

create index "IX_versioned_mappable_parentPackageId"
    on "versioned_mappable" ("parentPackageId");

create index "IX_versioned_mappable_parentClassId"
    on "versioned_mappable" ("parentClassId");

create index "IX_versioned_mappable_parentMethodId"
    on "versioned_mappable" ("parentMethodId");
