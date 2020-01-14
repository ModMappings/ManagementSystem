create table "mappable"
(
	"id" uuid not null
		constraint "PK_mappable"
			primary key,
	"type" integer not null,
	"created_by" uuid default '00000000-0000-0000-0000-000000000000'::uuid not null,
	"created_cn" timestamp default '0001-01-01 00:00:00'::timestamp without time zone not null
);

