create table "CommentReaction"
(
	"Id" uuid not null
		constraint "PK_CommentReaction"
			primary key,
	"CreatedBy" uuid not null,
	"CreatedOn" timestamp not null,
	"Type" integer not null,
	"CommentId" uuid not null
		constraint "FK_CommentReaction_Comment_CommentId"
			references "Comment"
				on delete cascade
);

alter table "CommentReaction" owner to "mcp-migrations";

create index "IX_CommentReaction_CommentId"
	on "CommentReaction" ("CommentId");

