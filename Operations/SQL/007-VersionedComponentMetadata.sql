create table "VersionedComponentMetadata"
(
	"Id" uuid not null
		constraint "PK_VersionedComponentMetadata"
			primary key
		constraint "FK_VersionedComponentMetadata_VersionedComponents_Id"
			references "VersionedComponents"
				on delete cascade,
	"Discriminator" text not null,
	"ParentId" uuid
		constraint "FK_VersionedComponentMetadata_VersionedComponentMetadata_Paren~"
			references "VersionedComponentMetadata"
				on delete restrict,
	"Type" text,
	"MemberOfId" uuid
		constraint "FK_VersionedComponentMetadata_VersionedComponentMetadata_Membe~"
			references "VersionedComponentMetadata"
				on delete cascade,
	"IsStatic" boolean,
	"MethodMetadata_MemberOfId" uuid
		constraint "FK_VersionedComponentMetadata_VersionedComponentMetadata_Metho~"
			references "VersionedComponentMetadata"
				on delete cascade,
	"MethodMetadata_IsStatic" boolean,
	"Descriptor" text,
	"ParameterOfId" uuid
		constraint "FK_VersionedComponentMetadata_VersionedComponentMetadata_Param~"
			references "VersionedComponentMetadata"
				on delete cascade,
	"Index" integer,
	"OuterId" uuid
		constraint "FK_VersionedComponentMetadata_VersionedComponentMetadata_Outer~"
			references "VersionedComponentMetadata"
				on delete restrict,
	"PackageId" uuid
		constraint "FK_VersionedComponentMetadata_VersionedComponentMetadata_Packa~"
			references "VersionedComponentMetadata"
				on delete restrict
);

alter table "VersionedComponentMetadata" owner to "mcp-migrations";

create index "IX_VersionedComponentMetadata_ParentId"
	on "VersionedComponentMetadata" ("ParentId");

create index "IX_VersionedComponentMetadata_MemberOfId"
	on "VersionedComponentMetadata" ("MemberOfId");

create index "IX_VersionedComponentMetadata_MethodMetadata_MemberOfId"
	on "VersionedComponentMetadata" ("MethodMetadata_MemberOfId");

create index "IX_VersionedComponentMetadata_ParameterOfId"
	on "VersionedComponentMetadata" ("ParameterOfId");

create index "IX_VersionedComponentMetadata_OuterId"
	on "VersionedComponentMetadata" ("OuterId");

create index "IX_VersionedComponentMetadata_PackageId"
	on "VersionedComponentMetadata" ("PackageId");

