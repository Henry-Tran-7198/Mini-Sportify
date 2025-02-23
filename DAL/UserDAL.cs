using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using Persistence.Models;

namespace DAL;

public class UserDAL
{
    private readonly string connectionString;
    
    public UserDAL()
    {
        connectionString = "Server=localhost;Database=SpotifyDB;Uid=root;Pwd=Duong7198!;";
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    public User? SignIn(string email, string password)
    {
        using var connection = new MySqlConnection(connectionString);
        using var command = new MySqlCommand(
            "SELECT * FROM users WHERE userEmail=@email AND userPassword=@password",
            connection
        );
        
        command.Parameters.AddWithValue("@email", email);
        command.Parameters.AddWithValue("@password", HashPassword(password));

        try
        {
            connection.Open();
            using var reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                return new User
                {
                    UserId = reader.GetInt32("userId"),
                    UserName = reader.GetString("userName"),
                    UserEmail = reader.GetString("userEmail"),
                    UserPassword = reader.GetString("userPassword"),
                    Roles = reader.GetString("roles")
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
        }
        return null;
    }

    public bool SignUp(User user)
    {
        using var connection = new MySqlConnection(connectionString);
        using var command = new MySqlCommand(
            "INSERT INTO users(userName, userEmail, userPassword, roles) VALUES(@userName, @email, @password, @roles)",
            connection
        );

        command.Parameters.AddWithValue("@userName", user.UserName);
        command.Parameters.AddWithValue("@email", user.UserEmail);
        command.Parameters.AddWithValue("@password", HashPassword(user.UserPassword));
        command.Parameters.AddWithValue("@roles", user.Roles);

        try
        {
            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
            return false;
        }
    }

    public User ? GetUserById(int userId)
    {
        using var connection = new MySqlConnection(connectionString);
        using var command = new MySqlCommand("SELECT * FROM users WHERE userId = @userId", connection);

        command.Parameters.AddWithValue("@userId", userId);

        try
        {
            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new User
                {
                    UserId = reader.GetInt32("userId"),
                    UserName = reader.GetString("userName"),
                    UserEmail = reader.GetString("userEmail"),
                    UserPassword = reader.GetString("userPassword"),
                    Roles = reader.GetString("roles")
                };
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Database error: {ex.Message}");
        }
        return null;
    }

    public bool DeleteUserById(int userId)
    {
        using var connection = new MySqlConnection(connectionString);
        using var command = new MySqlCommand("DELETE FROM users WHERE userId = @userId", connection);

        command.Parameters.AddWithValue("@userId", userId);
        try
        {
            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                System.Console.WriteLine("✅ User deleted successfully!");
                return true;
            }
            else
            {
                System.Console.WriteLine("❌ Failed to delete user.");
                return false;
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Database error: {ex.Message}");
            return false;
        }
    }

    public List<User> GetAllUsers()
    {
        List<User> users = new List<User>();
        using var connection = new MySqlConnection(connectionString);
        using var command = new MySqlCommand("SELECT * FROM users", connection);

        try
        {
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User
                {
                    UserId = reader.GetInt32("userId"),
                    UserName = reader.GetString("userName"),
                    UserEmail = reader.GetString("userEmail"),
                    UserPassword = reader.GetString("userPassword"),
                    Roles = reader.GetString("roles")
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
        }
        return users;
    }
}