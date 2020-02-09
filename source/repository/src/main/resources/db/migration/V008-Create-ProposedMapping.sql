create table "proposed_mapping"
(
	"id" uuid not null
		constraint "PK_proposed_mapping"
            primary key
                default uuid_generate_v4(),
	"versioned_mappable_id" uuid not null
		constraint "FK_proposed_mapping_versioned_mappable_versionedMappableId"
			references "versioned_mappable"
				on delete cascade,
	"created_on" timestamp not null,
	"input" text not null,
	"output" text not null,
	"documentation" text,
	"distribution" text not null,
	"mapping_type_id" uuid not null
		constraint "FK_proposed_mapping_mapping_type_mappingTypeId"
			references "mapping_type"
				on delete cascade,
	"created_by" uuid not null,
	"is_public" boolean not null,
	"closed_by" uuid,
	"closed_on" timestamp,
	"mapping_id" uuid
		constraint "FK_proposed_mapping_mapping_mappingId"
			references "mapping"
				on delete restrict
);

create index "IX_proposed_mapping_mappingTypeId"
	on "proposed_mapping" ("mapping_type_id");

create index "IX_proposed_mapping_versionedMappableId"
	on "proposed_mapping" ("versioned_mappable_id");

create unique index "IX_proposed_mapping_mappingId"
	on "proposed_mapping" ("mapping_id");

