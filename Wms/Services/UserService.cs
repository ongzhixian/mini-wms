using Microsoft.AspNetCore.Mvc.RazorPages;
using Mini.Common.Responses;
using Mini.Wms.DomainMessages;
using System.Linq.Expressions;
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

        internal async Task<IEnumerable<UserRecord>> GetAllUsersAsync(object filter, object sortOrder, uint page = 1, uint pageSize = 12)
        {
            IEnumerable<UserRecord>? res = await userEndpoint.GetAllUsers();

            res = res.Where(r => r.Username == "asd");

            res = res.OrderBy(r => r.LastName);

            return res;
        }

        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string sortColumn, bool descending)
        {
            // Dynamically creates a call like this: query.OrderBy(p =&gt; p.SortColumn)
            var parameter = Expression.Parameter(typeof(T), "p");

            string command = "OrderBy";

            if (descending)
            {
                command = "OrderByDescending";
            }

            Expression resultExpression = null;

            var property = typeof(T).GetProperty(sortColumn);
            // this is the part p.SortColumn
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);

            // this is the part p =&gt; p.SortColumn
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            // finally, call the "OrderBy" / "OrderByDescending" method with the order by lamba expression
            resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { typeof(T), property.PropertyType },
               query.Expression, Expression.Quote(orderByExpression));

            return query.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
