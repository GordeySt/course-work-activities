using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistance;

namespace Infrastructure.Security
{
    public class IsHostRequirement : IAuthorizationRequirement
    {
    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;

        public IsHostRequirementHandler(IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            var currentUserName = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

            if (currentUserName == null) return Task.CompletedTask;

            var activityId = Guid.Parse(_httpContextAccessor.HttpContext.Request.RouteValues
                .SingleOrDefault(x => x.Key == "id").Value.ToString());

            var activity = _context.Activities.FindAsync(activityId).Result;

            if (activity == null) return Task.CompletedTask;

            var host = activity.UserActivities.FirstOrDefault(x => x.IsHost);

            if (host?.AppUser?.UserName == currentUserName) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}