create extension if not exists "uuid-ossp";

create table "mappable"
(
	"id" uuid not null
		constraint "PK_mappable"
			primary key
                default uuid_generate_v4(),
	"type" text not null,
	"created_by" uuid default '00000000-0000-0000-0000-000000000000'::uuid not null,
	"created_on" timestamp default '0001-01-01 00:00:00'::timestamp not null
);

