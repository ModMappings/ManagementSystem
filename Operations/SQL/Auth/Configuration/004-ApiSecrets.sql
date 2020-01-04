create table "ApiSecrets"
(
	"Id" serial not null
		constraint "PK_ApiSecrets"
			primary key,
	"Description" varchar(1000),
	"Value" varchar(4000) not null,
	"Expiration" timestamp,
	"Type" varchar(250) not null,
	"Created" timestamp not null,
	"ApiResourceId" integer not null
		constraint "FK_ApiSecrets_ApiResources_ApiResourceId"
			references "ApiResources"
				on delete cascade
);

alter table "ApiSecrets" owner to "auth-configuration";

create index "IX_ApiSecrets_ApiResourceId"
	on "ApiSecrets" ("ApiResourceId");

