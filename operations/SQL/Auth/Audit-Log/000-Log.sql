create table "Log"
(
	"Id" bigserial not null
		constraint "PK_Log"
			primary key,
	"Message" text,
	"MessageTemplate" text,
	"Level" varchar(128),
	"TimeStamp" timestamp with time zone not null,
	"Exception" text,
	"LogEvent" text,
	"Properties" text
);

alter table "Log" owner to "auth-admin-log";

