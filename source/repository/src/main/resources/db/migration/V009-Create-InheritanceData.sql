create table "inheritance_data"
(
	"id" uuid not null
		constraint "PK_inheritance_data"
            primary key
                default uuid_generate_v4(),
	"sub_type_versioned_mappable_id" uuid
		constraint "FK_inheritance_data_versioned_mappable_subTypeId"
			references "versioned_mappable"
				on delete cascade ,
	"super_type_versioned_mappable_id" uuid
		constraint "FK_inheritance_data_versioned_mappable_superTypeId"
			references "versioned_mappable"
				on delete cascade
);

create index "IX_inheritance_data_subTypeId"
	on "inheritance_data" ("sub_type_versioned_mappable_id");

create index "IX_inheritance_data_superTypeId"
	on "inheritance_data" ("super_type_versioned_mappable_id");

