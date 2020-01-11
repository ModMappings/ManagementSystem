create table "Roles"
(
	"Id" text not null
		constraint "PK_Roles"
			primary key,
	"Name" varchar(256),
	"NormalizedName" varchar(256),
	"ConcurrencyStamp" text
);

alter table "Roles" owner to "auth-identity";

create unique index "RoleNameIndex"
	on "Roles" ("NormalizedName");

