namespace Persistence.Models;

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public string UserPassword { get; set; } = null!;
    public string Roles { get; set; } = null!;

    public User() { }

    public User(string userName, string userEmail, string userPassword, string roles)
    {
        UserName = userName;
        UserEmail = userEmail;
        UserPassword = userPassword;
        Roles = roles;
    }
}