using System.ComponentModel.DataAnnotations;

namespace Wms.Models;

public class NewUserViewModel
{
    [Required]
    [Display(Name = "Username", Description = "User's username.", ShortName = "UNAME")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [Display(Name ="First Name", Description ="User's first name.", ShortName ="FNAME")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Last Name", Description = "User's last name.", ShortName = "LNAME")]
    public string LastName { get; set; } = string.Empty;

    //public string Password { get; set; } = string.Empty;
}
