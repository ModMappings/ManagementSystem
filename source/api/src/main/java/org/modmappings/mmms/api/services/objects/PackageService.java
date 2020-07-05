package org.modmappings.mmms.api.services.objects;

public class PackageService {

    /*
    SELECT  DISTINCT ON (trim(both '/' from substring(m.output, '(\Anet/minecraft/([a-zA-Z]+/){0,1})')), vm.id) trim(both '/' from substring(m.output, '(\Anet/minecraft/([a-zA-Z]+/){0,1})')) as package, vm.id as class_id from mapping m
    JOIN versioned_mappable vm on m.versioned_mappable_id = vm.id
    JOIN mappable mp on vm.mappable_id = mp.id
    WHERE (m.output ~ '\Anet/minecraft/([a-zA-Z]+/){0,1}[a-zA-Z]+\Z') AND m.mapping_type_id = '15af2c0b-e672-44ff-9a85-65f394274a02' and mp.type = 'CLASS'
    order by trim(both '/' from substring(m.output, '(\Anet/minecraft/([a-zA-Z]+/){0,1})')), vm.id , m.created_on desc;
     */


}
