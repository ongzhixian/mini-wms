using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Wms.Services;

public class LocalJwtTokenService : IJwtTokenService
{
    public Task<SecurityKey> DecryptingKeyAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> EncryptingKeyXmlAsync()
    {
        throw new NotImplementedException();
    }

    public Task<SecurityKey> SigningKeyAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> SigningKeyXmlAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<(ClaimsPrincipal, string)> GetSecurityAsync(string jwt)
    {
        List<Claim> claims = new List<Claim>();

        claims.Add(new Claim(ClaimTypes.Name, "zhixian"));

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return (claimsPrincipal, string.Empty);

    }
}
