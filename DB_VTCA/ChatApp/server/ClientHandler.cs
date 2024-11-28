using System.Net.Sockets;
using System.Text;
using ChatApp;

namespace ChatServer
{
    public class ClientHandler
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread thread;
        private Action<string, ClientHandler> broadcast;
        private DbHelper db;
        private int userId;
        private string username;

        public ClientHandler(TcpClient client, Action<string, ClientHandler> broadcast, DbHelper db)
        {
            this.client = client;
            this.broadcast = broadcast;
            this.db = db;
            this.stream = client.GetStream();
        }

        public void Run()
        {
            try
            {
                // Xử lý đăng nhập hoặc đăng ký
                bool authenticated = false;
                while (!authenticated)
                {
                    string authData = ReadMessage();
                    // Dữ liệu định dạng: "LOGIN|username|password" hoặc "REGISTER|username|password"
                    string[] parts = authData.Split('|');
                    if (parts.Length == 3)
                    {
                        string command = parts[0];
                        string user = parts[1];
                        string pass = parts[2];
                        if (command == "LOGIN")
                        {
                            int id = db.Login(user, pass);
                            if (id != -1)
                            {
                                userId = id;
                                username = user;
                                SendMessage("LOGIN_SUCCESS");
                                authenticated = true;
                                // Gửi lịch sử tin nhắn
                                var history = db.GetMessageHistory();
                                foreach (var msg in history)
                                {
                                    SendMessage(msg);
                                }
                            }
                            else
                            {
                                SendMessage("LOGIN_FAILED");
                            }
                        }
                        else if (command == "REGISTER")
                        {
                            bool success = db.Register(user, pass);
                            if (success)
                            {
                                SendMessage("REGISTER_SUCCESS");
                            }
                            else
                            {
                                SendMessage("REGISTER_FAILED");
                            }
                        }
                    }
                }

                // Bắt đầu nhận tin nhắn
                while (true)
                {
                    string data = ReadMessage();
                    if (data == null)
                        break;

                    Console.WriteLine($"Received from {username}: {data}");
                    string formattedMessage = $"[{DateTime.Now}] {username}: {data}";
                    db.SaveMessage(userId, data);
                    broadcast(formattedMessage, this);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                Close();
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                if (stream.CanWrite)
                {
                    byte[] msg = Encoding.UTF8.GetBytes(message + "\n");
                    stream.Write(msg, 0, msg.Length);
                    stream.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SendMessage Exception: {ex.Message}");
            }
        }

        private string ReadMessage()
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

        private void Close()
        {
            Program.RemoveClient(this);
            stream.Close();
            client.Close();
        }
    }
}