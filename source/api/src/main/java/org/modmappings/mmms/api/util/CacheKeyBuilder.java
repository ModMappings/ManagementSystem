package org.modmappings.mmms.api.util;

import java.util.HashMap;
import java.util.Map;

public class CacheKeyBuilder {

    public static CacheKeyBuilder create() {
        return new CacheKeyBuilder();
    }

    private Map<String, String> target = new HashMap<>();

    public CacheKeyBuilder put(final String key, final Object value){
        this.target.put(key, ParameterUtils.toString(value));
        return this;
    }

    public Map<String, String> build() {
        final Map<String, String> target = this.target;
        this.target = new HashMap<>();
        return target;
    }
}
