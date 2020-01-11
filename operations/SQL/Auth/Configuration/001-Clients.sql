create table "Clients"
(
	"Id" serial not null
		constraint "PK_Clients"
			primary key,
	"Enabled" boolean not null,
	"ClientId" varchar(200) not null,
	"ProtocolType" varchar(200) not null,
	"RequireClientSecret" boolean not null,
	"ClientName" varchar(200),
	"Description" varchar(1000),
	"ClientUri" varchar(2000),
	"LogoUri" varchar(2000),
	"RequireConsent" boolean not null,
	"AllowRememberConsent" boolean not null,
	"AlwaysIncludeUserClaimsInIdToken" boolean not null,
	"RequirePkce" boolean not null,
	"AllowPlainTextPkce" boolean not null,
	"AllowAccessTokensViaBrowser" boolean not null,
	"FrontChannelLogoutUri" varchar(2000),
	"FrontChannelLogoutSessionRequired" boolean not null,
	"BackChannelLogoutUri" varchar(2000),
	"BackChannelLogoutSessionRequired" boolean not null,
	"AllowOfflineAccess" boolean not null,
	"IdentityTokenLifetime" integer not null,
	"AccessTokenLifetime" integer not null,
	"AuthorizationCodeLifetime" integer not null,
	"ConsentLifetime" integer,
	"AbsoluteRefreshTokenLifetime" integer not null,
	"SlidingRefreshTokenLifetime" integer not null,
	"RefreshTokenUsage" integer not null,
	"UpdateAccessTokenClaimsOnRefresh" boolean not null,
	"RefreshTokenExpiration" integer not null,
	"AccessTokenType" integer not null,
	"EnableLocalLogin" boolean not null,
	"IncludeJwtId" boolean not null,
	"AlwaysSendClientClaims" boolean not null,
	"ClientClaimsPrefix" varchar(200),
	"PairWiseSubjectSalt" varchar(200),
	"Created" timestamp not null,
	"Updated" timestamp,
	"LastAccessed" timestamp,
	"UserSsoLifetime" integer,
	"UserCodeType" varchar(100),
	"DeviceCodeLifetime" integer not null,
	"NonEditable" boolean not null
);

alter table "Clients" owner to "auth-configuration";

create unique index "IX_Clients_ClientId"
	on "Clients" ("ClientId");

