create table "protected_mappable"
(
    "id"                    uuid not null
        constraint "PK_protected_mappable"
            primary key
        default uuid_generate_v4(),
    "versioned_mappable_id" uuid not null
        constraint "FK_protected_mappable_versioned_mappable_versionedMappableId"
            references "versioned_mappable"
            on delete cascade,
    "mapping_type_id"       uuid not null
        constraint "FK_protected_mappable_mapping_type_mappingTypeId"
            references "mapping_type"
            on delete cascade,

    unique ("versioned_mappable_id", "mapping_type_id")
);

create index "IX_protected_mappable_mappingTypeId"
    on "protected_mappable" ("mapping_type_id");

create index "IX_protected_mappable_versionedMappableId"
    on "protected_mappable" ("versioned_mappable_id");