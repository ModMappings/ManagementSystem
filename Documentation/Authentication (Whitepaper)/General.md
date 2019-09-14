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
| Authentication Flow | A particular way of having client and OpenID/OAuth2 server communicate with one another. | Every time a user logs into a client, a particular flow is used to exchange the user account information for tokens |
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
Any system that will provide the local authentication mechanisms should as such also provide this endpoint, or the authentication server will need to have local user accounts baked in.
   
