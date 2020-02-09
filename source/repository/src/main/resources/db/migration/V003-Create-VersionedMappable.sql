create table "versioned_mappable"
(
	"id" uuid not null
		constraint "PK_versioned_mappable"
            primary key
                default uuid_generate_v4(),
	"game_version_id" uuid not null
		constraint "FK_versioned_mappable_game_version_gameVersionId"
			references "game_version"
				on delete cascade,
	"created_by" uuid not null,
	"created_on" timestamp not null,
	"mappable_id" uuid not null
		constraint "FK_versioned_mappable_mappable_mappableId"
			references "mappable"
				on delete cascade,

	"parent_class_id" uuid
	    constraint "FK_versioned_mappable_versioned_mappable_parentClassId"
	        references "versioned_mappable"
	            on delete restrict,
	"parent_method_id" uuid
	    constraint "FK_versioned_mappable_versioned_mappable_parentMethodId"
	        references "versioned_mappable"
	            on delete restrict,

	"visibility" text,
	"is_static" boolean,
	"type" text,
	"descriptor" text
);

create index "IX_versioned_mappable_mappableId"
	on "versioned_mappable" ("mappable_id");

create index "IX_versioned_mappable_gameVersionId"
	on "versioned_mappable" ("game_version_id");

create index "IX_versioned_mappable_parentPackageId"
    on "versioned_mappable" ("parent_package_id");

create index "IX_versioned_mappable_parentClassId"
    on "versioned_mappable" ("parent_class_id");

create index "IX_versioned_mappable_parentMethodId"
    on "versioned_mappable" ("parent_method_id");
