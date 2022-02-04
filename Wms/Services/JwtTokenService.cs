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
    private readonly RsaKeySetting signingKeySetting;
    private readonly RsaKeySetting encryptingKeySetting;
    //private readonly RsaKeySetting privateKeySetting;

    //TokenValidationParameters tokenValidationParameters = new TokenValidationParameters();

    public JwtTokenService(IOptionsMonitor<RsaKeySetting> optionsMonitor)
    {
        signingKeySetting = optionsMonitor.Get(RsaKeyName.RecepSigningKey);
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
    public async Task<string> EncryptingKeyXmlAsync()
    {
        return await encryptingKeySetting.GetRsaSecurityKeyXmlAsync(false);
    }

    /// <summary>
    /// Returns RsaSecurity private key XML for decrypting
    /// </summary>
    public async Task<SecurityKey> DecryptingKeyAsync()
    {
        return await encryptingKeySetting.GetRsaSecurityKeyAsync(true);
    }

    internal async Task<ClaimsPrincipal> GetClaimsPrincipalAsync(string jwt, string? authenticationScheme = null)
    {
        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters();
        tokenValidationParameters.TokenDecryptionKey = await DecryptingKeyAsync();
        tokenValidationParameters.IssuerSigningKey = await SigningKeyAsync();
        tokenValidationParameters.ValidateAudience = false;
        tokenValidationParameters.ValidateIssuer = false;

        return new JwtSecurityTokenHandler().ValidateToken(jwt, tokenValidationParameters, out _);
    }

    internal async Task<string> GetSecurityAsync(string jwt, string? authenticationScheme = null)
    {
        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters();
        tokenValidationParameters.TokenDecryptionKey = await DecryptingKeyAsync();
        tokenValidationParameters.IssuerSigningKey = await SigningKeyAsync();
        tokenValidationParameters.ValidateAudience = false;
        tokenValidationParameters.ValidateIssuer = false;

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

        ClaimsPrincipal cp = handler.ValidateToken(jwt, tokenValidationParameters, out SecurityToken securityToken);



        string newBearer = handler.WriteToken(securityToken);

        //jwtSecurityToken = jwtSecurityTokenHandler.CreateJwtSecurityToken(
        //    issuer: jwtSetting.Issuer,
        //    audience: jwtSetting.Audience,
        //    subject: new ClaimsIdentity(authClaims),
        //    notBefore: DateTime.UtcNow,
        //    expires: DateTime.UtcNow.AddMinutes(jwtSetting.ExpirationMinutes),
        //    issuedAt: DateTime.UtcNow,
        //    signingCredentials: new SigningCredentials(
        //        signingCredentialSecurityKey
        //        , SecurityAlgorithms.RsaSsaPssSha256
        //        , SecurityAlgorithms.RsaSsaPssSha256Signature),
        //    encryptingCredentials: new EncryptingCredentials(
        //        encryptingCredentialSecurityKey
        //        , securityCredential.SecurityAlgorithm
        //        , securityCredential.SecurityDigest)
        //);

        return newBearer;
    }

    public async Task<JwtSecurityToken> ParseAsync(string jwtString)
    {
        var token = jwtSecurityTokenHandler.ReadJwtToken(jwtString);

        //(var scKey, var ecKey) = SecurityKeyHelper.SymmetricSecurityKey("SOME_SALT|SOME_PASSWORD|11970|16180", HashAlgorithmName.SHA256);

        //AsymmetricSecurityKey key  = signingKeySetting.GetRsaSecurityKey(false);

        TokenValidationParameters prm = new TokenValidationParameters();
        prm.TokenDecryptionKey = await DecryptingKeyAsync();
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
