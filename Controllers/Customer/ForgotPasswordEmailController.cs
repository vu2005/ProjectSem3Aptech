using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CarInsuranceManage.Models;
using CarInsuranceManage.Database;

namespace CarInsuranceManage.Controllers
{
    public class ForgotPasswordEmailController : Controller
    {
        private readonly CarInsuranceDbContext _context;

        public ForgotPasswordEmailController(CarInsuranceDbContext context)
        {
            _context = context;
        }

        // Phương thức để gửi email với mã xác nhận ngẫu nhiên
        public async Task<IActionResult> SendMail(string email)
        {
            // Tìm người dùng theo email
            var user = _context.users.SingleOrDefault(u => u.email == email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Email không hợp lệ.";
                return RedirectToAction("Login", "Account");
            }

            // Tạo mã xác nhận ngẫu nhiên
            var resetCode = GenerateRandomCode();

            // Lưu mã reset vào cơ sở dữ liệu
            user.password = resetCode;
            await _context.SaveChangesAsync();

            // Gửi email
            var result = await SendEmailAsync(email, resetCode);
            if (!result)
            {
                TempData["ErrorMessage"] = "Lỗi khi gửi email.";
                return RedirectToAction("Login", "Account");
            }

            // Thông báo gửi thành công
            TempData["SuccessMessage"] = "We have sent you the code, please use it to login!";
            return RedirectToAction("Login", "Account");
        }


        // Tạo mã xác nhận ngẫu nhiên
        private string GenerateRandomCode(int length = 6)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            StringBuilder randomCode = new StringBuilder(length);
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);
                for (int i = 0; i < length; i++)
                {
                    randomCode.Append(validChars[randomBytes[i] % validChars.Length]);
                }
            }
            return randomCode.ToString();
        }

        // Gửi email
        private async Task<bool> SendEmailAsync(string toEmail, string resetCode)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Hỗ trợ Khách Hàng", "insurancecarsore@gmail.com"));
                emailMessage.To.Add(new MailboxAddress("", toEmail));
                emailMessage.Subject = "Thông báo thay đổi mật khẩu tài khoản của bạn";

                // Thông điệp email chi tiết và chuyên nghiệp
                string emailBody = $@"
                    Chào bạn,

                    Chúng tôi nhận thấy có yêu cầu thay đổi mật khẩu cho tài khoản của bạn tại hệ thống Car Insurance. 
                    Nếu bạn không yêu cầu thay đổi mật khẩu, xin vui lòng bỏ qua email này.

                    Mật khẩu mới của bạn là: {resetCode}

                    Vui lòng sử dụng mật khẩu này để đăng nhập vào hệ thống.

                    Nếu bạn gặp bất kỳ khó khăn nào trong việc thay đổi mật khẩu, vui lòng liên hệ với chúng tôi qua email này hoặc gọi đến số hỗ trợ: 123-456-789.

                    Trân trọng,
                    Đội ngũ Hỗ trợ Khách Hàng
                    Car Insurance";

                emailMessage.Body = new TextPart("plain")
                {
                    Text = emailBody
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync("insurancecarsore@gmail.com", "bfuv iniw uecz xgyl");
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi gửi email
                Console.WriteLine($"Lỗi gửi email: {ex.Message}");
                return false;
            }
        }

    }
}
