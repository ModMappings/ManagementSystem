create table "UserLogins"
(
	"LoginProvider" text not null,
	"ProviderKey" text not null,
	"ProviderDisplayName" text,
	"UserId" text not null
		constraint "FK_UserLogins_Users_UserId"
			references "Users"
				on delete cascade,
	constraint "PK_UserLogins"
		primary key ("LoginProvider", "ProviderKey")
);

alter table "UserLogins" owner to "auth-identity";

create index "IX_UserLogins_UserId"
	on "UserLogins" ("UserId");

