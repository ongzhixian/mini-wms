using Microsoft.AspNetCore.Authentication.Cookies;
using Mini.Common.Responses;
using System.Security.Claims;
using Wms.Models;
using Wms.Services.HttpClients;

namespace Wms.Services
{
    public class UserService
    {
        private readonly ILogger<UserService> logger;

        private readonly AuthenticationEndpoint authenticationEndpoint;
        private readonly UserEndpoint userEndpoint;


        public UserService(
            ILogger<UserService> logger
            , AuthenticationEndpoint httpClient
            , UserEndpoint userEndpoint)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.authenticationEndpoint = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.userEndpoint = userEndpoint ?? throw new ArgumentNullException(nameof(userEndpoint));
        }

        //[Obsolete]
        //public async Task<ClaimsPrincipal> GetClaimsPrincipalAsync(string username, string password)
        //{
        //    //string jwt = httpClient.GetJwt(username, password);

        //    System.IdentityModel.Tokens.Jwt.JwtSecurityToken? jwt = await authenticationEndpoint.GetJwtAsync(username, password);

        //    List<Claim> claims = new List<Claim>();
        //    claims.Add(new Claim(ClaimTypes.Name, "someUsername"));

        //    ClaimsIdentity ci = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //    ClaimsPrincipal cp = new ClaimsPrincipal(ci);

        //    return cp;
        //}

        public async Task<LoginResponse> AuthenticateAsync(LoginViewModel login)
        {
            return await authenticationEndpoint.PostAsync(login.Username, login.Password);
        }

        public async Task AddUserAsync(NewUserViewModel newUser)
        {
            await userEndpoint.AddUserAsync(newUser);
        }
    }
}
