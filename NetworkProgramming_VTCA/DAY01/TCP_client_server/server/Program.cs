using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TCPServer
{
    static void Main()
    {
        TcpListener server = null;
        try
        {
            // Địa chỉ IP và cổng của server
            IPAddress ipAddress = IPAddress.Parse("10.10.20.87");
            int port = 12345;

            // Tạo đối tượng TcpListener
            server = new TcpListener(ipAddress, port);

            // Bắt đầu lắng nghe kết nối từ client
            server.Start();

            Console.WriteLine("Server is listening...");

            while (true)
            {
                // Chấp nhận kết nối từ client
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected by " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());

                NetworkStream stream = client.GetStream();

                byte[] bytes = new byte[256];
                int bytesRead = stream.Read(bytes, 0, bytes.Length);
                string dataReceived = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                Console.WriteLine("Received: " + dataReceived);

                // Gửi dữ liệu trả lời về client
                string responseData = "hello world";
                byte[] responseBytes = Encoding.ASCII.GetBytes(responseData);
                stream.Write(responseBytes, 0, responseBytes.Length);

                // Đóng kết nối
                client.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        finally
        {
            // Dừng lắng nghe kết nối
            server.Stop();
        }
    }
}