create table "comment_reaction"
(
	"id" uuid not null
		constraint "PK_comment_reaction"
			primary key,
	"createdBy" uuid not null,
	"createdOn" timestamp not null,
	"type" integer not null,
	"commentId" uuid not null
		constraint "FK_comment_reaction_comment_commentId"
			references "comment"
				on delete cascade
);

create index "IX_comment_reaction_commentId"
	on "comment_reaction" ("commentId");

