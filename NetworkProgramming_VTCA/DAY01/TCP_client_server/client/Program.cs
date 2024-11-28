using System;
using System.Net.Sockets;
using System.Text;

class TCPClient
{
    static void Main(string[] args)
    {
        try
        {
            // Địa chỉ IP và cổng của server
            string serverIP = "10.10.20.40";
            int port = 12345;

            // Nội dung text gửi đến server
            string message = "hhhhhhhh";

            // Tạo đối tượng TcpClient
            TcpClient client = new TcpClient(serverIP, port);

            // Gửi dữ liệu tới server
            byte[] data = Encoding.ASCII.GetBytes(message);
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: " + message);

            // Nhận dữ liệu từ server
            data = new byte[256];
            int bytes = stream.Read(data, 0, data.Length);
            string responseData = Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("server tra loi la: " + responseData);

            // Đóng kết nối
            client.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }
}