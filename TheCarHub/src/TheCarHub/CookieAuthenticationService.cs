using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Cookies = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults;

namespace TheCarHub
{
    public class CookieAuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieAuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<Result> Login(string username, string password)
        {
            if (!"ADMIN".Equals(username?.ToUpperInvariant()) ||
                !"P@ssw0rd".Equals(password))
            {
                return new Result("Login credentials were not correct. Please try again.");
            }

            var principal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, username),
                    },
                    Cookies.AuthenticationScheme));

            await this._httpContextAccessor.HttpContext!
                .SignInAsync(Cookies.AuthenticationScheme, principal);

            return new Result();
        }

        public Task Logout()
        {
            return this._httpContextAccessor.HttpContext!
                .SignOutAsync(Cookies.AuthenticationScheme);
        }
    }
}