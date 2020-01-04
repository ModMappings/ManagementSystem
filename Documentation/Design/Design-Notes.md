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
 * OpenAPI / Swagger definitions.
 * OData queryables.
 
Which ever is chosen should be used throughout.

#### Autentication:
OpenID and OAuth2 with JWT Period. Full Stop.
We need several Flows to support different systems, see the Whitepapers on Authentication that OrionDevelopment wrote for the .Net Core implementation.
Here are several good options available, some are:
 * Keyguard -> Out of the box solution but not perfect, boxed up, not open source.
 * MITREiD -> Minor bootstrapping needed, reference implementation. (Would be equivalent to IdentityServer 4 for .Net Core)
The link to the Certified list: [List](https://openid.net/developers/certified/)

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
