create table "packages"
(
    "path" text not null,
    "name" text not null,
    "parent_path" text not null,
    "parent_parent_path" text not null,
    "created_by" uuid default '00000000-0000-0000-0000-000000000000'::uuid not null,
    "created_on" timestamp default '0001-01-01 00:00:00'::timestamp not null,

    constraint "PK_packages_path_parent" primary key (path, parent_path)
);

create index "IX_packages_path"
    on packages (path);

create index "IX_packages_parent_path"
    on packages (parent_path);

alter table mmms.public.mapping
    add column package_path text,
    add column package_parent_path text,
    add constraint "FK_mapping_packages" foreign key (package_path, package_parent_path) references packages (path, parent_path);

insert into packages (path, name, parent_path, parent_parent_path, created_by, created_on)
    values ('', '', '', '', '00000000-0000-0000-0000-000000000000', now());

alter table packages
    alter column created_on set default 'now()';

delete from packages p where true;

insert into packages (path, name, parent_path, parent_parent_path)
    select distinct on (coalesce(substring(m.output, '([a-zA-Z0-9$-_]+)\/'), ''),  coalesce(substring(m.output, '(?:[a-zA-Z0-9$\-_]+\/)*([a-zA-Z0-9$\-_]+)\/[a-zA-Z0-9$\-_]+'), ''))
                    coalesce(substring(m.output, '([a-zA-Z0-9$-_]+)\/'), '') as path,
                    coalesce(substring(m.output, '(?:[a-zA-Z0-9$\-_]+\/)*([a-zA-Z0-9$\-_]+)\/[a-zA-Z0-9$\-_]+'), '') as name,
                    coalesce(substring(coalesce(substring(m.output, '([a-zA-Z0-9$-_]+)\/'), ''), '([a-zA-Z0-9$-_]+)\/'), '') as parent_path,
                    coalesce(substring(coalesce(substring(coalesce(substring(m.output, '([a-zA-Z0-9$-_]+)\/'), ''), '([a-zA-Z0-9$-_]+)\/'), ''), '([a-zA-Z0-9$-_]+)\/'), '') as parent_parent_path
                    from mapping m
                    where m.mappable_type = 'CLASS'
                    order by coalesce(substring(m.output, '([a-zA-Z0-9$-_]+)\/'), ''),  coalesce(substring(m.output, '(?:[a-zA-Z0-9$\-_]+\/)*([a-zA-Z0-9$\-_]+)\/[a-zA-Z0-9$\-_]+'), '')
on conflict do nothing ;


do $$
begin
for r in 1..20 loop
    insert into packages (path, name, parent_path, parent_parent_path)
        select distinct on (p.parent_path, coalesce(substring(p.parent_path, '(?:[a-zA-Z0-9$\-_]+\/)*([a-zA-Z0-9$\-_]+)'), ''))
            p.parent_path                                                                                                                                         as path,
            coalesce(substring(p.parent_path, '(?:[a-zA-Z0-9$\-_]+\/)*([a-zA-Z0-9$\-_]+)'), '')                                                                   as name,
            coalesce(substring(p.parent_path, '([a-zA-Z0-9$-_]+)\/'), '')                                         as parent_path,
            coalesce(substring(coalesce(substring(p.parent_path, '([a-zA-Z0-9$-_]+)\/'), ''), '([a-zA-Z0-9$-_]+)\/'), '')                   as parent_parent_path
        from packages p
        order by p.parent_path, coalesce(substring(p.parent_path, '(?:[a-zA-Z0-9$\-_]+\/)*([a-zA-Z0-9$\-_]+)'), '')
        on conflict do nothing;
end loop;
end$$;

alter table packages
    add constraint "FK_packages_packages_parent" foreign key (parent_path, parent_parent_path) references packages (path, parent_path);

update mmms.public.mapping
    set package_path = coalesce(substring(mapping.output, '([a-zA-Z0-9$-_]+)\/'), ''),
        package_parent_path = coalesce(substring(coalesce(substring(mapping.output, '([a-zA-Z0-9$-_]+)\/'), ''), '([a-zA-Z0-9$-_]+)\/'), '')
where mappable_type = 'CLASS' and package_path is null and package_parent_path is null;