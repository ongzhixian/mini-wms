using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wms.Models;
using Wms.Services;

namespace Wms.Pages.Registrar.User;

public class AddModel : PageModel
{
    [BindProperty]
    public NewUserViewModel NewUser { get; set; } = new();

    private readonly UserService userService;

    public AddModel(UserService userService)
    {
        this.userService = userService ?? throw new Exception(nameof(userService));
    }

    public async Task OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            //Mini.Wms.MongoDbImplementation.Models.User userAccount = new Mini.Wms.MongoDbImplementation.Models.User();
            
            await userService.AddUserAsync(NewUser);
        }
    }
}
