using Microsoft.IdentityModel.Tokens;
using Mini.Common.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Wms.Services;

public class JwtTokenService
{
    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

    public JwtTokenService()
    {
    }

    public JwtSecurityToken Parse(string jwtString)
    {
        var token = jwtSecurityTokenHandler.ReadJwtToken(jwtString);

        TokenValidationParameters prm = new TokenValidationParameters();
        (var scKey, var ecKey) = SecurityKeyHelper.SymmetricSecurityKey("SOME_SALT|SOME_PASSWORD|11970|16180", HashAlgorithmName.SHA256);


        byte[] pvkBytes = System.IO.File.ReadAllBytes(@"D:\src\github\recep\test2.pvk");

        RSA myRsa = RSA.Create();

        myRsa.ImportRSAPrivateKey(pvkBytes, out int count);

        var myRsaPbkParams = myRsa.ExportParameters(true);

        AsymmetricSecurityKey key = new RsaSecurityKey(myRsaPbkParams);


        prm.TokenDecryptionKey = key;
        prm.IssuerSigningKey = scKey;
        prm.ValidateAudience = false;
        prm.ValidateIssuer = false;
        //prm.ValidateLifetime = false;


        //JwtSecurityTokenHandler sec = new JwtSecurityTokenHandler();

        try
        {
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtString, prm, out SecurityToken validatedToken);
        }
        catch (Exception ex)
        {
            throw;
        }

        return token;
    }

}
