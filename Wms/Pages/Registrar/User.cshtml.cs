using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mini.Wms.DomainMessages;
using Wms.Services;

namespace Wms.Pages.Registrar;

public class UserModel : PageModel
{
    [BindProperty]
    public IEnumerable<UserRecord> UserList { get; set; } = new List<UserRecord>();

    private readonly UserService userService;

    public UserModel(UserService userService)
    {
        this.userService = userService ?? throw new Exception(nameof(userService));
    }

    public async Task OnGetAsync()
    {
        //IEnumerable<UserRecord> result = await userService.GetAllUsersAsync();

        UserList = await userService.GetAllUsersAsync();

    }
}
