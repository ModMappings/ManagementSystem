### Introduction
As with the general purpose white-paper on how a MCP WebService could handle authentication and authorization, while keeping several user sources and client types into account.
A short white-paper used to discuss how this could be implemented in reality is also needed, this is what this paper provides.
As this project proposal is based on .Net Core and C#, implementing user account management and authentication should, in the humble opinion of the writer, happen in a same fashion.
As such I propose to use the open source IdentityServer4 OIDC Server, as it implements OAuth2 and OpenID in a verified manner.

#### Documentation
The documentation for IdentityServer4 can be found [here](http://docs.identityserver.io/en/latest/index.html).

### Changes required
As the general white-paper proposes an additional grant for using with clients that do not provide direct user authentication is needed.
This can be done via the extension grants principle, access control to this endpoint is then managed via servers client definition list, and as such
allows for easy retrieval of tokens and their data.
To handle the merging of user accounts, a email based random token can be used.
The user would log in into the master account, click the merge accounts button, get a randomly generated value, that is valid for say a day, log out and in into the other account and enter the token their.
Once complete this would allow the user to now have one or possible multiple merge accounts.
As again with IRC, as this is a special case, and possibly with discord as well, merging this should possibly happen directly from the client as well, so an endpoint should be provided.
