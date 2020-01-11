create table "ClientCorsOrigins"
(
	"Id" serial not null
		constraint "PK_ClientCorsOrigins"
			primary key,
	"Origin" varchar(150) not null,
	"ClientId" integer not null
		constraint "FK_ClientCorsOrigins_Clients_ClientId"
			references "Clients"
				on delete cascade
);

alter table "ClientCorsOrigins" owner to "auth-configuration";

create index "IX_ClientCorsOrigins_ClientId"
	on "ClientCorsOrigins" ("ClientId");

