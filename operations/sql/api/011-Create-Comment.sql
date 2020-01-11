create table "comment"
(
	"id" uuid not null
		constraint "PK_comment"
			primary key,
	"createdBy" uuid not null,
	"createdOn" timestamp not null,
	"content" text not null,
    "deletedBy" uuid,
    "deletedOn" timestamp,
    "lastEditBy" uuid,
    "lastEditOn" timestamp,
	"proposedMappingId" uuid
		constraint "FK_comment_proposed_mapping_proposedMappingId"
			references "proposed_mapping"
				on delete restrict,
	"releaseId" uuid
		constraint "FK_comment_releases_releaseId"
			references "release"
				on delete restrict,
	"parentCommentId" uuid
		constraint "FK_comment_comment_parentCommentId"
			references "comment"
				on delete restrict
);

create index "IX_comment_parentCommentId"
	on "comment" ("parentCommentId");

create index "IX_comment_proposedMappingId"
	on "comment" ("proposedMappingId");

create index "IX_comment_releaseId"
	on "comment" ("releaseId");

