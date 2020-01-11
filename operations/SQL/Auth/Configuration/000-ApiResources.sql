create table "ApiResources"
(
	"Id" serial not null
		constraint "PK_ApiResources"
			primary key,
	"Enabled" boolean not null,
	"Name" varchar(200) not null,
	"DisplayName" varchar(200),
	"Description" varchar(1000),
	"Created" timestamp not null,
	"Updated" timestamp,
	"LastAccessed" timestamp,
	"NonEditable" boolean not null
);

alter table "ApiResources" owner to "auth-configuration";

create unique index "IX_ApiResources_Name"
	on "ApiResources" ("Name");

