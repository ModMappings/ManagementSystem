package org.modmappings.mmms.er2dbc.relational.postgres.codec;

import io.netty.buffer.ByteBufAllocator;
import io.r2dbc.postgresql.api.PostgresqlConnection;
import io.r2dbc.postgresql.codec.CodecRegistry;
import io.r2dbc.postgresql.extension.CodecRegistrar;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.reactivestreams.Publisher;
import reactor.core.publisher.Mono;

public class ER2DBCCodecRegistrar implements CodecRegistrar {

    private static final Logger LOGGER = LogManager.getLogger();

    @Override
    public Publisher<Void> register(final PostgresqlConnection postgresqlConnection, final ByteBufAllocator byteBufAllocator, final CodecRegistry codecRegistry) {
        codecRegistry.addFirst(new EnumCodec(byteBufAllocator));
        return Mono.empty();
    }
}
