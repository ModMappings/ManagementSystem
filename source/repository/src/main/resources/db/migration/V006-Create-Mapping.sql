create table "mapping"
(
	"id" uuid not null
		constraint "PK_mapping"
			primary key,
	"versionedMappableId" uuid not null
		constraint "FK_mapping_versioned_mappable_versionedMappableId"
			references "versioned_mappable"
				on delete cascade,
	"createdOn" timestamp not null,
	"input" text not null,
	"output" text not null,
	"documentation" text,
	"distribution" integer not null,
	"mappingTypeId" uuid not null
		constraint "FK_mapping_mapping_type_mappingTypeId"
			references "mapping_type"
				on delete cascade,
	"createdBy" uuid default '00000000-0000-0000-0000-000000000000'::uuid not null
);

create index "IX_mapping_mappingTypeId"
	on "mapping" ("mappingTypeId");

create index "IX_mapping_versionedMappableId"
	on "mapping" ("versionedMappableId");

