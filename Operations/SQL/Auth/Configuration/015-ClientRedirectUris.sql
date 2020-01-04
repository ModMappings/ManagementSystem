create table "ClientRedirectUris"
(
	"Id" serial not null
		constraint "PK_ClientRedirectUris"
			primary key,
	"RedirectUri" varchar(2000) not null,
	"ClientId" integer not null
		constraint "FK_ClientRedirectUris_Clients_ClientId"
			references "Clients"
				on delete cascade
);

alter table "ClientRedirectUris" owner to "auth-configuration";

create index "IX_ClientRedirectUris_ClientId"
	on "ClientRedirectUris" ("ClientId");

