package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.springframework.core.ResolvableType;
import org.springframework.data.relational.core.sql.Visitable;
import org.springframework.data.relational.core.sql.Visitor;
import org.springframework.lang.Nullable;

public class TypedSubtreeVisitor<T extends Visitable> extends DelegatingVisitor {

    private final ResolvableType type;
    private @Nullable
    Visitable currentSegment;

    /**
     * Creates a new {@link TypedSubtreeVisitor}.
     */
    TypedSubtreeVisitor() {
        this.type = ResolvableType.forClass(getClass()).as(TypedSubtreeVisitor.class).getGeneric(0);
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
    Delegation enterMatched(final T segment) {
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
    Delegation enterNested(final Visitable segment) {
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
    Delegation leaveMatched(final T segment) {
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
    Delegation leaveNested(final Visitable segment) {
        return Delegation.retain();
    }

    /*
     * (non-Javadoc)
     * @see DelegatingVisitor#doEnter(org.springframework.data.relational.core.sql.Visitable)
     */
    @SuppressWarnings("unchecked")
    @Override
    public final Delegation doEnter(final Visitable segment) {

        if (currentSegment == null) {

            if (this.type.isInstance(segment)) {

                currentSegment = segment;
                return enterMatched((T) segment);
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
    @SuppressWarnings("unchecked")
    @Override
    public final Delegation doLeave(final Visitable segment) {

        if (currentSegment == null) {
            return Delegation.leave();
        } else if (segment == currentSegment) {
            currentSegment = null;
            return leaveMatched((T) segment);
        } else {
            return leaveNested(segment);
        }
    }
}