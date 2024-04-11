using System.ComponentModel.DataAnnotations;

public class UserRegistrationModel
{
    public  string Name {  get; set; }
    public  string Password { get; set; }
    [EmailAddress]
    public  string Email { get; set; }

}