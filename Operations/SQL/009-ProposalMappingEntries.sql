create table "ProposalMappingEntries"
(
	"Id" uuid not null
		constraint "PK_ProposalMappingEntries"
			primary key,
	"VersionedComponentId" uuid not null
		constraint "FK_ProposalMappingEntries_VersionedComponents_VersionedCompone~"
			references "VersionedComponents"
				on delete cascade,
	"CreatedOn" timestamp not null,
	"InputMapping" text not null,
	"OutputMapping" text not null,
	"Documentation" text,
	"Distribution" integer not null,
	"MappingTypeId" uuid not null
		constraint "FK_ProposalMappingEntries_MappingTypes_MappingTypeId"
			references "MappingTypes"
				on delete cascade,
	"CreatedBy" uuid not null,
	"IsOpen" boolean not null,
	"IsPublicVote" boolean not null,
	"ClosedBy" uuid,
	"ClosedOn" timestamp,
	"Merged" boolean,
	"CommittedWithId" uuid
		constraint "FK_ProposalMappingEntries_LiveMappingEntries_CommittedWithId"
			references "LiveMappingEntries"
				on delete restrict
);

alter table "ProposalMappingEntries" owner to "mcp-migrations";

create index "IX_ProposalMappingEntries_MappingTypeId"
	on "ProposalMappingEntries" ("MappingTypeId");

create index "IX_ProposalMappingEntries_VersionedComponentId"
	on "ProposalMappingEntries" ("VersionedComponentId");

create unique index "IX_ProposalMappingEntries_CommittedWithId"
	on "ProposalMappingEntries" ("CommittedWithId");

