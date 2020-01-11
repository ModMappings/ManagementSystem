create table "proposed_mapping"
(
	"id" uuid not null
		constraint "PK_proposed_mapping"
			primary key,
	"versionedMappableId" uuid not null
		constraint "FK_proposed_mapping_versioned_mappable_versionedMappableId"
			references "versioned_mappable"
				on delete cascade,
	"createdOn" timestamp not null,
	"input" text not null,
	"output" text not null,
	"documentation" text,
	"distribution" integer not null,
	"mappingTypeId" uuid not null
		constraint "FK_proposed_mapping_mapping_type_mappingTypeId"
			references "mapping_type"
				on delete cascade,
	"createdBy" uuid not null,
	"isPublic" boolean not null,
	"closedBy" uuid,
	"closedOn" timestamp,
	"mappingId" uuid
		constraint "FK_proposed_mapping_mapping_mappingId"
			references "mapping"
				on delete restrict
);

create index "IX_proposed_mapping_mappingTypeId"
	on "proposed_mapping" ("mappingTypeId");

create index "IX_proposed_mapping_versionedMappableId"
	on "proposed_mapping" ("versionedMappableId");

create unique index "IX_proposed_mapping_mappingId"
	on "proposed_mapping" ("mappingId");

