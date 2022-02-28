using Mini.Common.Models;
using Mini.Common.Responses;
using Mini.Wms.DomainMessages;
using Wms.Models;

namespace Wms.Services;

public class LocalUserService : IUserService
{
    public Task AddUserAsync(NewUserViewModel newUser)
    {
        throw new NotImplementedException();
    }

    public Task<LoginResponse> AuthenticateAsync(LoginViewModel login)
    {
        return Task.FromResult(new LoginResponse
        {
            Jwt = ""
        });
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
