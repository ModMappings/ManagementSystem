create table "ClassInheritanceData"
(
	"Id" uuid not null
		constraint "PK_ClassInheritanceData"
			primary key,
	"SubclassId" uuid
		constraint "FK_ClassInheritanceData_VersionedComponentMetadata_SubclassId"
			references "VersionedComponentMetadata"
				on delete restrict,
	"SuperclassId" uuid
		constraint "FK_ClassInheritanceData_VersionedComponentMetadata_SuperclassId"
			references "VersionedComponentMetadata"
				on delete restrict
);

alter table "ClassInheritanceData" owner to "mcp-migrations";

create index "IX_ClassInheritanceData_SubclassId"
	on "ClassInheritanceData" ("SubclassId");

create index "IX_ClassInheritanceData_SuperclassId"
	on "ClassInheritanceData" ("SuperclassId");

