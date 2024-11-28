using System;
using System.Net.NetworkInformation;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Text;
using MailKit.Net.Imap;
using MailKit;
using MimeKit;

namespace NetworkApplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int choice = 0;
            do
            {
                Console.WriteLine("===== ỨNG DỤNG MẠNG TỔNG HỢP =====");
                Console.WriteLine("1. Kiểm tra kết nối Internet");
                Console.WriteLine("2. Gửi yêu cầu HTTP GET");
                Console.WriteLine("3. Gửi yêu cầu HTTP POST");
                Console.WriteLine("4. Gửi Email");
                Console.WriteLine("5. Đọc Email");
                Console.WriteLine("6. Thoát");
                Console.Write("Chọn chức năng (1-6): ");
                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        CheckInternetConnection();
                        break;
                    case 2:
                        await SendHttpGetRequest();
                        break;
                    case 3:
                        await SendHttpPostRequest();
                        break;
                    case 4:
                        SendEmail();
                        break;
                    case 5:
                        ReadEmails();
                        break;
                    case 6:
                        Console.WriteLine("Thoát ứng dụng.");
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                        break;
                }

                Console.WriteLine();
            } while (choice != 6);
        }

        // Chức năng 1: Kiểm tra kết nối Internet
        static void CheckInternetConnection()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine("Máy tính có kết nối Internet.");
                }
                else
                {
                    Console.WriteLine("Máy tính không có kết nối Internet.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Không thể kiểm tra kết nối Internet.");
            }
        }

        // Chức năng 2: Gửi yêu cầu HTTP GET
        static async Task SendHttpGetRequest()
        {
            Console.Write("Nhập URL để gửi yêu cầu GET: ");
            string url = Console.ReadLine();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode(); // Kiểm tra trạng thái phản hồi
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Kết quả nhận được:");
                    Console.WriteLine(responseBody);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Lỗi yêu cầu: {e.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi chung: {ex.Message}");
                }
            }
        }

        // Chức năng 3: Gửi yêu cầu HTTP POST
        static async Task SendHttpPostRequest()
        {
            Console.Write("Nhập URL để gửi yêu cầu POST: ");
            string url = Console.ReadLine();
            Console.Write("Nhập dữ liệu JSON để gửi: ");
            string jsonData = Console.ReadLine();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode(); // Kiểm tra trạng thái phản hồi
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Kết quả nhận được:");
                    Console.WriteLine(responseBody);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Lỗi yêu cầu: {e.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi chung: {ex.Message}");
                }
            }
        }

        // Chức năng 4: Gửi Email
        static void SendEmail()
        {
            try
            {
                Console.Write("Nhập địa chỉ email người nhận: ");
                string toEmail = Console.ReadLine();
                Console.Write("Nhập chủ đề email: ");
                string subject = Console.ReadLine();
                Console.Write("Nhập nội dung email: ");
                string body = Console.ReadLine();

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("tduy527@gmail.com");
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;

                Console.Write("Bạn có muốn đính kèm tệp tin? (y/n): ");
                string attachChoice = Console.ReadLine();
                if (attachChoice.ToLower() == "y")
                {
                    Console.Write("Nhập đường dẫn tệp tin đính kèm: ");
                    string attachmentPath = Console.ReadLine();
                    if (System.IO.File.Exists(attachmentPath))
                    {
                        Attachment attachment = new Attachment(attachmentPath);
                        mail.Attachments.Add(attachment);
                    }
                    else
                    {
                        Console.WriteLine("Tệp tin không tồn tại.");
                    }
                }

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587, // Cổng SMTP
                    Credentials = new NetworkCredential("tduy527@gmail.com", "iwyr ybmy bfsy folz"),
                    EnableSsl = true // Bật SSL
                };

                smtpClient.Send(mail);
                Console.WriteLine("Email đã được gửi thành công.");
            }
            catch (SmtpFailedRecipientsException ex)
            {
                foreach (SmtpFailedRecipientException recipientEx in ex.InnerExceptions)
                {
                    Console.WriteLine($"Không thể gửi email đến {recipientEx.FailedRecipient}: {recipientEx.Message}");
                }
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Lỗi SMTP: {ex.StatusCode} - {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gửi email: {ex.Message}");
            }
        }

        // Chức năng 5: Đọc Email
        static void ReadEmails()
        {
            try
            {
                using (var client = new ImapClient())
                {
                    try {
                        ServicePointManager.ServerCertificateValidationCallback = 
                            (sender, certificate, chain, sslPolicyErrors) => true;
                        client.Connect("imap.gmail.com", 993, MailKit.Security.SecureSocketOptions.SslOnConnect);
                        client.Authenticate("tduy527@gmail.com", "iwyr ybmy bfsy folz");

                        // Mở hộp thư đến (INBOX)
                        var inbox = client.Inbox;
                        inbox.Open(FolderAccess.ReadOnly);
                        Console.WriteLine($"Số lượng email: {inbox.Count}");

                        // Đọc các email
                        for (int i = 0; i < inbox.Count; i++)
                        {
                            var message = inbox.GetMessage(i);
                            Console.WriteLine("====================================");
                            Console.WriteLine($"Email số {i + 1}:");
                            Console.WriteLine($"Chủ đề: {message.Subject}");
                            Console.WriteLine($"Người gửi: {message.From}");
                            Console.WriteLine($"Ngày gửi: {message.Date}");
                            Console.WriteLine($"Nội dung: {message.TextBody}");
                        }

                    client.Disconnect(true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi khi đọc email: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đọc email: {ex.Message}");
            }
        }
    }
}
