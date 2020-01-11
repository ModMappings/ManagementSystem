create table "ApiScopeClaims"
(
	"Id" serial not null
		constraint "PK_ApiScopeClaims"
			primary key,
	"Type" varchar(200) not null,
	"ApiScopeId" integer not null
		constraint "FK_ApiScopeClaims_ApiScopes_ApiScopeId"
			references "ApiScopes"
				on delete cascade
);

alter table "ApiScopeClaims" owner to "auth-configuration";

create index "IX_ApiScopeClaims_ApiScopeId"
	on "ApiScopeClaims" ("ApiScopeId");

