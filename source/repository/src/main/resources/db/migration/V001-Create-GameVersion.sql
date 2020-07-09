create table "game_version"
(
    "id"             uuid      not null
        constraint "PK_game_version"
            primary key
        default uuid_generate_v4(),
    "created_on"     timestamp not null,
    "created_by"     uuid      not null,
    "name"           text      not null,
    "is_pre_release" boolean   not null,
    "is_snapshot"    boolean   not null
);

create unique index "IX_game_version_name"
    on "game_version" ("name");

