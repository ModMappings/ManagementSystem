create table "ApiProperties"
(
	"Id" serial not null
		constraint "PK_ApiProperties"
			primary key,
	"Key" varchar(250) not null,
	"Value" varchar(2000) not null,
	"ApiResourceId" integer not null
		constraint "FK_ApiProperties_ApiResources_ApiResourceId"
			references "ApiResources"
				on delete cascade
);

alter table "ApiProperties" owner to "auth-configuration";

create index "IX_ApiProperties_ApiResourceId"
	on "ApiProperties" ("ApiResourceId");

