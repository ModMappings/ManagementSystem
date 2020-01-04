create table "ClientSecrets"
(
	"Id" serial not null
		constraint "PK_ClientSecrets"
			primary key,
	"Description" varchar(2000),
	"Value" varchar(4000) not null,
	"Expiration" timestamp,
	"Type" varchar(250) not null,
	"Created" timestamp not null,
	"ClientId" integer not null
		constraint "FK_ClientSecrets_Clients_ClientId"
			references "Clients"
				on delete cascade
);

alter table "ClientSecrets" owner to "auth-configuration";

create index "IX_ClientSecrets_ClientId"
	on "ClientSecrets" ("ClientId");

