package com.mcms.data.core.repositories.mapping.mapping;

import java.util.UUID;

import com.mcms.api.datamodel.mapping.mappings.MappingDMO;
import com.mcms.data.core.repositories.IRepository;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

public interface IMappingRepository extends IRepository<MappingDMO> {

    /**
     * Finds all mappings for a given mappable in game version id.
     *
     * The mappings will be returned from newest to oldest.
     *
     * @param mappableInGameVersionId The id of the mappable in game version to get the mappings for.
     * @return The mappings for the given mappable in game version.
     */
    Flux<MappingDMO> findAllForMappableInGameVersion(UUID mappableInGameVersionId);

    /**
     * Finds the latest mappings for a given mappable in game version id.
     *
     * @param mappableInGameVersionId The id of the mappable in game version to get the latest mapping for.
     * @return The latest mapping for the given mappable in game version.
     */
    Mono<MappingDMO> findLatestForMappableInGameVersion(UUID mappableInGameVersionId);

    /**
     * Finds all mappings for a given mapping type id.
     *
     * The mappings will be returned from newest to oldest.
     *
     * @param mappingTypeId The id of the mapping type to get the mappings for.
     * @return The mappings for the given mapping type.
     */
    Flux<MappingDMO> findAllForMappingType(UUID mappingTypeId);

    /**
     * Finds all mappings for a given mapping type id and game version.
     *
     * The mappings will be returned from newest to oldest.
     *
     * @param mappingTypeId The id of the mapping type to get the mappings for.
     * @param gameVersionId The id of the game version that the mappings need to be for.
     * @return The mappings for the given mapping type and game version.
     */
    Flux<MappingDMO> findAllForMappingTypeAndGameVersion(UUID mappingTypeId, UUID gameVersionId);

    /**
     * Finds the latest mappings for a given mapping type id.
     * This will, in particular, find all the mappings for every mappable that exist, and grab their latest.
     * Which is in contrast too {@link #findAllLatestForMappingType(UUID)} which just finds all regardless of the fact
     * that there then might be multiple mappings for a given mappable.
     *
     * The mappings will be returned from newest to oldest.
     *
     * @param mappingTypeId The id of the mapping type to get the latest mapping for.
     * @return The latest mappings for the given mapping type.
     */
    Flux<MappingDMO> findAllLatestForMappingType(UUID mappingTypeId);

    /**
     * Finds the latest mappings for a given mapping type id which are part of a given game version.
     *
     * This will, in particular, find all the mappings for every mappable that exist, inside of the given game version, and grab their latest.
     * Which is in contrast too {@link #findAllLatestForMappingType(UUID)} which just finds all regardless of the fact
     * that there then might be multiple mappings for a given mappable.
     *
     * The mappings will be returned from newest to oldest.
     *
     * @param mappingTypeId The id of the mapping type to get the latest mapping for.
     * @param gameVersionId The id of the game version to get the latest mappings for.
     * @return The latest mappings for the given mapping type.
     */
    Flux<MappingDMO> findAllLatestForMappingTypeAndGameVersion(UUID mappingTypeId, UUID gameVersionId);

    /**
     * Finds all mappings of which the input matches the given regex.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @return All mappings who's input matches the given regex.
     */
    Flux<MappingDMO> findAllForInputRegex(String inputRegex);

    /**
     * Finds all mappings of which the input matches the given regex.
     * Will only return the latest mapping for any given mappable in game version.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @return All mappings who's input matches the given regex.
     */
    Flux<MappingDMO> findLatestForInputRegex(String inputRegex);

    /**
     * Finds all mappings of which the input matches the given regex.
     * The mapping also has to be for a mappable in game version who targets the game version with
     * the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @param gameVersionId The id of the game version that the mappings mappable in a game version has to target.
     * @return All mappings who's input matches the given regex.
     */
    Flux<MappingDMO> findAllForInputRegexAndGameVersion(String inputRegex, UUID gameVersionId);

    /**
     * Finds all mappings of which the input matches the given regex.
     * Will only return the latest mapping for any given mappable in game version.
     * The mapping also has to be for a mappable in game version who targets the game version with
     * the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @param gameVersionId The id of the game version that the mappings mappable in a game version has to target.
     * @return All mappings who's input matches the given regex.
     */
    Flux<MappingDMO> findLatestForInputRegexAndGameVersion(String inputRegex, UUID gameVersionId);

    /**
     * Finds all mappings of which the input matches the given regex.
     * The mapping also has to be for a mapping type with the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @return All mappings who's input matches the given regex.
     */
    Flux<MappingDMO> findAllForInputRegexAndMappingType(String inputRegex, UUID mappingTypeId);

