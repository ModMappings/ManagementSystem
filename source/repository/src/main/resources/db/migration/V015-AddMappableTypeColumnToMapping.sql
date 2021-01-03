alter table mmms.public.mapping
    drop constraint if exists "fk_mapping_versioned_mappable_versionmappableid_gameversionid",
    drop constraint if exists  "FK_mapping_mapping_type_mappingTypeId";

alter table mmms.public.versioned_mappable
    drop constraint if exists  "FK_versioned_mappable_mappable_mappableId",
    drop constraint if exists  "UQ_versioned_mappable_versionedMappableId_gameVersionId";

alter table mappable
    add constraint "UQ_mappable_id_type" unique (id, type);

alter table mmms.public.versioned_mappable
    add column mappable_type text;

update mmms.public.versioned_mappable
    set mappable_type = mappable.type
    from mmms.public.mappable
    where mmms.public.versioned_mappable.mappable_id = mappable.id;

alter table mmms.public.versioned_mappable
    alter column mappable_type set not null,
    add constraint "FK_mappable_mappableId_type" foreign key (mappable_id, mappable_type) references mmms.public.mappable(id, type),
    add constraint "UQ_versioned_mappable_versionedMappableId_gameVersionId_mappableId_mappableType" unique (id, game_version_id, mappable_id, mappable_type);

alter table mmms.public.mapping
    add column mappable_type text,
    add column mappable_id uuid;

update mmms.public.mapping
    set mappable_type = versioned_mappable.mappable_type,
        mappable_id = versioned_mappable.mappable_id
    from mmms.public.versioned_mappable
    where mmms.public.mapping.versioned_mappable_id = versioned_mappable.id;

alter table mmms.public.mapping
    alter column mappable_type set not null,
    alter column mappable_id set not null,
    add constraint FK_mapping_versioned_mappable_versionMappableId_gameVersionId_mappableId_mappableType foreign key (versioned_mappable_id, game_version_id, mappable_id, mappable_type) references mmms.public.versioned_mappable (id, game_version_id, mappable_id, mappable_type);

