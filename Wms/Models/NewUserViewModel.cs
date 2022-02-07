using Mini.Wms.DomainMessages;
using System.ComponentModel.DataAnnotations;

namespace Wms.Models;

//public record class SomeUserClass
//{
//    public string xName { get; set; }
//}

//public record class UserRec : IUser<string>
//{
//    public virtual string Username { get; init; } = string.Empty;

//    public string FirstName { get; init; } = string.Empty;

//    public string LastName { get; init; } = string.Empty;

//    public string Password { get; init; } = string.Empty;

//    public DateTime PasswordUpdatedDateTime { get; init; } = DateTime.MinValue;

//    public string Id { get; init; } = string.Empty;

//    //[JsonPropertyName("createdDateTime")]
//    //[BsonElement("createdDateTime")]
//    public DateTime CreatedDateTime { get; init; } = DateTime.MinValue;

//    //[JsonPropertyName("lastUpdatedDateTime")]
//    //[BsonElement("lastUpdatedDateTime")]
//    public DateTime LastUpdatedDateTime { get; init; } = DateTime.MinValue;
//}

//public record class NewUserViewModel : UserRec
//{
//    [Required]
//    [Display(Name = "Username", Description = "User's username.", ShortName = "UNAME")]
//    public override string Username { get; init; } = string.Empty;


//    [Required]
//    [Display(Name = "First Name", Description = "User's first name.", ShortName = "FNAME")]
//    public string FirstName { get; init; } = string.Empty;

//    [Required]
//    [Display(Name = "Last Name", Description = "User's last name.", ShortName = "LNAME")]
//    public string LastName { get; init; } = string.Empty;
//}

//public abstract record Dummy // (string Username, string FirstName, string LastName);
//{
//    private string Username { get; init; } = string.Empty;
//    private string FirstName { get; init; } = string.Empty;
//    private string LastName { get; init; } = string.Empty;

//}

//public readonly record struct NewUserDomainModel : Dummy()
//{
//    //public string Username { get; init; } = string.Empty;
//    //public string FirstName { get; init; } = string.Empty;
//    //public string LastName { get; init; } = string.Empty;
//}

public record class NewUserViewModel : INewUserMessage
{

    //public NewUserViewModel(string Username, string FirstName, string LastName) : base(Username, FirstName, LastName)
    //{
    //}


    //public NewUserViewModel(string username, string firstName, string lastName)
    //{
    //    Username = username;
    //    FirstName = firstName;
    //    LastName = lastName;
    //}

    //public NewUserViewModel(string Username, string FirstName, string LastName) : base(Username, FirstName, LastName)
    //{
    //}


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
