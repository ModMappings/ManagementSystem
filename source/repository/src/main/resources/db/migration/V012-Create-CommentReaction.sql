create table "comment_reaction"
(
    "id"         uuid      not null
        constraint "PK_comment_reaction"
            primary key
        default uuid_generate_v4(),
    "created_by" uuid      not null,
    "created_on" timestamp not null,
    "type"       text      not null,
    "comment_id" uuid      not null
        constraint "FK_comment_reaction_comment_commentId"
            references "comment"
            on delete cascade
);

create index "IX_comment_reaction_commentId"
    on "comment_reaction" ("comment_id");

