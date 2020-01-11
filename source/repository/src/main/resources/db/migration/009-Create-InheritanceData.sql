create table "inheritance_data"
(
	"id" uuid not null
		constraint "PK_inheritance_data"
			primary key,
	"subTypeVersionedMappableId" uuid
		constraint "FK_inheritance_data_versioned_mappable_subTypeId"
			references "versioned_mappable"
				on delete restrict,
	"superTypeVersionedMappableId" uuid
		constraint "FK_inheritance_data_versioned_mappable_superTypeId"
			references "versioned_mappable"
				on delete restrict
);

create index "IX_inheritance_data_subTypeId"
	on "inheritance_data" ("subTypeVersionedMappableId");

create index "IX_inheritance_data_superTypeId"
	on "inheritance_data" ("superTypeVersionedMappableId");

