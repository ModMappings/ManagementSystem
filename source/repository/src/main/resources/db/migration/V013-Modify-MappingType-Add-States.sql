ALTER TABLE "mapping_type"
    ADD COLUMN "state_in" text not null default '';

ALTER TABLE "mapping_type"
    ADD COLUMN "state_out" text not null default '';
