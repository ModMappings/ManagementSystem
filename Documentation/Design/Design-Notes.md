### These are the design notes for the MCMS
They are combined from the original .NetCore version of the MCMS developed by OrionDevelopment, as well as the information collected on 3.1.2020 during a Discord session.
They might or might not be complete or even still correct at the point this is read.

#### Premise
During the end of 2019 LexManos requested a system that would replace the aging system of the MCPBot which became hard to maintain and update to the newer MC Versions, for reasons unknown to the writer at this point in time.
The goal of this system would be to provide a HTTP based backend that would communicate with a database that in the end would serve the mapping information that currently the MCPBot did.
Next to this primary goal more targets where added over time:
 * Web interfaces to create mappings and manage the system
 * Advanced search capabilities beyond what the current bots could offer
 * Support for proposals, instead of mappings that went directly live
 * A given amount of backwards compatibility that still allowed the use of bots
 * A synchronized authentication mechanism, that allowed users to register with accounts from:
   * Github
   * Discord
   * A local account system, for the paranoid.
 * Easy to maintain, it should run in docker.
 * It should not only keep track of MCP Mappings but different mappings as well, and prevent invalid proposals from being created.
 * It should mark a proposal invalid if any of the following conditions where met:
   * The new mapping was not a valid Java identifier.
   * The mapping was already in use by a different mapping system, which would potentially cause licence problems.
   * The mapping was simply of bad taste (Illegal word list, to filter out troll mappings) -> _This was moved to a later version_
 * Voting on proposals should be possible, merging the proposed mapping into the official list, should still be at the discretion of a select few with knowledge of the systems.
 * It should be possible to comment on proposals to state a users opinion.
 * Some systems should be auditable to be able to revert damage in case of problems.
 
#### New design:
##### DataLayer:
The system will track pieces of remappable source code as part of any project (regardless of MC or anything else) these pieces are called `Mappable`s and are additionally differentiated on a system version level, creating the `VersionedMappable`s.
Each of these `VersionedMappable`s has some metadata attached that represents the specific information that is relevant to a given `Mappable`. For example `Classes` have different metadata then `Methods`.
`VersionedMappables` also store the current `Mapping`s as well as all `ProposedMapping`s for that `Mappable` and `GameVersion`, additionally a given `VersionedMappable` can be protected against unwanted modifications (for example all mappings from obfuscated to srg names) by creating a `ProtectedMapping` instance and registering it.
Additionally all `ProposedMapping`s and `Release`s support comments.

##### BusinessLayer:
The job of the business layer is to consume the data coming from the _ViewLayer_, and then validate it and pass it on to the _DataLayer_ where it is stored in the Database.
This layer will need to perform the validations which are listed above in the _Premise_ and will need to be implemented in custom code.
These validations should not be implemented in the Controllers of the _ViewLayer_ for separation of concern reasons.

##### ViewLayer:
The ViewLayer will function as a hollow shell around the _BusinessLayer_ only cleaning up the incoming and outgoing data, plus performing basic authentication against the users identification.
Its primary job will be to provide a transport mechanism with which the clients communicate and should provide tools that will allow simple and easy generation of clients. Examples here are:
 * OpenAPI / Swagger definitions -> Probably better, more spring tools available.
 * OData queryables. -> OData, although a public standard and implemented by Apache in Java, is not really suited for use in Spring.
 
Which ever is chosen should be used throughout.

#### Authentication:
OpenID and OAuth2 with JWT Period. Full Stop.
We need several Flows to support different systems, see the Whitepapers on Authentication that OrionDevelopment wrote for the .Net Core implementation.
Here are several good options available, after discussion, KeyGuard was chosen for its out of the box functionality.

At least three clients will need to be specified:
 1) The WebClient (Using PKCE Authorization Code flow.)
 2) The Discord bot (Using device flow so it can simply use chat mechanics, much simpler to implement on the bot end)
 3) The IRC bot (Using device flow again much simpler chat mechanics)
 4) Its own admin client -> _This is still optional i am guessing it would be nice to have one, but possibly KeyGuard provides one that is to our liking, it at least provides a rest api so that is a start)

Additionally it will also need protect at least two ApiResources:
 1) The MCMS api.
 2) The KeyGuard admin api.
 
On top of this several IdentityResources will need to be added, which represent the access rights each user has or does not have:
 1) Can Propose -> List of mapping types comma separated -> Indicates if a user is allowed to create a proposal for a given mapping type.
 2) Can Vote -> List of mapping types comma separated -> Indicates if a user is allowed to vote for or against a given proposal if said proposal is not public for a given mapping type.
 3) Can Merge Or Reject -> List of mapping types comma separated -> Indicates if a user is allowed to merge or reject a proposal if it has a positive amount of votes.
 4) Can Override Voting -> List of mapping types comma separated -> Indicates that this user can merge a given proposal even if the voting result is negative for a given mapping type.
 5) Can Create Immediately -> List of mapping types comma separated -> Indicates that this user can immediately create a mapping regardless of it being a proposal first, for each given mapping type.
 6) Can Remove Mapping -> List of mapping types comma separated -> Allows the removal of an already published mapping, for each mapping type.
 7) Can Create GameVersion -> Boolean -> Indicates if this user is allowed to create a GameVersion.
 8) Can Create Release -> List of mapping types comma separated -> Allows the creation of a new Release for each mapping type
 9) Can Create MappingType -> Boolean -> Indicates if this user is allowed to create a new MappingType
 10) Can Modify GameVersion -> Boolean -> Indicates if this user is allowed to modify an existing GameVersion.
 11) Can Delete GameVersion -> Boolean -> Indicates if this user is allowed to delete an existing GameVersion.
 12) Can Modify Release -> Boolean -> Indicates if this user is allowed to modify existing releases.
 13) Can Delete Release -> Boolean -> Indicates if this user is allowed to delete existing releases.
 15) Can Modify MappingType -> Boolean -> Indicates if this user is allowed to modify an existing MappingType.
 16) Can Delete MappingType -> Boolean -> Indicates if this user is allowed to delete an existing MappingType.
 17) Can Import -> List of mapping types comma separated -> Indicates if this user is allowed to import an already existing release for a given mapping type. Requires rights to create a GameVersion, MappingType and Release.

Some of these can obviously be merged together (like the Create, Modify, Delete ones) but they are listed here for completeness.
#### Build system:
Jenkins from Forge

#### Project management:
Gradle, multiproject

##### Modularity:
We should provide as much modularity as possibly, this will be a bit higher of an effort to create, but making interfaces and proper API surfaces should be straight forward so use the IDE integrated functions todo so.

##### Testing:
With more people working on this, tests are a welcome addition.
Especially to the _BusinessLayer_.
Integration tests can come at a later state if need be.
