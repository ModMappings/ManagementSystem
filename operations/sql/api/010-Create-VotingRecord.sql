create table "voting_record"
(
	"id" uuid not null
		constraint "PK_voting_record"
			primary key,
	"proposalId" uuid not null
		constraint "FK_voting_record_proposed_mapping_proposalId"
			references "proposed_mapping"
				on delete cascade,
	"createdOn" timestamp not null,
	"votedBy" uuid not null,
	"isForVote" boolean not null,
	"hasBeenRescinded" boolean not null
);

create index "IX_voting_record_proposalId"
	on "voting_record" ("proposalId");

