using System.ComponentModel.DataAnnotations;

public class UserAuthorisationModel
{
    [EmailAddress]
    public  string Email { get; set; }
    public  string Password { get; set; }
}
