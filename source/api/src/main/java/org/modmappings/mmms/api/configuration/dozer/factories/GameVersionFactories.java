package org.modmappings.mmms.api.configuration.dozer.factories;

import java.util.UUID;

import com.github.dozermapper.core.BeanFactory;
import com.github.dozermapper.core.config.BeanContainer;
import org.modmappings.mmms.api.model.core.GameVersionDTO;
import org.springframework.stereotype.Component;

public class GameVersionFactories {
/*
    public static class DTOtoDMOFactory implements BeanFactory {

        @Override
        public Object createBean(final Object source, final Class<?> sourceClass, final String targetBeanId, final BeanContainer beanContainer) {
            if (!(source instanceof GameVersionDTO))
                throw new IllegalArgumentException("Dozer passed in an illegal source for mapping from DTO to DMO!");

            final GameVersionDTO dto = (GameVersionDTO) source;
            if (dto.getId() == null)
            {
                //We are creating a new instance.
                //return new GameVersionDMO()
            }

            return null;
        }
    }*/
}
