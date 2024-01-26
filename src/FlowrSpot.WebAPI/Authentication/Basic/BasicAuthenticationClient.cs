using System.Security.Principal;

namespace FlowrSpot.WebAPI.Authentication.Basic
{
    public class BasicAuthenticationClient : IIdentity
    {
        public string? AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? Name { get; set; }
    }
}
