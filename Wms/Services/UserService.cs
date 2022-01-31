using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Wms.Services.HttpClients;

namespace Wms.Services
{
    public class UserService
    {
        private readonly ILogger<UserService> logger;

        private readonly AuthenticationHttpClient httpClient;

        public UserService(
            ILogger<UserService> logger
            , AuthenticationHttpClient httpClient)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<ClaimsPrincipal> GetClaimsPrincipalAsync(string username, string password)
        {
            //string jwt = httpClient.GetJwt(username, password);

            var jwt = await httpClient.GetJwtAsync(username, password);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "someUsername"));

            ClaimsIdentity ci = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal cp = new ClaimsPrincipal(ci);

            return cp;
        }
    }
}
