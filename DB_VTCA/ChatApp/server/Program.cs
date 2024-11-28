using System.Net;
using System.Net.Sockets;
using ChatApp;

namespace ChatServer
{
    class Program
    {
        static TcpListener server = null;
        static List<ClientHandler> clients = new List<ClientHandler>();
        static DbHelper db = new DbHelper();

        static void Main(string[] args)
        {
            int port = 5000;
            IPAddress localAddr = IPAddress.Any;

            server = new TcpListener(localAddr, port);
            server.Start();
            Console.WriteLine($"Server started on port {port}.");

            Thread serverBroadCastThread = new Thread(ServerBroadcast);
            serverBroadCastThread.Start();

            try
            {
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Client connected.");
                    ClientHandler handler = new ClientHandler(client, BroadcastMessage, db);
                    lock (clients)
                    {
                        clients.Add(handler);
                    }
                    Thread clientThread = new Thread(new ThreadStart(handler.Run));
                    clientThread.Start();
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine($"SocketException: {se}");
            }
            finally
            {
                server.Stop();
            }
        }

        public static void ServerBroadcast()
        {
            while (true)
            {
                string message = Console.ReadLine();
                if (!string.IsNullOrEmpty(message))
                {
                    Console.WriteLine($"Server broadcasting: {message}");
                    BroadcastMessage($"Server: {message}", excludeClient: null);
                }
            }
        }
        // Phát tin nhắn đến tất cả các client
        public static void BroadcastMessage(string message, ClientHandler excludeClient)
        {
            lock (clients)
            {
                foreach (var client in clients)
                {
                    if (client != excludeClient)
                    {
                        client.SendMessage(message);
                    }
                }
            }
        }

        // Xóa client khỏi danh sách khi ngắt kết nối
        public static void RemoveClient(ClientHandler client)
        {
            lock (clients)
            {
                clients.Remove(client);
            }
            Console.WriteLine("Client disconnected.");
        }
    }
}