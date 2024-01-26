using Microsoft.AspNetCore.Authorization;

namespace FlowrSpot.WebAPI.Authentication.Basic.Attributes
{
    public class BasicAuthorizationAttribute : AuthorizeAttribute
    {
        public BasicAuthorizationAttribute()
        {
            AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationSchemes;
        }
    }
}
