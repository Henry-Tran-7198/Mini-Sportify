using System.Text.RegularExpressions;
using DAL;
using Persistence.Models;

namespace BL.Services;

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
        => password.Length >= 8 && password.Any(char.IsUpper) && 
           password.Any(char.IsLower) && password.Any(char.IsDigit);

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

        var user = new User(userName, email, password, role.ToLower());
        return _userDAL.SignUp(user);
    }

    public User? SearchUser(int userId)
    {
        if (userId <= 0)
        {
            Console.WriteLine("❌ Invalid ID. Please enter a valid number.");
            return null;
        }

        var user = _userDAL.GetUserById(userId);
        if (user == null)
        {
            Console.WriteLine("❌ User not found.");
        }

        return user;
    }
    
    public bool DeleteUser(int userId)
    {
        UserDAL userDAL = new UserDAL(); //Tạo 1 đối tượng UserDAL để làm việc với database
        bool isDeleted = userDAL.DeleteUserById(userId); //Gọi phương thức DeleteUserById trong UserId

        if (isDeleted)
        {
            return true;
        }
        else
        {
            System.Console.WriteLine("❌ User not found!");
            return false;
        }
    }

    public List<User> GetAllUsers()
    {
        return _userDAL.GetAllUsers();
    }
}