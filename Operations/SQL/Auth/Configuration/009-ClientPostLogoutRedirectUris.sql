create table "ClientPostLogoutRedirectUris"
(
	"Id" serial not null
		constraint "PK_ClientPostLogoutRedirectUris"
			primary key,
	"PostLogoutRedirectUri" varchar(2000) not null,
	"ClientId" integer not null
		constraint "FK_ClientPostLogoutRedirectUris_Clients_ClientId"
			references "Clients"
				on delete cascade
);

alter table "ClientPostLogoutRedirectUris" owner to "auth-configuration";

create index "IX_ClientPostLogoutRedirectUris_ClientId"
	on "ClientPostLogoutRedirectUris" ("ClientId");

