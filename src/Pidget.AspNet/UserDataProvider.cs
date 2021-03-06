using Microsoft.AspNetCore.Http;
using Pidget.Client.DataModels;
using System;
using System.Security.Claims;

namespace Pidget.AspNet
{
    public class UserDataProvider
    {
        public static UserDataProvider Default { get; }
             = new UserDataProvider();

        public UserData GetUserData(HttpContext http)
        {
            AssertContextNotNull(http);

            var user = http.User == null
                ? new UserData()
                : new UserData
                {
                    Id = GetId(http.User),
                    UserName = GetUserName(http.User),
                    Email = GetEmail(http.User),
                };

            user.IpAddress = GetIpAddress(http);

            return user;
        }

        private void AssertContextNotNull(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
        }

        public string GetUserName(ClaimsPrincipal user)
            => user.Identity?.Name
            ?? user.FindFirst(ClaimTypes.Name)?.Value;

        public string GetEmail(ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.Email)?.Value;

        public string GetId(ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public string GetIpAddress(HttpContext http)
            => GetXForwardedFor(http.Request)
            ?? http.Connection?.RemoteIpAddress?.ToString();

        private string GetXForwardedFor(HttpRequest req)
        {
            req.Headers?.TryGetValue("X-Forwarded-For",
                out var forwardedFor);

            return forwardedFor;
        }
    }
}
