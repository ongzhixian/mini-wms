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
    private readonly RsaKeySetting signingKeySetting;
    private readonly RsaKeySetting encryptingKeySetting;
    //private readonly RsaKeySetting privateKeySetting;

    public JwtTokenService(IOptionsMonitor<RsaKeySetting> optionsMonitor)
    {
        signingKeySetting = optionsMonitor.Get(RsaKeyName.SigningKey);
        encryptingKeySetting = optionsMonitor.Get(RsaKeyName.EncryptingKey);

        //privateKeySetting = optionsMonitor.Get(RsaKeyName.PrivateKey);

        //RSA pbk = RSA.Create();
        //RSA pvk = RSA.Create();

        ////Environment.GetEnvironmentVariable(setting2.Source);
        //pbk.FromXmlString(setting.RsaXml());
        //pvk.FromXmlString(setting2.RsaXml());

        //publicKeySetting.GetRsaSecurityKey(false);
    }

    //public string PublicKeyXml
    //{
    //    get
    //    {
    //        return signingKeySetting.SourceXml();
    //        //return publicKeySetting.GetRsaSecurityKey(false);
    //    }
    //}

    public string SigningKeyXml
    {
        get
        {
            return signingKeySetting.RsaXml();
            //return publicKeySetting.GetRsaSecurityKey(false);
        }
    }

    public SecurityKey SigningKey
    {
        get
        {
            return signingKeySetting.GetRsaSecurityKey(false);
            //return publicKeySetting.GetRsaSecurityKey(false);
        }
    }

    public string EncryptingKeyXml
    {
        get
        {
            return encryptingKeySetting.RsaXml();
            //return publicKeySetting.GetRsaSecurityKey(false);
        }
    }

    public SecurityKey EncryptingKey
    {
        get
        {
            return encryptingKeySetting.GetRsaSecurityKey(true);
            //return publicKeySetting.GetRsaSecurityKey(false);
        }
    }

    public JwtSecurityToken Parse(string jwtString)
    {
        var token = jwtSecurityTokenHandler.ReadJwtToken(jwtString);
        
        //(var scKey, var ecKey) = SecurityKeyHelper.SymmetricSecurityKey("SOME_SALT|SOME_PASSWORD|11970|16180", HashAlgorithmName.SHA256);

        //AsymmetricSecurityKey key  = signingKeySetting.GetRsaSecurityKey(false);

        TokenValidationParameters prm = new TokenValidationParameters();
        prm.TokenDecryptionKey = EncryptingKey;
        prm.IssuerSigningKey = SigningKey;
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
