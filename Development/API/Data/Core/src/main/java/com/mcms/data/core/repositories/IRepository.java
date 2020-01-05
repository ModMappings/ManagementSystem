package com.mcms.data.core.repositories;

import java.util.UUID;

import org.reactivestreams.Publisher;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

/**
 * This is a raw interface that declares how a repository should look and function.
 * It provides raw CRUD operations as well as saving support.
 *
 * It is largely equal to the ReactiveCrudRepository from Spring Reactive.
 * However we are free to add more methods or remove them if we see fit.
 *
 * Additionally this forces all our ids to be uuids.
 *
 * @param <T> The type that is stored in this repository.
 */
public interface IRepository<T> {
    /**
     * Saves a given entity. Use the returned instance for further operations as the save operation might have changed the
     * entity instance completely.
     *
     * @param entity must not be {@literal null}.
     * @return {@link Mono} emitting the saved entity.
     * @throws IllegalArgumentException in case the given {@literal entity} is {@literal null}.
     */
    <S extends T> Mono<S> save(S entity);

    /**
     * Saves all given entities.
     *
     * @param entities must not be {@literal null}.
     * @return {@link Flux} emitting the saved entities.
     * @throws IllegalArgumentException in case the given {@link Iterable entities} or one of its entities is
     *           {@literal null}.
     */
    <S extends T> Flux<S> saveAll(Iterable<S> entities);

    /**
     * Saves all given entities.
     *
     * @param entityStream must not be {@literal null}.
     * @return {@link Flux} emitting the saved entities.
     * @throws IllegalArgumentException in case the given {@link Publisher entityStream} is {@literal null}.
     */
    <S extends T> Flux<S> saveAll(Publisher<S> entityStream);

    /**
     * Retrieves an entity by its id.
     *
     * @param id must not be {@literal null}.
     * @return {@link Mono} emitting the entity with the given id or {@link Mono#empty()} if none found.
     * @throws IllegalArgumentException in case the given {@literal id} is {@literal null}.
     */
    Mono<T> findById(UUID id);

    /**
     * Retrieves an entity by its id supplied by a {@link Publisher}.
     *
     * @param id must not be {@literal null}. Uses the first emitted element to perform the find-query.
     * @return {@link Mono} emitting the entity with the given id or {@link Mono#empty()} if none found.
     * @throws IllegalArgumentException in case the given {@link Publisher id} is {@literal null}.
     */
    Mono<T> findById(Publisher<UUID> id);

    /**
     * Returns whether an entity with the given {@literal id} exists.
     *
     * @param id must not be {@literal null}.
     * @return {@link Mono} emitting {@literal true} if an entity with the given id exists, {@literal false} otherwise.
     * @throws IllegalArgumentException in case the given {@literal id} is {@literal null}.
     */
    Mono<Boolean> existsById(UUID id);

    /**
     * Returns whether an entity with the given id, supplied by a {@link Publisher}, exists. Uses the first emitted
     * element to perform the exists-query.
     *
     * @param id must not be {@literal null}.
     * @return {@link Mono} emitting {@literal true} if an entity with the given id exists, {@literal false} otherwise.
     * @throws IllegalArgumentException in case the given {@link Publisher id} is {@literal null}.
     */
    Mono<Boolean> existsById(Publisher<UUID> id);

    /**
     * Returns all instances of the type.
     *
     * @return {@link Flux} emitting all entities.
     */
    Flux<T> findAll();

    /**
     * Returns all instances of the type {@code T} with the given UUIDs.
     * <p>
     * If some or all ids are not found, no entities are returned for these UUIDs.
     * <p>
     * Note that the order of elements in the result is not guaranteed.
     *
     * @param ids must not be {@literal null} nor contain any {@literal null} values.
     * @return {@link Flux} emitting the found entities. The size can be equal or less than the number of given
     *         {@literal ids}.
     * @throws IllegalArgumentException in case the given {@link Iterable ids} or one of its items is {@literal null}.
     */
    Flux<T> findAllById(Iterable<UUID> ids);

    /**
     * Returns all instances of the type {@code T} with the given UUIDs supplied by a {@link Publisher}.
     * <p>
     * If some or all ids are not found, no entities are returned for these UUIDs.
     * <p>
     * Note that the order of elements in the result is not guaranteed.
     *
     * @param idStream must not be {@literal null}.
     * @return {@link Flux} emitting the found entities.
     * @throws IllegalArgumentException in case the given {@link Publisher idStream} is {@literal null}.
     */
    Flux<T> findAllById(Publisher<UUID> idStream);

    /**
     * Returns the number of entities available.
     *
     * @return {@link Mono} emitting the number of entities.
     */
    Mono<Long> count();
    
    /**
     * Deletes the entity with the given id.
     *
     * @param id must not be {@literal null}.
     * @return {@link Mono} signaling when operation has completed.
     * @throws IllegalArgumentException in case the given {@literal id} is {@literal null}.
     */
    Mono<Void> deleteById(UUID id);

    /**
     * Deletes the entity with the given id supplied by a {@link Publisher}.
     *
     * @param id must not be {@literal null}.
     * @return {@link Mono} signaling when operation has completed.
     * @throws IllegalArgumentException in case the given {@link Publisher id} is {@literal null}.
     */
    Mono<Void> deleteById(Publisher<UUID> id);

    /**
     * Deletes a given entity.
     *
     * @param entity must not be {@literal null}.
     * @return {@link Mono} signaling when operation has completed.
     * @throws IllegalArgumentException in case the given entity is {@literal null}.
     */
    Mono<Void> delete(T entity);

    /**
     * Deletes the given entities.
     *
     * @param entities must not be {@literal null}.
     * @return {@link Mono} signaling when operation has completed.
     * @throws IllegalArgumentException in case the given {@link Iterable entities} or one of its entities is
     *           {@literal null}.
     */
    Mono<Void> deleteAll(Iterable<? extends T> entities);

    /**
     * Deletes the given entities supplied by a {@link Publisher}.
     *
     * @param entityStream must not be {@literal null}.
     * @return {@link Mono} signaling when operation has completed.
     * @throws IllegalArgumentException in case the given {@link Publisher entityStream} is {@literal null}.
     */
    Mono<Void> deleteAll(Publisher<? extends T> entityStream);

    /**
     * Deletes all entities managed by the repository.
     *
     * @return {@link Mono} signaling when operation has completed.
     */
    Mono<Void> deleteAll();
}
