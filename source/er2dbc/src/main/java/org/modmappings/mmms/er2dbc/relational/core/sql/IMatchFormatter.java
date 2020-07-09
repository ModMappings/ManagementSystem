package org.modmappings.mmms.er2dbc.relational.core.sql;

/**
 * Allows for the formatting of the match statement in regex form based on the platform.
 */
@FunctionalInterface
public interface IMatchFormatter {

    /**
     * Formats the match expression using the pattern and the target.
     *
     * @param target  The target to match against the pattern.
     * @param pattern The pattern to match the target against.
     * @return The formatted match sql statement.
     */
    String format(String target, String pattern);
}
