using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class UDPClient
{
    static void Main()
    {
        UdpClient udpClient = null;
        try
        {
            // Địa chỉ IP và cổng của server
            string serverIP = "127.0.0.1";
            int port = 12345;

            // Tạo đối tượng UdpClient
            udpClient = new UdpClient();

            // Nội dung text gửi đến server
            string message = "Hello from client!";
            byte[] sendBytes = Encoding.ASCII.GetBytes(message);

            // Gửi dữ liệu tới server
            udpClient.Send(sendBytes, sendBytes.Length, serverIP, port);
            Console.WriteLine("Sent: " + message);

            // Nhận dữ liệu từ server
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            byte[] receiveBytes = udpClient.Receive(ref remoteEP);
            string receivedData = Encoding.ASCII.GetString(receiveBytes);
            Console.WriteLine("Received: " + receivedData);
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        finally
        {
            // Đóng kết nối
            udpClient.Close();
        }
    }
}
