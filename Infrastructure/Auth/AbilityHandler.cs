using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApi_Coris.DataContext;

namespace WebApi_Coris.Infrastructure.Auth
{
    public class AbilityHandler : AuthorizationHandler<AbilityRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _db;

        public AbilityHandler(IHttpContextAccessor httpContextAccessor, ApplicationDbContext db)
        {
            _httpContextAccessor = httpContextAccessor;
            _db = db;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AbilityRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var authHeader = httpContext?
                .Request.Headers["Authorization"]
                .FirstOrDefault();

            // Se vier nulo ou vazio, já falha a autorização
            if (string.IsNullOrEmpty(authHeader))
            {
                context.Fail();
                return;
            }

            var parts = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2 || !parts[0].Equals("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                context.Fail();
                return;
            }

            var token = parts[1].Trim();

            var abilitiesJson = await _db.PersonalAccessTokens
                .Where(p => p.token == token)
                .Select(p => p.abilities)
                .SingleOrDefaultAsync();

                        if (!string.IsNullOrEmpty(abilitiesJson))
                        {
                            var abilities = JsonSerializer.Deserialize<List<string>>(abilitiesJson);
                            if (abilities != null && abilities.Contains(requirement.RequiredAbility))
                            {
                                context.Succeed(requirement);
                                return;
                            }
                        }

            context.Fail();
        }
    }
}
