using Microsoft.AspNetCore.Authorization;

namespace WebApi_Coris.Infrastructure.Auth
{
    public class AbilityRequirement : IAuthorizationRequirement
    {
        public string RequiredAbility { get; }
        public AbilityRequirement(string requiredAbility) => RequiredAbility = requiredAbility;
    }
}
