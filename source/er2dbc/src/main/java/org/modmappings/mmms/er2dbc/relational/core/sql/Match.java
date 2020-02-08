package org.modmappings.mmms.er2dbc.relational.core.sql;

import org.springframework.data.relational.core.sql.Condition;
import org.springframework.data.relational.core.sql.Expression;
import org.springframework.data.relational.core.sql.Like;
import org.springframework.util.Assert;

public class Match extends AbstractSegment implements Condition {

    private final Expression target;
    private final Expression pattern;
    private final IMatchFormatter formatter;

    private Match(final Expression target, final Expression pattern, final IMatchFormatter formatter) {

        super(pattern, target);

        this.pattern = pattern;
        this.target = target;
        this.formatter = formatter;
    }

    /**
     * Creates a new {@link Like} {@link Condition} given two {@link Expression}s.
     *
     * @param target the right {@link Expression}.
     * @param pattern the left {@link Expression}.
     * @param formatter The match statement formatter to use.
     * @return the {@link Like} condition.
     */
    public static Match create(final Expression target, final Expression pattern, final IMatchFormatter formatter) {

        Assert.notNull(pattern, "Left expression must not be null!");
        Assert.notNull(target, "Right expression must not be null!");

        return new Match(pattern, target, formatter);
    }

    /**
     * @return the right {@link Expression}.
     */
    public Expression getTarget() {
        return target;
    }

    /**
     * @return the left {@link Expression}.
     */
    public Expression getPattern() {
        return pattern;
    }


    @Override
    public String toString() {
        return formatter.format(getPattern().toString(), getTarget().toString());
    }
}
