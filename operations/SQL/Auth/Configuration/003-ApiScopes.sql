create table "ApiScopes"
(
	"Id" serial not null
		constraint "PK_ApiScopes"
			primary key,
	"Name" varchar(200) not null,
	"DisplayName" varchar(200),
	"Description" varchar(1000),
	"Required" boolean not null,
	"Emphasize" boolean not null,
	"ShowInDiscoveryDocument" boolean not null,
	"ApiResourceId" integer not null
		constraint "FK_ApiScopes_ApiResources_ApiResourceId"
			references "ApiResources"
				on delete cascade
);

alter table "ApiScopes" owner to "auth-configuration";

create index "IX_ApiScopes_ApiResourceId"
	on "ApiScopes" ("ApiResourceId");

create unique index "IX_ApiScopes_Name"
	on "ApiScopes" ("Name");

