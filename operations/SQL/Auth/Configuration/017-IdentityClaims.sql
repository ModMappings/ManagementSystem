create table "IdentityClaims"
(
	"Id" serial not null
		constraint "PK_IdentityClaims"
			primary key,
	"Type" varchar(200) not null,
	"IdentityResourceId" integer not null
		constraint "FK_IdentityClaims_IdentityResources_IdentityResourceId"
			references "IdentityResources"
				on delete cascade
);

alter table "IdentityClaims" owner to "auth-configuration";

create index "IX_IdentityClaims_IdentityResourceId"
	on "IdentityClaims" ("IdentityResourceId");

