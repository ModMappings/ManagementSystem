create table "UserTokens"
(
	"UserId" text not null
		constraint "FK_UserTokens_Users_UserId"
			references "Users"
				on delete cascade,
	"LoginProvider" text not null,
	"Name" text not null,
	"Value" text,
	constraint "PK_UserTokens"
		primary key ("UserId", "LoginProvider", "Name")
);

alter table "UserTokens" owner to "auth-identity";

