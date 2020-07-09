package org.modmappings.mmms.er2dbc.data.statements.expression;

import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;

/**
 * Represents the left or right hand aside of the criteria.
 */
public interface Expression {
    static Expression NULL = new Expression() {
        @Override
        public boolean isNull() {
            return true;
        }

        @Override
        public Expression dealias() {
            return this;
        }
    };

    default boolean isValue() { return false; }
    default boolean isDistinct() { return false; }
    default boolean isAliased() {return false;}
    default boolean isReference() { return false; }
    default boolean isNull() { return false; }
    default boolean isCollection() { return false; }
    default boolean isNative() { return false; }
    default boolean isFunction() { return false; }

    Expression dealias();
}
