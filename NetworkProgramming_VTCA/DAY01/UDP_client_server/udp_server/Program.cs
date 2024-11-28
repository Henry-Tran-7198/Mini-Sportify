using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class UDPServer
{
    static void Main()
    {
        UdpClient udpServer = null;
        try
        {
            // Địa chỉ IP và cổng của server
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 12345;

            // Tạo đối tượng UdpClient
            udpServer = new UdpClient(port);

            Console.WriteLine("Server is listening...");

            while (true)
            {
                // Nhận dữ liệu từ client
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);
                byte[] receiveBytes = udpServer.Receive(ref remoteEP);
                string receivedData = Encoding.ASCII.GetString(receiveBytes);

                // In ra dữ liệu nhận được từ client
                Console.WriteLine("Received: " + receivedData);

                // Gửi dữ liệu trả lời về client
                string responseData = "Hello from server!";
                byte[] sendBytes = Encoding.ASCII.GetBytes(responseData);
                udpServer.Send(sendBytes, sendBytes.Length, remoteEP);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        finally
        {
            // Đóng kết nối
            udpServer.Close();
        }
    }
}