    /**
     * Finds all mappings of which the input matches the given regex.
     * Will only return the latest mapping for any given mappable in game version.
     * The mapping also has to be for a mapping type with the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @return All mappings who's input matches the given regex.
     */
    Flux<MappingDMO> findLatestForInputRegexAndMappingType(String inputRegex, UUID mappingTypeId);

    /**
     * Finds all mappings of which the input matches the given regex.
     * The mapping also has to be for a mapping type with the given id.
     * The mapping also has to be for a mappable in game version who targets the game version with
     * the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @return All mappings who's input matches the given regex.
     */
    Flux<MappingDMO> findAllForInputRegexAndMappingTypeAndGameVersion(String inputRegex, UUID mappingTypeId, UUID gameVersionId);

    /**
     * Finds all mappings of which the input matches the given regex.
     * Will only return the latest mapping for any given mappable in game version.
     * The mapping also has to be for a mapping type with the given id.
     * The mapping also has to be for a mappable in game version who targets the game version with
     * the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @return All mappings who's input matches the given regex.
     */
    Flux<MappingDMO> findLatestForInputRegexAndMappingTypeAndGameVersion(String inputRegex, UUID mappingTypeId, UUID gameVersionId);

    /**
     * Finds all mappings of which the output matches the given regex.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param outputRegex The regex against which the output of the mappings is matched to be included in the result.
     * @return All mappings who's output matches the given regex.
     */
    Flux<MappingDMO> findAllForOutputRegex(String outputRegex);

    /**
     * Finds all mappings of which the output matches the given regex.
     * Will only return the latest mapping for any given mappable in game version.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param outputRegex The regex against which the output of the mappings is matched to be included in the result.
     * @return All mappings who's output matches the given regex.
     */
    Flux<MappingDMO> findLatestForOutputRegex(String outputRegex);

    /**
     * Finds all mappings of which the output matches the given regex.
     * The mapping also has to be for a mappable in game version who targets the game version with
     * the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param outputRegex The regex against which the output of the mappings is matched to be included in the result.
     * @param gameVersionId The id of the game version that the mappings mappable in a game version has to target.
     * @return All mappings who's output matches the given regex.
     */
    Flux<MappingDMO> findAllForOutputRegexAndGameVersion(String outputRegex, UUID gameVersionId);

    /**
     * Finds all mappings of which the output matches the given regex.
     * Will only return the latest mapping for any given mappable in game version.
     * The mapping also has to be for a mappable in game version who targets the game version with
     * the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param outputRegex The regex against which the output of the mappings is matched to be included in the result.
     * @param gameVersionId The id of the game version that the mappings mappable in a game version has to target.
     * @return All mappings who's output matches the given regex.
     */
    Flux<MappingDMO> findLatestForOutputRegexAndGameVersion(String outputRegex, UUID gameVersionId);

    /**
     * Finds all mappings of which the output matches the given regex.
     * The mapping also has to be for a mapping type with the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param outputRegex The regex against which the output of the mappings is matched to be included in the result.
     * @return All mappings who's output matches the given regex.
     */
    Flux<MappingDMO> findAllForOutputRegexAndMappingType(String outputRegex, UUID mappingTypeId);

    /**
     * Finds all mappings of which the output matches the given regex.
     * Will only return the latest mapping for any given mappable in game version.
     * The mapping also has to be for a mapping type with the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param outputRegex The regex against which the output of the mappings is matched to be included in the result.
     * @return All mappings who's output matches the given regex.
     */
    Flux<MappingDMO> findLatestForOutputRegexAndMappingType(String outputRegex, UUID mappingTypeId);

    /**
     * Finds all mappings of which the output matches the given regex.
     * The mapping also has to be for a mapping type with the given id.
     * The mapping also has to be for a mappable in game version who targets the game version with
     * the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param outputRegex The regex against which the output of the mappings is matched to be included in the result.
     * @return All mappings who's output matches the given regex.
     */
    Flux<MappingDMO> findAllForOutputRegexAndMappingTypeAndGameVersion(String outputRegex, UUID mappingTypeId, UUID gameVersionId);

    /**
     * Finds all mappings of which the output matches the given regex.
     * Will only return the latest mapping for any given mappable in game version.
     * The mapping also has to be for a mapping type with the given id.
     * The mapping also has to be for a mappable in game version who targets the game version with
     * the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param outputRegex The regex against which the output of the mappings is matched to be included in the result.
     * @return All mappings who's output matches the given regex.
     */
    Flux<MappingDMO> findLatestForOutputRegexAndMappingTypeAndGameVersion(String outputRegex, UUID mappingTypeId, UUID gameVersionId);


}
