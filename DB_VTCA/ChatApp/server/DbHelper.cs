using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ChatApp
{
    public class DbHelper
    {
        private string connectionString = "server=localhost;database=ChatApp;uid=root;pwd=iLoveNOTP69;";

        // Đăng ký người dùng mới
        public bool Register(string username, string password)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (Username, Password) VALUES (@username, @password)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password); // Trong thực tế, hãy mã hóa mật khẩu
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return false;
                }
            }
        }

        // Đăng nhập người dùng
        public int Login(string username, string password)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT UserId FROM Users WHERE Username=@username AND Password=@password";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    return -1; // Không tìm thấy người dùng
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return -1;
                }
            }
        }

        // Lưu tin nhắn
        public void SaveMessage(int senderId, string message)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO Messages (SenderId, MessageText) VALUES (@senderId, @message)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@senderId", senderId);
                cmd.Parameters.AddWithValue("@message", message);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        // Lấy lịch sử tin nhắn
        public List<string> GetMessageHistory()
        {
            List<string> messages = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT u.Username, m.MessageText, m.Timestamp FROM Messages m JOIN Users u ON m.SenderId = u.UserId ORDER BY m.Timestamp ASC";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                try
                {
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string username = reader.GetString("Username");
                        string message = reader.GetString("MessageText");
                        DateTime timestamp = reader.GetDateTime("Timestamp");
                        messages.Add($"[{timestamp}] {username}: {message}");
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return messages;
        }
    }
}