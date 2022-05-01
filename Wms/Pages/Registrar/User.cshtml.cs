    using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mini.Common.Models;
using Mini.Wms.DomainMessages;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Wms.Models;
using Wms.Services;

namespace Wms.Pages.Registrar;

public class UserModel : PageModel
{
    [BindProperty]
    public IEnumerable<UserRecord> UserList { get; set; } = new List<UserRecord>();

    [BindProperty]
    public PagedDataOptions PagedData { get; set; } = new PagedDataOptions();

    [BindProperty(SupportsGet = true, Name = "pg")]
    public int PageNum { get; set; } = 1;

    //[BindProperty(SupportsGet = true)]
    //public int PageSize { get; set; }

    private readonly UserService userService;

    public UserModel(UserService userService)
    {
        this.userService = userService ?? throw new Exception(nameof(userService));

        // SetupPageDefaults();
    }

    void SetupPageDefaults()
    {
        PagedData.Page = 1;
        PagedData.PageSize = 5;
        PagedData.DataType = "User";
        PagedData.DataFieldList.Add(new DataField("Username", true, 1));
        PagedData.DataFieldList.Add(new DataField("FirstName", true, 2));
        PagedData.DataFieldList.Add(new DataField("LastName", true, 3));
    }

    public void OnGet()
    {
        //SetupPageDefaults();

        //if (Request.Query.ContainsKey("pg") && int.TryParse(Request.Query["pg"], out int pageNumber) && pageNumber > 0)
        //{
        //    PagedData.Page = (uint)pageNumber;
        //}

        //if (Request.Query.ContainsKey("ps") && int.TryParse(Request.Query["ps"], out int pageSize) && pageSize > 0)
        //{
        //    PagedData.PageSize = (uint)pageSize;
        //}

        // var result = await userService.GetAllUsersAsync(PagedData);

        //UserList = result.Data;

        //RedirectToPage("Query", new { name1 = "value1", name2 = "value2" });
    }
}



//public record TableColumn
//{
//    public string HeaderText { get; set; } = string.Empty;
//    public bool SortAsc { get; set; } = true;

//}


//public readonly record struct UserTableRecord(
//    [Display(Order = 0)]
//    string Username,
//    [Display(Order = 2)]
//    string FirstName,
//    [Display(Order = 1)]
//    string LastName
//    );

//public readonly record struct UserTableRecord2()
//{
//    [Display(Order = 0)]
//    public readonly string Username { get; init; } = String.Empty;

//    [Display(Order = 2)]
//    public readonly string FirstName { get; init; } = String.Empty;

    
//    //public readonly string Password { get; init; } = String.Empty;

//    [Display(Order = 1)]
//    public readonly string LastName { get; init; } = String.Empty;

//};
