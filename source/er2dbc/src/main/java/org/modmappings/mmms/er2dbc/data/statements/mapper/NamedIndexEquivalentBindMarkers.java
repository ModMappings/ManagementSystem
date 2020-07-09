package org.modmappings.mmms.er2dbc.data.statements.mapper;

import org.springframework.data.r2dbc.dialect.BindMarker;
import org.springframework.data.r2dbc.dialect.BindMarkers;

import java.util.HashMap;
import java.util.Map;

public class NamedIndexEquivalentBindMarkers implements BindMarkers {

    private final BindMarkers inner;
    private final Map<String, BindMarker> namedMarker = new HashMap<>();

    public NamedIndexEquivalentBindMarkers(final BindMarkers inner) {
        this.inner = inner;
    }

    @Override
    public BindMarker next() {
        return inner.next();
    }

    @Override
    public BindMarker next(final String hint) {
        if (hint.equals(""))
            return inner.next(hint);

        return this.namedMarker.computeIfAbsent(hint, inner::next);
    }
}
