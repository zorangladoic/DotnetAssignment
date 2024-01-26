using FlowrSpot.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using FlowrSpot.Application.Services;

namespace FlowrSpot.WebAPI.Authentication.Basic
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;
        public BasicAuthenticationHandler(IUserService userService,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // skip if endpoint has [AllowAnonymous] attribute defined
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                return AuthenticateResult.NoResult();

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization key");
            }

            var authorizationHeader = Request.Headers["Authorization"].ToString();

            if (!authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.Fail("Authorization header doesn't start with 'Basic '");
            }

            var authBase64Decode = Encoding.UTF8.GetString(
                Convert.FromBase64String(
                    authorizationHeader.Replace("Basic ", "", StringComparison.OrdinalIgnoreCase))
                );

            User? user;
            try 
            {
                var authSplit = authBase64Decode.Split(new[] { ':' }, 2);

                string clientUsername = authSplit[0];
                string clientSecret = authSplit[1];

                user = await _userService.Authenticate(clientUsername, clientSecret);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
            if (user == null)
            {
                return AuthenticateResult.Fail("The username or password is not correct");
            }

            var client = new BasicAuthenticationClient
            {
                AuthenticationType = BasicAuthenticationDefaults.AuthenticationSchemes,
                IsAuthenticated = true,
                Name = user.Username
            };

            var claimsPrinciple = new ClaimsPrincipal(new ClaimsIdentity(client, new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }));


            return AuthenticateResult.Success(
                new AuthenticationTicket(claimsPrinciple, Scheme.Name));
        }

    }
}
