using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
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

    public async Task<PageResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            //Mini.Wms.MongoDbImplementation.Models.User userAccount = new Mini.Wms.MongoDbImplementation.Models.User();

            try
            {
                await userService.AddUserAsync(NewUser);

                ViewData["Alert"] = new BootstrapAlert()
                {
                    AlertType = BootstrapAlert.AlertTypeName.Success,
                    Description = "Record added."
                };

                ModelState.Clear();
                NewUser = new();
                return Page();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewData["Alert"] = new BootstrapAlert()
                {
                    AlertType = BootstrapAlert.AlertTypeName.Danger,
                    Description = "Unauthorized action."
                };
                return Page();
            }
            catch (Exception)
            {
                // Log something
                ViewData["Alert"] = new BootstrapAlert()
                {
                    AlertType = BootstrapAlert.AlertTypeName.Danger,
                    Description = "Server error."
                };
                return Page();
            }
        }

        ViewData["Alert"] = new BootstrapAlert()
        {
            AlertType = BootstrapAlert.AlertTypeName.Danger,
            Description = "Invalid form."
        };

        return Page();
    }
}
