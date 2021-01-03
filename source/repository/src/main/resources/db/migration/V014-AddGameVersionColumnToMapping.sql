
alter table mmms.public.mapping
    drop constraint if exists "FK_mapping_versioned_mappable_versionedMappableId";

alter table mmms.public.mapping
    add column game_version_id uuid;

update mmms.public.mapping
    set game_version_id = versioned_mappable.game_version_id
    from mmms.public.versioned_mappable
    where mmms.public.mapping.versioned_mappable_id = versioned_mappable.id;

alter table versioned_mappable
    add constraint "UQ_versioned_mappable_versionedMappableId_gameVersionId" unique (id, game_version_id);

alter table mmms.public.mapping
    alter column game_version_id set not null,
    add constraint FK_mapping_versioned_mappable_versionMappableId_gameVersionId foreign key (versioned_mappable_id, game_version_id) references mmms.public.versioned_mappable (id, game_version_id);

