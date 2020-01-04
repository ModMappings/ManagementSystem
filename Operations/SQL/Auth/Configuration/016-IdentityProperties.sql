create table "IdentityProperties"
(
	"Id" serial not null
		constraint "PK_IdentityProperties"
			primary key,
	"Key" varchar(250) not null,
	"Value" varchar(2000) not null,
	"IdentityResourceId" integer not null
		constraint "FK_IdentityProperties_IdentityResources_IdentityResourceId"
			references "IdentityResources"
				on delete cascade
);

alter table "IdentityProperties" owner to "auth-configuration";

create index "IX_IdentityProperties_IdentityResourceId"
	on "IdentityProperties" ("IdentityResourceId");

