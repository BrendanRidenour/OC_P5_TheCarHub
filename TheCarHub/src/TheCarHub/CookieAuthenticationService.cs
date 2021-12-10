using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Cookies = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults;

namespace TheCarHub
{
    public class CookieAuthenticationService : IAuthenticationService
    {
        private readonly SecureLoginCredentials _credentials;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieAuthenticationService(SecureLoginCredentials credentials,
            IHttpContextAccessor httpContextAccessor)
        {
            this._credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
            this._httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<Result> Login(string username, string password)
        {
            if (!this._credentials.Username.ToUpperInvariant().Equals(username?.ToUpperInvariant()) ||
                !this._credentials.Password.Equals(password))
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