create table "comment"
(
	"id" uuid not null
		constraint "PK_comment"
			primary key,
	"created_by" uuid not null,
	"created_on" timestamp not null,
	"content" text not null,
    "deleted_by" uuid,
    "deleted_on" timestamp,
    "last_edit_by" uuid,
    "last_edit_on" timestamp,
	"proposed_mapping_id" uuid
		constraint "FK_comment_proposed_mapping_proposedMappingId"
			references "proposed_mapping"
				on delete restrict,
	"release_id" uuid
		constraint "FK_comment_releases_releaseId"
			references "release"
				on delete restrict,
	"parent_comment_id" uuid
		constraint "FK_comment_comment_parentCommentId"
			references "comment"
				on delete restrict
);

create index "IX_comment_parentCommentId"
	on "comment" ("parent_comment_id");

create index "IX_comment_proposedMappingId"
	on "comment" ("proposed_mapping_id");

create index "IX_comment_releaseId"
	on "comment" ("release_id");

