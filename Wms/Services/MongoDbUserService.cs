using Mini.Common.Models;
using Mini.Common.Responses;
using Mini.Wms.DomainMessages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Wms.DbContexts;
using Wms.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Wms.Models.Shared;
using MongoDB.Bson;
using Wms.Extensions;

namespace Wms.Services;

public class MongoDbUserService : IUserService
{
    private readonly ILogger<MongoDbUserService> logger;
    private readonly SharedMongoDbContext sharedMongoDbContext;

    public MongoDbUserService(ILogger<MongoDbUserService> logger, SharedMongoDbContext sharedMongoDbContext)
    {
        this.logger = logger;
        this.sharedMongoDbContext = sharedMongoDbContext;
    }

    public Task AddUserAsync(NewUserViewModel newUser)
    {
        throw new NotImplementedException();
    }

    public async Task<LoginResponse> AuthenticateAsync(LoginViewModel login)
    {
        User? localUser = await sharedMongoDbContext.Users.FindFindFirstOrDefaultAsync(login.Username);

        if (localUser == null)
        {
            return new LoginResponse
            {
                Jwt = string.Empty
            };
        }

        // Verify password

        if (localUser.Password == login.Password)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, localUser.Username));
            claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, localUser.Username));
            claims.AddRange(localUser.Roles.Select(localRole => new Claim(ClaimsIdentity.DefaultRoleClaimType, localRole)));

            var securityTokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            // Opting to do explict un-wind of mapping when reading JWT; See note below
            // tokenHandler.OutboundClaimTypeMap.Clear();

            // Note!
            // Instances of JwtSecurityTokenHandler makes use of OutboundClaimTypeMap
            // to encode the uri form into a simpler name (eg. "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" into "role"]
            // While there is nothing wrong with this, when reading JWT the InboundClaimTypeMap is not used. :-(
            // This means when reading the JWT we need to explicitly map the claims back to uri-form
            // or we are better off "disabling" OutboundClaimTypeMap.
            // Unfortunately, there is no flag like "MapOutboundClaims" (compared to "MapInboundClaims" a flag that exists for inbound claims)
            // There are 2 methods to remove all OutboundClaimTypeMap mappings:
            // Method 1: Remove global mapping; Future instances of JwtSecurityTokenHandler will not have OutboundClaimTypeMap mappings
            // JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            // Method 2: Remove mapping only for specific instance
            // var tokenHandler = new JwtSecurityTokenHandler();
            // tokenHandler.OutboundClaimTypeMap.Clear();
            // var jwt = tokenHandler.CreateEncodedJwt(desc);
            // There is another way to map using IClaimsTransformation
            // See: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/claims?view=aspnetcore-6.0

            return new LoginResponse
            {
                Jwt = tokenHandler.CreateEncodedJwt(securityTokenDescriptor)
            };
        }

        return new LoginResponse
        {
            Jwt = string.Empty
        };
    }

    public Task<IEnumerable<UserRecord>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<PagedData<UserRecord>> GetAllUsersAsync(PagedDataOptions pagedData)
    {
        throw new NotImplementedException();
    }
}
