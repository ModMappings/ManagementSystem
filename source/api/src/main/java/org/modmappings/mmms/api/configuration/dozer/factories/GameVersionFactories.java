package org.modmappings.mmms.api.configuration.dozer.factories;

import com.github.dozermapper.core.BeanFactory;
import com.github.dozermapper.core.config.BeanContainer;
import org.modmappings.mmms.api.model.core.GameVersionDTO;
import org.modmappings.mmms.repository.model.core.GameVersionDMO;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

public class GameVersionFactories {

    @Component
    public static class DTOtoDMOFactory implements BeanFactory {

        @Override
        public Object createBean(final Object source, final Class<?> sourceClass, final String targetBeanId, final BeanContainer beanContainer) {
            if (!(source instanceof GameVersionDTO))
                throw new IllegalArgumentException("Dozer passed in an illegal source for mapping from DTO to DMO!");

            final GameVersionDTO dto = (GameVersionDTO) source;
            if (dto.getId() == null)
            {
                //We are creating a new instance.
                return new GameVersionDMO()
            }

        }
    }
}
