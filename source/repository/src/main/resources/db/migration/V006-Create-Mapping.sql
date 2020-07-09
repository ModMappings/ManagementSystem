create table "mapping"
(
    "id"                    uuid      not null
        constraint "PK_mapping"
            primary key
        default uuid_generate_v4(),
    "versioned_mappable_id" uuid      not null
        constraint "FK_mapping_versioned_mappable_versionedMappableId"
            references "versioned_mappable"
            on delete cascade,
    "created_on"            timestamp not null,
    "input"                 text      not null,
    "output"                text      not null,
    "documentation"         text,
    "distribution"          text      not null,
    "mapping_type_id"       uuid      not null
        constraint "FK_mapping_mapping_type_mappingTypeId"
            references "mapping_type"
            on delete cascade,
    "created_by"            uuid      not null
);

create index "IX_mapping_mappingTypeId"
    on "mapping" ("mapping_type_id");

create index "IX_mapping_versionedMappableId"
    on "mapping" ("created_by");

