create table "mapping_type"
(
	"id" uuid not null
		constraint "PK_mapping_type"
			primary key,
	"name" text not null,
	"created_by" uuid not null,
	"created_on" timestamp not null,
	"visible" boolean not null ,
	"editable" boolean not null
);

create unique index "IX_mapping_type_name"
	on "mapping_type" ("name");
