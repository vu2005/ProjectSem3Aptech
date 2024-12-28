using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CarInsuranceManage.Models;
using CarInsuranceManage.Database;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;

namespace CarInsuranceManage.Controllers.Customer
{
    public class ClaimController : Controller
    {
        private readonly CarInsuranceDbContext _context;

        public ClaimController(CarInsuranceDbContext context)
        {
            _context = context;
        }

        // GET: Hiển thị Form yêu cầu bồi thường
        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Customer/Claim/Index.cshtml");
        }

        // POST: Nhận dữ liệu từ form và xử lý yêu cầu bồi thường
        [HttpPost]
        public async Task<IActionResult> SendClaim(Claim model)
        {
            // Lấy customer_id từ session
            var customerId = HttpContext.Session.GetInt32("customer_id");

            if (!customerId.HasValue)
            {
                // Nếu không tìm thấy customer_id trong session, có thể chuyển hướng đến trang đăng nhập hoặc báo lỗi
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Lưu thông tin yêu cầu vào cơ sở dữ liệu và liên kết với customer_id
                model.customer_id = customerId.Value; // Gán customer_id vào yêu cầu bồi thường
                _context.claims.Add(model);
                await _context.SaveChangesAsync();

                // Xác định đường dẫn file PDF trong thư mục wwwroot
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Claim_pdf", "Claim_Form.pdf");

                // Kiểm tra xem file có tồn tại hay không
                if (!System.IO.File.Exists(filePath))
                {
                    TempData["ErrorMessage"] = "Đã xảy ra lỗi khi gửi yêu cầu bồi thường. Vui lòng thử lại sau.";
                    return View("Error");
                }

                // Soạn nội dung email
                var emailBody = $@"
            <p>Chào {model.customer_full_name},</p>
            <p>Chúng tôi đã nhận được yêu cầu bồi thường của bạn với các thông tin như sau:</p>
            <ul>
                <li><strong>Tên khách hàng:</strong> {model.customer_full_name}</li>
                <li><strong>Email:</strong> {model.customer_email}</li>
                <li><strong>Mô tả yêu cầu:</strong> {model.description}</li>
            </ul>

            <p>Vui lòng tải xuống và in <strong>form yêu cầu bồi thường</strong> đính kèm bên dưới. Bạn cần mang theo form này đến cửa hàng để hoàn tất thủ tục xác nhận yêu cầu bồi thường.</p>

            <p><strong>Lưu ý:</strong> Nếu bạn có bất kỳ câu hỏi nào hoặc cần thêm thông tin, đừng ngần ngại liên hệ với chúng tôi qua email này hoặc gọi điện đến <strong>hotline hỗ trợ</strong>.</p>

            <p>Chúng tôi sẽ tiếp tục xử lý yêu cầu của bạn ngay khi nhận được form yêu cầu bồi thường từ bạn.</p>

            <p>Cảm ơn bạn đã tin tưởng dịch vụ của chúng tôi!</p>

            <p>Trân trọng,<br/>Đội ngũ Bảo hiểm Xe hơi</p>
            <p><em>Lưu ý: Đây là email tự động, vui lòng không trả lời trực tiếp vào email này.</em></p>
        ";

                // Gửi email với file đính kèm
                var emailSent = await SendEmailAsync(model.customer_email, "Yêu Cầu Bồi Thường", emailBody, filePath);

                if (!emailSent)
                {
                    TempData["ErrorMessage"] = "Đã xảy ra lỗi khi gửi email. Vui lòng thử lại sau.";
                    return View("Error");
                }

                // Set a success message in TempData
                TempData["SuccessMessage"] = "Yêu cầu bồi thường đã được gửi thành công! Chúng tôi sẽ xử lý trong thời gian sớm nhất.";

                // Redirect sau khi gửi thành công
                return RedirectToAction("Index", "Home");
            }

            // Nếu có lỗi trong dữ liệu, trả lại form
            return View("Index", model);
        }

        // Phương thức gửi email
        private async Task<bool> SendEmailAsync(string toEmail, string subject, string body, string attachmentPath)
        {
            try
            {
                var emailMessage = new MailMessage
                {
                    From = new MailAddress("insurancecarsore@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                emailMessage.To.Add(toEmail);

                // Đính kèm file PDF nếu tồn tại
                if (System.IO.File.Exists(attachmentPath))
                {
                    var attachment = new Attachment(attachmentPath);
                    emailMessage.Attachments.Add(attachment);
                }

                // Sử dụng SMTP client để gửi email
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("insurancecarsore@gmail.com", "bfuv iniw uecz xgyl");

                    await client.SendMailAsync(emailMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
