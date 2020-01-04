create table "UserRoles"
(
	"UserId" text not null
		constraint "FK_UserRoles_Users_UserId"
			references "Users"
				on delete cascade,
	"RoleId" text not null
		constraint "FK_UserRoles_Roles_RoleId"
			references "Roles"
				on delete cascade,
	constraint "PK_UserRoles"
		primary key ("UserId", "RoleId")
);

alter table "UserRoles" owner to "auth-identity";

create index "IX_UserRoles_RoleId"
	on "UserRoles" ("RoleId");

