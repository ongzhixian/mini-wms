using Mini.Wms.DomainMessages;
using System.ComponentModel.DataAnnotations;

namespace Wms.Models;

public record class NewUserViewModel : INewUserMessage
{

    [Required]
    [Display(Name = "Username", Description = "User's username.", ShortName = "UNAME")]
    public string Username { get; init; } = string.Empty;

    [Required]
    [Display(Name = "First Name", Description = "User's first name.", ShortName = "FNAME")]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    [Display(Name = "Last Name", Description = "User's last name.", ShortName = "LNAME")]
    public string LastName { get; init; } = string.Empty;

    //[Display(Name = "Password", Description = "User's password.", ShortName = "PWD")]
    //public string Password { get; init; } = string.Empty;
    //public DateTime PasswordUpdatedDateTime { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
    //public string Id { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
    //public DateTime CreatedDateTime { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
    //public DateTime LastUpdatedDateTime { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
}
