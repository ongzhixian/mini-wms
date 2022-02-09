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
    public IList<TableColumn> TableColumns { get; set; } = new List<TableColumn>();

    [BindProperty]
    public PagedDataOptions PagedData { get; set; } = new PagedDataOptions(); 


    private readonly UserService userService;

    public UserModel(UserService userService)
    {
        this.userService = userService ?? throw new Exception(nameof(userService));
    }

    public async Task OnGetAsync()
    {
        PagedData.Page = 1;
        PagedData.PageSize = 5;
        PagedData.DataType = "User";
        PagedData.DataFieldList.Add(new DataField("Username", true, 1));
        PagedData.DataFieldList.Add(new DataField("FirstName", true, 2));
        PagedData.DataFieldList.Add(new DataField("LastName", true, 3));

        //IEnumerable<UserRecord> result = await userService.GetAllUsersAsync();
        //UserList = await userService.GetAllUsersAsync(Page, PageSize, Filter, SortOrder);

        //FieldInfo[] fields = typeof(UserRecord).GetFields(BindingFlags.Public | BindingFlags.Instance);
        //PropertyInfo[] props = typeof(UserTableRecord2).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //var att1 = props[0].Attributes;
        //var att2 = props[0].CustomAttributes;

        //FieldInfo[] props2 = typeof(UserTableRecord).GetFields(BindingFlags.Public | BindingFlags.Instance);

        //var att3 = props2[0].Attributes;
        //var att4 = props2[0].CustomAttributes;

        //PropertyInfo[] props3 = typeof(UserRecord).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //var att5 = props3[0].Attributes;
        //var att6 = props3[0].CustomAttributes;

        //var columnDisplayOrder = props
        //    .OrderByDescending(r => r.IsDefined(typeof(DisplayAttribute)))
        //    .ThenBy(r => r.GetCustomAttribute<DisplayAttribute>()?.Order);

        //foreach (var prop in columnDisplayOrder)
        //{
        //    this.TableColumns.Add(new TableColumn
        //    {
        //        HeaderText = prop.Name,
        //    });
        //}

        var result = await userService.GetAllUsersAsync(PagedData);
        UserList = result.Data;
    }
}



public record TableColumn
{
    public string HeaderText { get; set; } = string.Empty;
    public bool SortAsc { get; set; } = true;

}


public readonly record struct UserTableRecord(
    [Display(Order = 0)]
    string Username,
    [Display(Order = 2)]
    string FirstName,
    [Display(Order = 1)]
    string LastName
    );

public readonly record struct UserTableRecord2()
{
    [Display(Order = 0)]
    public readonly string Username { get; init; } = String.Empty;

    [Display(Order = 2)]
    public readonly string FirstName { get; init; } = String.Empty;

    
    //public readonly string Password { get; init; } = String.Empty;

    [Display(Order = 1)]
    public readonly string LastName { get; init; } = String.Empty;

};
