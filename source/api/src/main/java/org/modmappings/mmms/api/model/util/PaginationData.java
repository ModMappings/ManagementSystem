package org.modmappings.mmms.api.model.util;

import io.swagger.v3.oas.annotations.media.Schema;
import org.apache.commons.lang3.StringUtils;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

public class PaginationData {

    @Schema(hidden = true)
    private boolean paged = false;
    private int page = 0;
    private int size = 25;
    private List<String> sort = new ArrayList<>();

    public PaginationData() {
    }

    public PaginationData(int page, int size, List<String> sort) {
        this.paged = true;
        this.page = page;
        this.size = size;
        this.sort = sort;
    }

    public boolean isPaged() {
        return paged;
    }

    public int getPage() {
        return page;
    }

    public void setPage(int page) {
        this.page = page;
        this.paged = true;
    }

    public int getSize() {
        return size;
    }

    public void setSize(int size) {
        this.size = size;
        this.paged = true;
    }

    public List<String> getSort() {
        return sort;
    }

    public void setSort(List<String> sort) {
        this.sort = sort;
        this.paged = true;
    }

    public Pageable toPageable() {
        if (!isPaged())
            return Pageable.unpaged();

        if (getSort().isEmpty())
            return PageRequest.of(getPage(), getSize());

        final List<Sort.Order> orderData = getSort().stream()
                .filter(s -> !StringUtils.isEmpty(s))
                .map(sortData -> {
                    final String[] data = sortData.split(",");
                    if (data.length >= 3)
                        throw new IllegalArgumentException("Can not parse sort data: " + sortData + " it contains three or more sections!");

                    if (data.length == 1)
                        return new Sort.Order(Sort.DEFAULT_DIRECTION, data[0]);

                    return new Sort.Order(Sort.Direction.fromString(data[1]), data[0]);
                })
                .collect(Collectors.toList());

        final Sort pageableSort = Sort.by(orderData);

        return PageRequest.of(getPage(), getSize(), pageableSort);
    }
}
