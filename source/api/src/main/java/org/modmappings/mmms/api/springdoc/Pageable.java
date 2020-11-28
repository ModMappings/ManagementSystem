package org.modmappings.mmms.api.springdoc;


import javax.validation.constraints.Max;
import javax.validation.constraints.Min;
import javax.validation.constraints.NotNull;
import java.util.List;
import java.util.Objects;

@NotNull
public class Pageable {

    @NotNull
    @Min(0)
    private int page;

    @NotNull
    @Min(1)
    @Max(2000)
    private int size;

    private List<String> sort;

    public Pageable(@NotNull @Min(0) final int page, @NotNull @Min(1) @Max(2000) final int size, final List<String> sort) {
        this.page = page;
        this.size = size;
        this.sort = sort;
    }

    public int getPage() {
        return page;
    }

    public void setPage(final int page) {
        this.page = page;
    }

    public int getSize() {
        return size;
    }

    public void setSize(final int size) {
        this.size = size;
    }

    public List<String> getSort() {
        return sort;
    }

    public void setSort(final List<String> sort) {
        if (sort == null) {
            this.sort.clear();
        }
        this.sort = sort;
    }

    public void addSort(final String sort) {
        this.sort.add(sort);
    }

    @Override
    public boolean equals(final Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        final Pageable pageable = (Pageable) o;
        return page == pageable.page &&
                size == pageable.size &&
                Objects.equals(sort, pageable.sort);
    }

    @Override
    public int hashCode() {
        return Objects.hash(page, size, sort);
    }

    @Override
    public String toString() {
        return "Pageable{" +
                "page=" + page +
                ", size=" + size +
                ", sort=" + sort +
                '}';
    }
}