package org.modmappings.mmms.api.configuration.dozer;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import com.github.dozermapper.core.DozerBeanMapper;
import com.github.dozermapper.core.DozerBeanMapperBuilder;
import com.github.dozermapper.core.Mapper;
import com.github.dozermapper.core.loader.api.BeanMappingBuilder;
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

        DozerBeanMapperBuilder dozerBeanMapperBuilder = DozerBeanMapperBuilder.create();
        dozerBeanMapperBuilder.withMappingFiles(mappingFileUrlList);

        dozerBeanMapperBuilder.withMappingBuilder(getGameVersionMapping());

        return dozerBeanMapperBuilder.build();
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
