create table "ApiClaims"
(
	"Id" serial not null
		constraint "PK_ApiClaims"
			primary key,
	"Type" varchar(200) not null,
	"ApiResourceId" integer not null
		constraint "FK_ApiClaims_ApiResources_ApiResourceId"
			references "ApiResources"
				on delete cascade
);

alter table "ApiClaims" owner to "auth-configuration";

create index "IX_ApiClaims_ApiResourceId"
	on "ApiClaims" ("ApiResourceId");

