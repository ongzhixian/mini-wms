using Mini.Common.Responses;
using Mini.Wms.DomainMessages;
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
            , AuthenticationEndpoint authenticationEndpoint
            , UserEndpoint userEndpoint)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.authenticationEndpoint = authenticationEndpoint ?? throw new ArgumentNullException(nameof(authenticationEndpoint));
            this.userEndpoint = userEndpoint ?? throw new ArgumentNullException(nameof(userEndpoint));
        }

#pragma warning disable S125 // Sections of code should not be commented out

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

        //internal async Task GetClaimsPrincipalAsync(string jwt)
        //{
        //    await userEndpoint.GetUserAsync();
        //}

#pragma warning restore S125 // Sections of code should not be commented out

        public async Task<LoginResponse> AuthenticateAsync(LoginViewModel login)
        {
            return await authenticationEndpoint.PostAsync(login.Username, login.Password);
        }

        public async Task AddUserAsync(NewUserViewModel newUser)
        {
            await userEndpoint.AddUserAsync(newUser);
        }

        public async Task<IEnumerable<UserRecord>> GetAllUsersAsync()
        {
            return await userEndpoint.GetAllUsers();
        }
    }
}
