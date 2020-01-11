create table "protected_mappable"
(
	"id" uuid not null
		constraint "PK_protected_mappable"
			primary key,
	"versionedMappableId" uuid not null
		constraint "FK_protected_mappable_versioned_mappable_versionedMappableId"
			references "versioned_mappable"
				on delete cascade,
	"mappingTypeId" uuid not null
		constraint "FK_protected_mappable_mapping_type_mappingTypeId"
			references "mapping_type"
				on delete cascade,

    unique ("versionedMappableId", "mappingTypeId")
);

create index "IX_protected_mappable_mappingTypeId"
	on "protected_mappable" ("mappingTypeId");

create index "IX_protected_mappable_versionedMappableId"
	on "protected_mappable" ("versionedMappableId");