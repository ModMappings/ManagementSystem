create table "VotingRecord"
(
	"Id" uuid not null
		constraint "PK_VotingRecord"
			primary key,
	"ProposalId" uuid not null
		constraint "FK_VotingRecord_ProposalMappingEntries_ProposalId"
			references "ProposalMappingEntries"
				on delete cascade,
	"CreatedOn" timestamp not null,
	"VotedBy" uuid not null,
	"IsForVote" boolean not null,
	"HasBeenRescinded" boolean not null
);

alter table "VotingRecord" owner to "mcp-migrations";

create index "IX_VotingRecord_ProposalId"
	on "VotingRecord" ("ProposalId");

