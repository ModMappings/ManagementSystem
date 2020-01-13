package org.modmappings.mmms.api.configuration;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import org.dozer.DozerBeanMapper;
import org.dozer.Mapper;
import org.dozer.loader.api.BeanMappingBuilder;
import org.modmappings.mmms.repository.model.core.GameVersionDMO;
import org.modmappings.mmms.api.model.core.GameVersionDTO;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.io.Resource;

@Configuration
public class DozerConfiguration {

    @Bean
    public Mapper mapper(@Value(value = "classpath*:mappings/*mappings.xml") Resource[] resourceArray) throws IOException {
        List<String> mappingFileUrlList = new ArrayList<>();
        for (Resource resource : resourceArray) {
            mappingFileUrlList.add(String.valueOf(resource.getURL()));
        }

        DozerBeanMapper dozerBeanMapper = new DozerBeanMapper();
        dozerBeanMapper.setMappingFiles(mappingFileUrlList);

        dozerBeanMapper.addMapping(getGameVersionMapping());

        return dozerBeanMapper;
    }

    private BeanMappingBuilder getGameVersionMapping() {
        return new BeanMappingBuilder() {
            @Override
            protected void configure() {
                mapping(GameVersionDTO.class, GameVersionDMO.class);
            }
        };
    }
}
