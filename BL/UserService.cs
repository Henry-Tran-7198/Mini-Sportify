using System.Text.RegularExpressions;
using DAL;
using Persistence.Models;

namespace BL;

public class UserService
{
    private readonly UserDAL _userDAL;

    public UserService()
    {
        _userDAL = new UserDAL();
    }

    /// Kiểm tra email có đúng định dạng không
    public bool IsValidEmail(string email){
        return _userDAL.IsValidEmail(email);
    }


    /// Đăng nhập người dùng
    public User? SignIn(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return null;
            
        if (!IsValidEmail(email))
            return null;
            
        return _userDAL.SignIn(email, password);
    }

    /// Đăng ký người dùng mới
    public bool SignUp(string userName, string email, string password, string role)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            return false;

        if (_userDAL.CheckUserNameExists(userName))
        {
            Console.WriteLine("Name already exists!");
            return false;
        }
        else if (_userDAL.CheckUserEmailExists(email))
        {
            Console.WriteLine("Email already exists!");
            return false;
        }

        if (!IsValidEmail(email))
            return false;

        var user = new User(userName, email, password, role.ToLower());
        return _userDAL.SignUp(user);
    }

    /// Kiểm tra tên người dùng đã tồn tại chưa
    public bool CheckUserNameExists(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return false;
            
        return _userDAL.CheckUserNameExists(userName);
    }

    /// Kiểm tra email đã tồn tại chưa
    public bool CheckUserEmailExists(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
            
        return _userDAL.CheckUserEmailExists(email);
    }

    /// Lấy thông tin người dùng theo ID
    public User? GetUserById(int userId)
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

    /// Tìm kiếm người dùng theo từ khóa
    public List<User> AdminSearchUsers(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return new List<User>();
        }
        
        return _userDAL.AdminSearchUsers(keyword);
    }

    /// Xóa người dùng theo ID
    public bool AdminDeleteUser(int userId)
    {
        if (userId <= 0)
        {
            Console.WriteLine("❌ Invalid ID. Please enter a valid number.");
            return false;
        }
        
        return _userDAL.AdminDeleteUser(userId);
    }

    /// Lấy tất cả người dùng
    public List<User> GetAllUsers()
    {
        return _userDAL.GetAllUsers();
    }
}