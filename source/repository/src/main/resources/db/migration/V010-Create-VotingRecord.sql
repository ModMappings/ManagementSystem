create table "voting_record"
(
	"id" uuid not null
		constraint "PK_voting_record"
            primary key
                default uuid_generate_v4(),
	"proposal_id" uuid not null
		constraint "FK_voting_record_proposed_mapping_proposalId"
			references "proposed_mapping"
				on delete cascade,
	"created_on" timestamp not null,
	"voted_by" uuid not null,
	"is_for_vote" boolean not null,
	"has_been_rescinded" boolean not null
);

create index "IX_voting_record_proposalId"
	on "voting_record" ("proposal_id");

