create table "ClientGrantTypes"
(
	"Id" serial not null
		constraint "PK_ClientGrantTypes"
			primary key,
	"GrantType" varchar(250) not null,
	"ClientId" integer not null
		constraint "FK_ClientGrantTypes_Clients_ClientId"
			references "Clients"
				on delete cascade
);

alter table "ClientGrantTypes" owner to "auth-configuration";

create index "IX_ClientGrantTypes_ClientId"
	on "ClientGrantTypes" ("ClientId");

