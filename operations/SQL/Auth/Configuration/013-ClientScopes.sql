create table "ClientScopes"
(
	"Id" serial not null
		constraint "PK_ClientScopes"
			primary key,
	"Scope" varchar(200) not null,
	"ClientId" integer not null
		constraint "FK_ClientScopes_Clients_ClientId"
			references "Clients"
				on delete cascade
);

alter table "ClientScopes" owner to "auth-configuration";

create index "IX_ClientScopes_ClientId"
	on "ClientScopes" ("ClientId");

