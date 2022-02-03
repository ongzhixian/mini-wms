using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mini.Common.Helpers;
using Mini.Common.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Wms.Models;


namespace Wms.Services;

public class JwtTokenService
{
    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
    private readonly RsaKeySetting2 signingKeySetting;
    private readonly RsaKeySetting encryptingKeySetting;
    //private readonly RsaKeySetting privateKeySetting;

    //TokenValidationParameters tokenValidationParameters = new TokenValidationParameters();

    public JwtTokenService(IOptionsMonitor<RsaKeySetting> optionsMonitor, IOptionsMonitor<RsaKeySetting2> optionsMonitor2)
    {
        signingKeySetting = optionsMonitor2.Get(RsaKeyName.SigningKey);
        encryptingKeySetting = optionsMonitor.Get(RsaKeyName.EncryptingKey);

        //privateKeySetting = optionsMonitor.Get(RsaKeyName.PrivateKey);

        //RSA pbk = RSA.Create();
        //RSA pvk = RSA.Create();

        ////Environment.GetEnvironmentVariable(setting2.Source);
        //pbk.FromXmlString(setting.RsaXml());
        //pvk.FromXmlString(setting2.RsaXml());

        //publicKeySetting.GetRsaSecurityKey(false);

        //var signingKey = await SigningKeyAsync();


        //tokenValidationParameters.TokenDecryptionKey = DecryptingKey;
        //tokenValidationParameters.IssuerSigningKey = signingKey;
        //tokenValidationParameters.ValidateAudience = false;
        //tokenValidationParameters.ValidateIssuer = false;
    }

    //public string PublicKeyXml
    //{
    //    get
    //    {
    //        return signingKeySetting.SourceXml();
    //        //return publicKeySetting.GetRsaSecurityKey(false);
    //    }
    //}

    public async Task<string> SigningKeyXmlAsync()
    {
        return await signingKeySetting.GetRsaSecurityKeyXmlAsync(false);
    }

    public async Task<SecurityKey> SigningKeyAsync()
    {
        return await signingKeySetting.GetRsaSecurityKeyAsync(false);
    }

    /// <summary>
    /// Returns public key XML for encrypting
    /// </summary>
    public string EncryptingKeyXml
    {
        get
        {
            return encryptingKeySetting.GetRsaSecurityKeyXml(false);
        }
    }

    /// <summary>
    /// Returns RsaSecurity private key XML for decrypting
    /// </summary>
    public SecurityKey DecryptingKey
    {
        get
        {
            return encryptingKeySetting.GetRsaSecurityKey(true);
        }
    }

    internal async Task<ClaimsPrincipal> GetClaimsPrincipalAsync(string jwt, string? authenticationScheme = null)
    {
        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters();
        tokenValidationParameters.TokenDecryptionKey = DecryptingKey;
        tokenValidationParameters.IssuerSigningKey = await SigningKeyAsync();
        tokenValidationParameters.ValidateAudience = false;
        tokenValidationParameters.ValidateIssuer = false;

        return new JwtSecurityTokenHandler().ValidateToken(jwt, tokenValidationParameters, out _);
    }



    public async Task<JwtSecurityToken> ParseAsync(string jwtString)
    {
        var token = jwtSecurityTokenHandler.ReadJwtToken(jwtString);

        //(var scKey, var ecKey) = SecurityKeyHelper.SymmetricSecurityKey("SOME_SALT|SOME_PASSWORD|11970|16180", HashAlgorithmName.SHA256);

        //AsymmetricSecurityKey key  = signingKeySetting.GetRsaSecurityKey(false);

        TokenValidationParameters prm = new TokenValidationParameters();
        prm.TokenDecryptionKey = DecryptingKey;
        prm.IssuerSigningKey = await SigningKeyAsync();
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
