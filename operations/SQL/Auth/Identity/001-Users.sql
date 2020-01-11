create table "Users"
(
	"Id" text not null
		constraint "PK_Users"
			primary key,
	"UserName" varchar(256),
	"NormalizedUserName" varchar(256),
	"Email" varchar(256),
	"NormalizedEmail" varchar(256),
	"EmailConfirmed" boolean not null,
	"PasswordHash" text,
	"SecurityStamp" text,
	"ConcurrencyStamp" text,
	"PhoneNumber" text,
	"PhoneNumberConfirmed" boolean not null,
	"TwoFactorEnabled" boolean not null,
	"LockoutEnd" timestamp with time zone,
	"LockoutEnabled" boolean not null,
	"AccessFailedCount" integer not null
);

alter table "Users" owner to "auth-identity";

create index "EmailIndex"
	on "Users" ("NormalizedEmail");

create unique index "UserNameIndex"
	on "Users" ("NormalizedUserName");

