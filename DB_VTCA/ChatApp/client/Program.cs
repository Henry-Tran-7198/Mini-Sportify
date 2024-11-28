using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatClient
{
    class Program
    {
        static TcpClient client;
        static NetworkStream stream;
        static bool connected = false;

        static void Main(string[] args)
        {
            Console.Write("Nhập địa chỉ IP của server: ");
            string serverIP = Console.ReadLine();

            Console.Write("Nhập cổng (default 5000): ");
            string portInput = Console.ReadLine();
            int port = 5000;
            if (!string.IsNullOrEmpty(portInput))
            {
                int.TryParse(portInput, out port);
            }

            try
            {
                client = new TcpClient();
                client.Connect(serverIP, port);
                stream = client.GetStream();
                connected = true;
                Console.WriteLine("Kết nối đến server thành công.");

                // Xử lý đăng nhập hoặc đăng ký
                while (true)
                {
                    Console.WriteLine("1. Đăng nhập");
                    Console.WriteLine("2. Đăng ký");
                    Console.Write("Chọn chức năng: ");
                    string choice = Console.ReadLine();
                    if (choice == "1")
                    {
                        Console.Write("Username: ");
                        string username = Console.ReadLine();
                        Console.Write("Password: ");
                        string password = ReadPassword();
                        SendMessage($"LOGIN|{username}|{password}");
                        string response = ReadMessage();
                        if (response == "LOGIN_SUCCESS")
                        {
                            Console.WriteLine("Đăng nhập thành công.");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Đăng nhập thất bại. Thử lại.");
                        }
                    }
                    else if (choice == "2")
                    {
                        Console.Write("Username: ");
                        string username = Console.ReadLine();
                        Console.Write("Password: ");
                        string password = ReadPassword();
                        SendMessage($"REGISTER|{username}|{password}");
                        string response = ReadMessage();
                        if (response == "REGISTER_SUCCESS")
                        {
                            Console.WriteLine("Đăng ký thành công. Bạn có thể đăng nhập.");
                        }
                        else
                        {
                            Console.WriteLine("Đăng ký thất bại. Username có thể đã tồn tại.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Lựa chọn không hợp lệ.");
                    }
                }

                // Bắt đầu luồng nhận tin nhắn
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessages));
                receiveThread.Start();

                // Gửi tin nhắn
                while (connected)
                {
                    string message = Console.ReadLine();
                    if (message.ToLower() == "/exit")
                    {
                        connected = false;
                        client.Close();
                        break;
                    }
                    SendMessage(message);
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine($"SocketException: {se.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        static void SendMessage(string message)
        {
            if (stream != null && stream.CanWrite)
            {
                byte[] msg = Encoding.UTF8.GetBytes(message + "\n");
                stream.Write(msg, 0, msg.Length);
                stream.Flush();
            }
        }

        static string ReadMessage()
        {
            try
            {
                StringBuilder data = new StringBuilder();
                byte[] buffer = new byte[1024];
                int bytes = 0;
                do
                {
                    bytes = stream.Read(buffer, 0, buffer.Length);
                    data.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
                }
                while (stream.DataAvailable);

                if (data.Length == 0)
                    return null;

                return data.ToString().Trim();
            }
            catch
            {
                return null;
            }
        }

        static void ReceiveMessages()
        {
            try
            {
                while (connected)
                {
                    string message = ReadMessage();
                    if (message == null)
                        break;
                    Console.WriteLine(message);
                }
            }
            catch
            {
                Console.WriteLine("Ngắt kết nối với server.");
            }
            finally
            {
                connected = false;
                client.Close();
            }
        }

        // Đọc mật khẩu mà không hiển thị trên console
        static string ReadPassword()
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.Remove(password.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password.Append(info.KeyChar);
                    Console.Write("*");
                }
                info = Console.ReadKey(true);
            }
            Console.WriteLine();
            return password.ToString();
        }
    }
}