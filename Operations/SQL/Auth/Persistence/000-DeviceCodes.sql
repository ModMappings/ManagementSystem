create table "DeviceCodes"
(
	"UserCode" varchar(200) not null
		constraint "PK_DeviceCodes"
			primary key,
	"DeviceCode" varchar(200) not null,
	"SubjectId" varchar(200),
	"ClientId" varchar(200) not null,
	"CreationTime" timestamp not null,
	"Expiration" timestamp not null,
	"Data" varchar(50000) not null
);

alter table "DeviceCodes" owner to "auth-persisted";

create unique index "IX_DeviceCodes_DeviceCode"
	on "DeviceCodes" ("DeviceCode");

