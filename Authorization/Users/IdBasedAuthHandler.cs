namespace GraphQL.AspNet.Examples.Authorization.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Net.Http.Headers;

    public class IdBasedAuthHandler : AuthenticationHandler<IdBasedAuthOptions>
    {
        private const string USERNAME_CLAIM_TYPE = "idAuth-username";
        private const string ROLES_CLAIM_TYPE = "idAuth-roles";
        private IUserStore<AppUser> _userStore;
        private readonly IOptionsMonitor<IdBasedAuthOptions> _options;

        public IdBasedAuthHandler(
            IUserStore<AppUser> userStore,
            IOptionsMonitor<IdBasedAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _userStore = userStore;
            _options = options;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Get Authorization header value
            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorization))
            {
                return AuthenticateResult.Fail("No Authorization Header Provided on the Request.");
            }

            // The auth header value from Authorization header must be a valid user id
            var user = await _userStore.FindByIdAsync(authorization.ToString(), default);
            if (user == null)
            {
                return AuthenticateResult.Fail("Invalid user id.");
            }

            // Create authenticated user ticket
            var identity = new ClaimsIdentity(Options.Scheme, USERNAME_CLAIM_TYPE, ROLES_CLAIM_TYPE);
            identity.AddClaim(new Claim(USERNAME_CLAIM_TYPE, user.UserName));
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Options.Scheme);

            return AuthenticateResult.Success(ticket);
        }
    }
}