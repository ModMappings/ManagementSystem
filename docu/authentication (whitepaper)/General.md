### TLDR:
 - Use OAuth2 to provide a Github and Discord based authentication.
 - Use OpenID to authenticate users within our service
 - Create a custom OAuth2/OpenID authentication flow to handle clients and their user networks that do not support OAuth2
 - Create a custom 'merge user account' feature that allows the system to have 1 master account and several slaves.

### Introduction
A couple of days ago I published my initial proposal on a new system for a MCP WebService based data source.
This proposal, as with any initial proposal, did not yet meet the requirement for actual use, but (based on my humble opinion) was a good starting point.
The proposal kicked of a discussion on how to handle the authentication and authorization of interactions with the service made by users. Especially in a way where multiple sources of users (Discord, Github and for example IRC, as well as a Local user account management system) would be handled gracefully, with the up most care for privacy and security of both the user data as well as the MCP data.
My initial proposal included a design that used OAuth 2 and OpenID together with an OIDC server to handle these aspects.
This general description will talk in about the handling of these technologies within the stack, used for the MCP WebService.
### Terminology
| Term | Description | Use in WebService |
| ---- | :---------: | :---------------: |
| Client | A program that wants to access protected resources | Every program, web interface, bot, etc. that wants to pull or write data to the MCP WebService. |
| Token | A identifier for a single session of a user, or client | The JWT WebToken is used to identify which user makes which call to the service. |
| OAuth2 | Second iteration of the Open Authentication protocol, used to allow external system to authenticate users. | Used to get the account information for new users when they login via a supported platform (currently planned: Github and Discord). |
| OpenID | Open identification protocol, used to allow clients to verify which user is logged and retrieve tokens for the user. | Clients use OpenID to get exchange the user account information for tokens |
| Authentication Flow (Client Grant) | A particular way of having client and OpenID/OAuth2 server communicate with one another. | Every time a user logs into a client, a particular flow is used to exchange the user account information for tokens |
### Description
To not allow unknown or unwanted users to make changes to critical components of MCP (like releases or the history.) authentication and authorization is needed.
In general a role based system will allow for a very flexible way to handle the authorization problems in principle.
However to handle authentication the Forge team has expressed wishes to allow several sources of users to be able to use the system.
These include, but are possibly not limited too:
 - IRC
 - Discord
 - Github

Additionally some users seem to want the possibility to create an account for just the MCP WebService and not share the information across applications.
To accommodate for all these wishes a central authentication handling server will need to be added.
This server will as such need to handle the conversion between the external authentication systems as well as our own data.
To accomodate this, both Discord and Github provide an OAuth2 API which can be used to achieve this.
Any system that will provide the local authentication mechanisms should as such also provide this api, or the authentication server will need to have support for local user accounts baked in.
#### Additional problems with systems that do not provide an OAuth2 or OIDC Api.
As previously stated it should be possible that users log in from services other then once that provide OAuth2 or OIDC Apis, as for example with IRC.
IRC provides by design no identification for its users, however a service called NickServ allows for the verification of a users Username.
This server does also not provide any official web endpoints to verify, luckily OAuth2 and OpenID allow for the definition of additional authentication flows.
We can use this to allow specific clients to create tokens for users of their user network, when we can not verify this externally.
This of course opens up a problematic vulnerability, since this relies on the security of the client who is connecting and its user network, however under the circumstances I do not see this feature used more then in the IRC scenario.
Since any other more modern communications platform at least provides some level of authentication handling via the web it self.
#### Handling multiple accounts.
Since we are now able to accept users from multiple sources (via the sources user network) we need to consider not to duplicate the users across the service.
Users who log in via Github, Discord and/or IRC, might potentially have up to three different accounts that need to be managed, and should all have the same access rights.
Especially with the IRC case, we should consider that accounts (possibly with different usernames) need to be joined together into a single master accounts, who's rights are then loaded.
While the other joined accounts get hidden from the public, they should still be able to be used for logging in.
This kind of feature is not included directly into the OAuth2 or OpenID spec, and entirely depends on the implementation of the mechanics backing the authentication server, as such (in most cases) they will need to be implemented directly by us.
