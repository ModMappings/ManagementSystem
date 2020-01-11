create table "ClientClaims"
(
	"Id" serial not null
		constraint "PK_ClientClaims"
			primary key,
	"Type" varchar(250) not null,
	"Value" varchar(250) not null,
	"ClientId" integer not null
		constraint "FK_ClientClaims_Clients_ClientId"
			references "Clients"
				on delete cascade
);

alter table "ClientClaims" owner to "auth-configuration";

create index "IX_ClientClaims_ClientId"
	on "ClientClaims" ("ClientId");

