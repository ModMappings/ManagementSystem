package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.springframework.data.relational.core.sql.Visitable;
import org.springframework.data.relational.core.sql.Visitor;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;

import java.util.Stack;

public abstract class DelegatingVisitor implements Visitor {
    private Stack<DelegatingVisitor> delegation = new Stack<>();

    /**
     * Invoked for a {@link Visitable segment} when entering the segment.
     * <p/>
     * This method can signal whether it is responsible for handling the {@link Visitor segment} or whether the segment
     * requires delegation to a sub-{@link Visitor}. When delegating to a sub-{@link Visitor}, {@link #doEnter(Visitable)}
     * is called on the {@link DelegatingVisitor delegate}.
     *
     * @param segment must not be {@literal null}.
     * @return
     */
    @Nullable
    public abstract DelegatingVisitor.Delegation doEnter(Visitable segment);

    /*
     * (non-Javadoc)
     * @see org.springframework.data.relational.core.sql.Visitor#enter(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    public final void enter(Visitable segment) {

        if (delegation.isEmpty()) {

            DelegatingVisitor.Delegation visitor = doEnter(segment);
            Assert.notNull(visitor,
                    () -> String.format("Visitor must not be null. Caused by %s.doEnter(…)", getClass().getName()));
            Assert.state(!visitor.isLeave(),
                    () -> String.format("Delegation indicates leave. Caused by %s.doEnter(…)", getClass().getName()));

            if (visitor.isDelegate()) {
                delegation.push(visitor.getDelegate());
                visitor.getDelegate().enter(segment);
            }
        } else {
            delegation.peek().enter(segment);
        }
    }

    /**
     * Invoked for a {@link Visitable segment} when leaving the segment.
     * <p/>
     * This method can signal whether this {@link Visitor} should remain responsible for handling subsequent
     * {@link Visitor segments} or whether it should step back from delegation. When stepping back from delegation,
     * {@link #doLeave(Visitable)} is called on the {@link DelegatingVisitor parent delegate}.
     *
     * @param segment must not be {@literal null}.
     * @return
     */
    public abstract DelegatingVisitor.Delegation doLeave(Visitable segment);

    /*
     * (non-Javadoc)
     * @see org.springframework.data.relational.core.sql.Visitor#leave(org.springframework.data.relational.core.sql.Visitable)
     */
    public final void leave(Visitable segment) {
        doLeave0(segment);
    }

    private DelegatingVisitor.Delegation doLeave0(Visitable segment) {

        if (delegation.isEmpty()) {
            return doLeave(segment);
        } else {

            DelegatingVisitor visitor = delegation.peek();
            while (visitor != null) {

                DelegatingVisitor.Delegation result = visitor.doLeave0(segment);
                Assert.notNull(visitor,
                        () -> String.format("Visitor must not be null. Caused by %s.doLeave(…)", getClass().getName()));

                if (visitor == this) {
                    if (result.isLeave()) {
                        return delegation.isEmpty() ? DelegatingVisitor.Delegation.leave() : DelegatingVisitor.Delegation.retain();
                    }
                    return DelegatingVisitor.Delegation.retain();
                }

                if (result.isRetain()) {
                    return result;
                }

                if (result.isLeave()) {

                    if (!delegation.isEmpty()) {
                        delegation.pop();
                    }

                    if (!delegation.isEmpty()) {
                        visitor = delegation.peek();
                    } else {
                        visitor = this;
                    }
                }
            }
        }

        return DelegatingVisitor.Delegation.leave();
    }

    /**
     * Value object to control delegation.
     */
    static class Delegation {

        private static DelegatingVisitor.Delegation RETAIN = new DelegatingVisitor.Delegation(true, false, null);
        private static DelegatingVisitor.Delegation LEAVE = new DelegatingVisitor.Delegation(false, true, null);

        private final boolean retain;
        private final boolean leave;

        private final @Nullable
        DelegatingVisitor delegate;

        private Delegation(boolean retain, boolean leave, @Nullable DelegatingVisitor delegate) {
            this.retain = retain;
            this.leave = leave;
            this.delegate = delegate;
        }

        public static DelegatingVisitor.Delegation retain() {
            return RETAIN;
        }

        public static DelegatingVisitor.Delegation leave() {
            return LEAVE;
        }

        public static DelegatingVisitor.Delegation delegateTo(DelegatingVisitor visitor) {
            return new DelegatingVisitor.Delegation(false, false, visitor);
        }

        boolean isDelegate() {
            return delegate != null;
        }

        boolean isRetain() {
            return retain;
        }

        boolean isLeave() {
            return leave;
        }

        DelegatingVisitor getDelegate() {

            Assert.state(isDelegate(), "No delegate available");
            return delegate;
        }
    }
}
