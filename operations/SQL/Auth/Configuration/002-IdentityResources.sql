create table "IdentityResources"
(
	"Id" serial not null
		constraint "PK_IdentityResources"
			primary key,
	"Enabled" boolean not null,
	"Name" varchar(200) not null,
	"DisplayName" varchar(200),
	"Description" varchar(1000),
	"Required" boolean not null,
	"Emphasize" boolean not null,
	"ShowInDiscoveryDocument" boolean not null,
	"Created" timestamp not null,
	"Updated" timestamp,
	"NonEditable" boolean not null
);

alter table "IdentityResources" owner to "auth-configuration";

create unique index "IX_IdentityResources_Name"
	on "IdentityResources" ("Name");

