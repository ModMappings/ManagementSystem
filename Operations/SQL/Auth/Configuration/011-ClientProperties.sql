create table "ClientProperties"
(
	"Id" serial not null
		constraint "PK_ClientProperties"
			primary key,
	"Key" varchar(250) not null,
	"Value" varchar(2000) not null,
	"ClientId" integer not null
		constraint "FK_ClientProperties_Clients_ClientId"
			references "Clients"
				on delete cascade
);

alter table "ClientProperties" owner to "auth-configuration";

create index "IX_ClientProperties_ClientId"
	on "ClientProperties" ("ClientId");

