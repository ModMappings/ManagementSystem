create table "inheritance_data"
(
	"id" uuid not null
		constraint "PK_inheritance_data"
			primary key,
	"sub_type_versioned_mappable_id" uuid
		constraint "FK_inheritance_data_versioned_mappable_subTypeId"
			references "versioned_mappable"
				on delete restrict,
	"super_type_versioned_mappable_id" uuid
		constraint "FK_inheritance_data_versioned_mappable_superTypeId"
			references "versioned_mappable"
				on delete restrict
);

create index "IX_inheritance_data_subTypeId"
	on "inheritance_data" ("sub_type_versioned_mappable_id");

create index "IX_inheritance_data_superTypeId"
	on "inheritance_data" ("super_type_versioned_mappable_id");

