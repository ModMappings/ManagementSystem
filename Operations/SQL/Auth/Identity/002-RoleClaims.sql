create table "RoleClaims"
(
	"Id" serial not null
		constraint "PK_RoleClaims"
			primary key,
	"RoleId" text not null
		constraint "FK_RoleClaims_Roles_RoleId"
			references "Roles"
				on delete cascade,
	"ClaimType" text,
	"ClaimValue" text
);

alter table "RoleClaims" owner to "auth-identity";

create index "IX_RoleClaims_RoleId"
	on "RoleClaims" ("RoleId");

