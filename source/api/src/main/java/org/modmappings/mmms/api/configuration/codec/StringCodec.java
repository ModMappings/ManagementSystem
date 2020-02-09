package org.modmappings.mmms.api.configuration.codec;

import io.netty.buffer.ByteBuf;
import io.netty.buffer.ByteBufAllocator;
import io.r2dbc.postgresql.client.Parameter;
import io.r2dbc.postgresql.message.Format;
import io.r2dbc.postgresql.type.PostgresqlObjectId;
import io.r2dbc.postgresql.util.Assert;
import io.r2dbc.postgresql.util.ByteBufUtils;
import org.springframework.lang.Nullable;

import static io.r2dbc.postgresql.message.Format.FORMAT_TEXT;
import static io.r2dbc.postgresql.type.PostgresqlObjectId.*;

public class StringCodec extends AbstractCodec<String> {

    private final ByteBufAllocator byteBufAllocator;

    StringCodec(ByteBufAllocator byteBufAllocator) {
        super(String.class);
        this.byteBufAllocator = Assert.requireNonNull(byteBufAllocator, "byteBufAllocator must not be null");
    }

    @Override
    public Parameter encodeNull() {
        return createNull(VARCHAR, FORMAT_TEXT);
    }

    @Override
    public boolean doCanDecode(PostgresqlObjectId type, Format format) {
        Assert.requireNonNull(format, "format must not be null");
        Assert.requireNonNull(type, "type must not be null");

        return BPCHAR == type || CHAR == type || TEXT == type || UNKNOWN == type || VARCHAR == type;
    }

    @Override
    public String doDecode(ByteBuf buffer, PostgresqlObjectId dataType, @Nullable Format format, @Nullable Class<? extends String> type) {
        Assert.requireNonNull(buffer, "byteBuf must not be null");

        return ByteBufUtils.decode(buffer);
    }

    @Override
    public Parameter doEncode(String value) {
        Assert.requireNonNull(value, "value must not be null");

        return create(VARCHAR, FORMAT_TEXT, () -> ByteBufUtils.encode(this.byteBufAllocator, value));
    }

}

