using System;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Core;
using Data.WebApi.Model;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Http;

namespace Data.WebApi.Services.UserResolving
{
    public class AuthorizationBasedUserResolvingService
        : IUserResolvingService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationBasedUserResolvingService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> Get()
        {
            var claimsPrinciple = _httpContextAccessor.HttpContext.User;

            var userId = claimsPrinciple?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return null;

            var userName = claimsPrinciple.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null)
                return null;

            var user = new User()
            {
                Id = Guid.Parse(userId),
                Name = userName,
                CanCommit = claimsPrinciple.HasClaim("CanCommit", "true"),
                CanRelease = claimsPrinciple.HasClaim("CanRelease", "true"),
                CanEdit = claimsPrinciple.HasClaim("CanEdit", "true"),
                CanReview = claimsPrinciple.HasClaim("CanReview", "true"),
                CanCreateGameVersions = claimsPrinciple.HasClaim("CanCreateGameVersions", "true"),
                CanCreateMappingTypes = claimsPrinciple.HasClaim("CanCreateMappingTypes", "true"),
                CanLock = claimsPrinciple.HasClaim("CanLock", "true"),
                CanUnlock = claimsPrinciple.HasClaim("CanUnlock", "true")
            };

            return await Task.FromResult(user);
        }
    }
}
