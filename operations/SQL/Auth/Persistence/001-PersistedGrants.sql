create table "PersistedGrants"
(
	"Key" varchar(200) not null
		constraint "PK_PersistedGrants"
			primary key,
	"Type" varchar(50) not null,
	"SubjectId" varchar(200),
	"ClientId" varchar(200) not null,
	"CreationTime" timestamp not null,
	"Expiration" timestamp,
	"Data" varchar(50000) not null
);

alter table "PersistedGrants" owner to "auth-persisted";

create index "IX_PersistedGrants_SubjectId_ClientId_Type"
	on "PersistedGrants" ("SubjectId", "ClientId", "Type");

