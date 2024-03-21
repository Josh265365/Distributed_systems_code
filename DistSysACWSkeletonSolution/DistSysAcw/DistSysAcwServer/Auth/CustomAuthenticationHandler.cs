using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace DistSysAcwServer.Auth
{
    /// <summary>
    /// Authenticates clients by API Key
    /// </summary>
    public class CustomAuthenticationHandler
        : AuthenticationHandler<AuthenticationSchemeOptions>, IAuthenticationHandler
    {
        private Models.UserContext DbContext { get; set; }
        private IHttpContextAccessor HttpContextAccessor { get; set; }

        public CustomAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            Models.UserContext dbContext,
            IHttpContextAccessor httpContextAccessor)
            : base(options, logger, encoder, clock) 
        {
            DbContext = dbContext;
            HttpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// this method is called when the server receives a request that requires authentication of the client to access the resource or endpoint
        /// </summary>
        /// <returns>access</returns>
        //protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        //{
        //    #region Task5
        //    // TODO:  Find if a header ‘ApiKey’ exists, and if it does, check the database to determine if the given API Key is valid
        //    //        Then create the correct Claims, add these to a ClaimsIdentity, create a ClaimsPrincipal from the identity 
        //    //        Then use the Principal to generate a new AuthenticationTicket to return a Success AuthenticateResult

        //    #endregion

        //    if (Request.Headers.ContainsKey("ApiKey"))
        //    {
        //        string apiKey = Request.Headers["ApiKey"];
        //        if (DbContext.Users.Any(u => u.ApiKey == apiKey))
        //        {

        //            var user = DbContext.Users.FirstOrDefault(u => u.ApiKey == apiKey);

        //            var claims = new[]
        //            {
        //                  new Claim(ClaimTypes.Name, user.UserName),
        //                  new Claim(ClaimTypes.Role, user.Role)
        //             };


        //            var identity = new ClaimsIdentity(claims, "ApiKey");
        //            var principal = new ClaimsPrincipal(identity);
        //            var ticket = new AuthenticationTicket(principal, "ApiKey");
        //            return Task.FromResult(AuthenticateResult.Success(ticket));
        //        }
        //        else
        //        {
        //            return Task.FromResult(AuthenticateResult.Fail("Unauthorized. Check ApiKey in Header is correct"));
        //        }

        //    }

        //    return Task.FromResult(AuthenticateResult.Fail("Not Authenticated"));
        //}
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var apiKey = Request.Headers["ApiKey"].FirstOrDefault();
            if (string.IsNullOrEmpty(apiKey))
            {
                return Task.FromResult(AuthenticateResult.Fail("Unauthorized. Check ApiKey in Header is correct"));
            }
            var user = DbContext.Users.FirstOrDefault(u => u.ApiKey == apiKey);
            if (user == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Not Authenticated"));
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, "ApiKey");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "ApiKey");

             return Task.FromResult(AuthenticateResult.Success(ticket));

        }


        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            byte[] messagebytes = Encoding.ASCII.GetBytes("Task 5 Incomplete");
            Context.Response.StatusCode = 401;
            Context.Response.ContentType = "application/json";
            await Context.Response.Body.WriteAsync(messagebytes, 0, messagebytes.Length);
            await HttpContextAccessor.HttpContext.Response.CompleteAsync();
        }
    }
}