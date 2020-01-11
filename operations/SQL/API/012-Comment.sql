create table "Comment"
(
	"Id" uuid not null
		constraint "PK_Comment"
			primary key,
	"CreatedBy" uuid not null,
	"CreatedOn" timestamp not null,
	"Content" text not null,
	"HasBeenEdited" boolean not null,
	"IsDeleted" boolean not null,
	"DeletedBy" uuid,
	"DeletedOn" timestamp,
	"ProposedMappingId" uuid
		constraint "FK_Comment_ProposalMappingEntries_ProposedMappingId"
			references "ProposalMappingEntries"
				on delete restrict,
	"ReleaseId" uuid
		constraint "FK_Comment_Releases_ReleaseId"
			references "Releases"
				on delete restrict,
	"ParentId" uuid
		constraint "FK_Comment_Comment_ParentId"
			references "Comment"
				on delete restrict
);

alter table "Comment" owner to "mcp-migrations";

create index "IX_Comment_ParentId"
	on "Comment" ("ParentId");

create index "IX_Comment_ProposedMappingId"
	on "Comment" ("ProposedMappingId");

create index "IX_Comment_ReleaseId"
	on "Comment" ("ReleaseId");

