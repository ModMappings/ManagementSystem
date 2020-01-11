create table "ClientIdPRestrictions"
(
	"Id" serial not null
		constraint "PK_ClientIdPRestrictions"
			primary key,
	"Provider" varchar(200) not null,
	"ClientId" integer not null
		constraint "FK_ClientIdPRestrictions_Clients_ClientId"
			references "Clients"
				on delete cascade
);

alter table "ClientIdPRestrictions" owner to "auth-configuration";

create index "IX_ClientIdPRestrictions_ClientId"
	on "ClientIdPRestrictions" ("ClientId");

