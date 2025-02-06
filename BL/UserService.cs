using System.Text.RegularExpressions;
using DAL;
using Persistence;

namespace BL;

public class UserService
{
    private readonly UserDAL _userDAL;
    private readonly Regex _emailRegex = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

    public UserService()
    {
        _userDAL = new UserDAL();
    }

    public bool IsValidEmail(string email)
        => _emailRegex.IsMatch(email);

    public bool IsValidPassword(string password)
        => password.Length >= 8 && password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit);

    public User? SignIn(string email, string password)
    {
        if (!IsValidEmail(email))
            return null;
            
        return _userDAL.SignIn(email, password);
    }

    public bool SignUp(string userName, string email, string password, string role)
    {
        if (string.IsNullOrEmpty(userName) || !IsValidEmail(email) || 
            !IsValidPassword(password) || string.IsNullOrEmpty(role))
            return false;

        if (role != "listener" && role != "artist")
            return false;

        var user = new User(userName, email, password, role.ToLower());
        return _userDAL.SignUp(user);
    }
}