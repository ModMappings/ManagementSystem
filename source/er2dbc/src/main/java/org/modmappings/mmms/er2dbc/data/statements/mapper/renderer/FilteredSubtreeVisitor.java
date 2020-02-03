package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.springframework.data.relational.core.sql.Visitable;
import org.springframework.data.relational.core.sql.Visitor;
import org.springframework.lang.Nullable;

import java.util.function.Predicate;

public class FilteredSubtreeVisitor extends DelegatingVisitor {

    private final Predicate<Visitable> filter;

    private @Nullable
    Visitable currentSegment;

    /**
     * Creates a new {@link FilteredSubtreeVisitor} given the filter {@link Predicate}.
     *
     * @param filter filter predicate to identify when to {@link #enterMatched(Visitable)
     *          enter}/{@link #leaveMatched(Visitable) leave} the {@link Visitable segment} that this visitor is
     *          responsible for.
     */
    FilteredSubtreeVisitor(Predicate<Visitable> filter) {
        this.filter = filter;
    }

    /**
     * {@link Visitor#enter(Visitable) Enter} callback for a {@link Visitable} that this {@link Visitor} is responsible
     * for. The default implementation retains delegation by default.
     *
     * @param segment the segment, must not be {@literal null}.
     * @return delegation options. Can be either {@link Delegation#retain()} or
     *         {@link Delegation#delegateTo(DelegatingVisitor)}.
     * @see Delegation#retain()
     */
    Delegation enterMatched(Visitable segment) {
        return Delegation.retain();
    }

    /**
     * {@link Visitor#enter(Visitable) Enter} callback for a nested {@link Visitable}. The default implementation retains
     * delegation by default.
     *
     * @param segment the segment, must not be {@literal null}.
     * @return delegation options. Can be either {@link Delegation#retain()} or
     *         {@link Delegation#delegateTo(DelegatingVisitor)}.
     * @see Delegation#retain()
     */
    Delegation enterNested(Visitable segment) {
        return Delegation.retain();
    }

    /**
     * {@link Visitor#leave(Visitable) Leave} callback for the matched {@link Visitable}. The default implementation steps
     * back from delegation by default.
     *
     * @param segment the segment, must not be {@literal null}.
     * @return delegation options. Can be either {@link Delegation#retain()} or {@link Delegation#leave()}.
     * @see Delegation#leave()
     */
    Delegation leaveMatched(Visitable segment) {
        return Delegation.leave();
    }

    /**
     * {@link Visitor#leave(Visitable) Leave} callback for a nested {@link Visitable}. The default implementation retains
     * delegation by default.
     *
     * @param segment the segment, must not be {@literal null}.
     * @return delegation options. Can be either {@link Delegation#retain()} or {@link Delegation#leave()}.
     * @see Delegation#retain()
     */
    Delegation leaveNested(Visitable segment) {
        return Delegation.retain();
    }

    /*
     * (non-Javadoc)
     * @see DelegatingVisitor#doEnter(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    public final Delegation doEnter(Visitable segment) {

        if (currentSegment == null) {

            if (filter.test(segment)) {
                currentSegment = segment;
                return enterMatched(segment);
            }
        } else {
            return enterNested(segment);
        }

        return Delegation.retain();
    }

    /*
     * (non-Javadoc)
     * @see DelegatingVisitor#doLeave(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    public final Delegation doLeave(Visitable segment) {

        if (currentSegment == null) {
            return Delegation.leave();
        } else if (segment == currentSegment) {
            currentSegment = null;
            return leaveMatched(segment);
        } else {
            return leaveNested(segment);
        }
    }
}