create table "mapping_type"
(
	"id" uuid not null
		constraint "PK_mapping_type"
			primary key,
	"name" text not null,
	"createdBy" uuid not null,
	"createdOn" timestamp not null
);

alter table "mapping_type" owner to "dbo";

create unique index "IX_mapping_type_name"
	on "mapping_type" ("name");
