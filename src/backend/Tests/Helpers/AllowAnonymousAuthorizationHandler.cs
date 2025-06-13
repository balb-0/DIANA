using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Helpers
{
    // Handler que permite tudo
    public class AllowAnonymousAuthorizationHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (var req in context.PendingRequirements.ToList())
                context.Succeed(req);
            return Task.CompletedTask;
        }
    }
}
