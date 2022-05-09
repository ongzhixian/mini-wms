using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Wms.Extensions;

namespace Wms.Services;

public class MongoDbJwtTokenService : IJwtTokenService
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
        if (string.IsNullOrEmpty(jwt))
        {
            return (new ClaimsPrincipal(), string.Empty);
        }

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.ReadJwtToken(jwt);

        if (securityToken == null)
        {
            return (new ClaimsPrincipal(), string.Empty);
        }

        var claimsIdentity = new ClaimsIdentity(
            securityToken.Claims.Map(tokenHandler.InboundClaimTypeMap),
            CookieAuthenticationDefaults.AuthenticationScheme);

        return await Task.FromResult((new ClaimsPrincipal(claimsIdentity), string.Empty));
    }
}
