using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace DistSysAcwServer.Auth
{
    /// <summary>
    /// Authorises clients by role
    /// </summary>
    public class CustomAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
    {
        private IHttpContextAccessor HttpContextAccessor { get; set; }

        public CustomAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
        {
            #region Task6
            // TODO:  Modify the server's behaviour so that, when the action requires a user to be in Admin role ONLY 
            // (e.g. [Authorize(Roles = "Admin")]) and the user does not have the Admin role, you return a Forbidden status (403) 
            // with the message: "Forbidden. Admin access only."
            #endregion
            var httpContext = HttpContextAccessor.HttpContext;

            if (httpContext.User != null && httpContext.User.Identity.IsAuthenticated)
            {
                foreach (string role in requirement.AllowedRoles)
                {
                    if (httpContext.User.IsInRole(role))
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                }
            }
            
            //context.Fail();

            //return Task.CompletedTask;

            httpContext.Response.StatusCode = 403; // code to Forbidden
            httpContext.Response.ContentType = "text/plain";
            return httpContext.Response.WriteAsync("Forbidden. Admin access only.");
        }
    }
}