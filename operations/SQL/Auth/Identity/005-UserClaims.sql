create table "UserClaims"
(
	"Id" serial not null
		constraint "PK_UserClaims"
			primary key,
	"UserId" text not null
		constraint "FK_UserClaims_Users_UserId"
			references "Users"
				on delete cascade,
	"ClaimType" text,
	"ClaimValue" text
);

alter table "UserClaims" owner to "auth-identity";

create index "IX_UserClaims_UserId"
	on "UserClaims" ("UserId");

