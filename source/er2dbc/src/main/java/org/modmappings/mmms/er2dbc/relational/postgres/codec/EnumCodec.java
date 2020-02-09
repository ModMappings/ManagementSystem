package org.modmappings.mmms.er2dbc.relational.postgres.codec;

import io.netty.buffer.ByteBuf;
import io.netty.buffer.ByteBufAllocator;
import io.r2dbc.postgresql.client.Parameter;
import io.r2dbc.postgresql.codec.Codec;
import io.r2dbc.postgresql.message.Format;
import io.r2dbc.postgresql.type.PostgresqlObjectId;
import io.r2dbc.postgresql.util.Assert;

public class EnumCodec implements Codec<Enum> {

    private final StringCodec delegate;

    public EnumCodec(ByteBufAllocator byteBufAllocator) {
        Assert.requireNonNull(byteBufAllocator, "byteBufAllocator must not be null");
        this.delegate = new StringCodec(byteBufAllocator);
    }

    @Override
    public boolean canDecode(int dataType, Format format, Class<?> type) {
        Assert.requireNonNull(format, "format must not be null");
        Assert.requireNonNull(type, "type must not be null");
        return Enum.class.isAssignableFrom(type) && this.delegate.doCanDecode(PostgresqlObjectId.valueOf(dataType), format);
    }

    @Override
    public boolean canEncode(Object value) {
        Assert.requireNonNull(value, "value must not be null");
        return value instanceof Enum;
    }

    @Override
    public boolean canEncodeNull(Class<?> type) {
        Assert.requireNonNull(type, "type must not be null");
        return Enum.class.isAssignableFrom(type);
    }

    @Override
    public Enum decode(final ByteBuf buffer, final int dataType, final Format format, final Class<? extends Enum> type) {
        Assert.requireNonNull(format, "format must not be null");
        Assert.requireNonNull(type, "type must not be null");
        if (buffer == null) {
            return null;
        }
        return Enum.valueOf(type, this.delegate.doDecode(buffer, PostgresqlObjectId.valueOf(dataType), format, String.class).trim());
    }

    @Override
    public Parameter encode(Object value) {
        Assert.requireNonNull(value, "value must not be null");
        return this.delegate.encode(((Enum) value).name());
    }

    @Override
    public Parameter encodeNull() {
        return this.delegate.encodeNull();
    }

    @Override
    public Class<?> type() {
        return Enum.class;
    }
}
