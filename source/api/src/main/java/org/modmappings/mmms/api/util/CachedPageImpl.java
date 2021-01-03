package org.modmappings.mmms.api.util;

import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.databind.JsonNode;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;

import java.util.ArrayList;
import java.util.List;

public class CachedPageImpl<T> extends PageImpl<T> {

    private static final long serialVersionUID = 3248189030448292002L;

    @JsonCreator(mode = JsonCreator.Mode.PROPERTIES)
    public CachedPageImpl(@JsonProperty("content") List<T> content, @JsonProperty("number") int number, @JsonProperty("size") int size,
                          @JsonProperty("totalElements") Long totalElements, @JsonProperty("pageable") JsonNode pageable, @JsonProperty("last") boolean last,
                          @JsonProperty("totalPages") int totalPages, @JsonProperty("sort") JsonNode sort, @JsonProperty("first") boolean first,
                          @JsonProperty("numberOfElements") int numberOfElements, @JsonProperty("empty") boolean empty) {
        super(content, PageRequest.of(number, size), totalElements);
    }

    public CachedPageImpl(List<T> content, Pageable pageable, long total) {
        super(content, pageable, total);
    }

    public CachedPageImpl(List<T> content) {
        super(content);
    }

    public CachedPageImpl() {
        super(new ArrayList<T>());
    }
}
