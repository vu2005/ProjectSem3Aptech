using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using CarInsuranceManage.Models;
using Microsoft.EntityFrameworkCore;
using CarInsuranceManage.Database;

namespace CarInsuranceManage.Controllers.Customer
{
    public class SuportController : Controller
    {
        private readonly CarInsuranceDbContext _context;

        public SuportController(CarInsuranceDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendMail(string email)
        {
            // Kiểm tra nếu email hợp lệ
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                TempData["MessageSendMail"] = "Please provide a valid email address.";
                return Redirect(Request.Headers["Referer"].ToString() ?? Url.Action("Index", "Home"));
            }

            try
            {
                // Cấu hình gửi email
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("insurancecarsore@gmail.com", "bfuv iniw uecz xgyl"), // This should be secured!
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("insurancecarsore@gmail.com"),
                    Subject = "Support Request - Car Insurance Management",
                    Body = $@"Hello,

                        Thank you for reaching out to Car Insurance Management. We have received your request for support and our team is on it! 

                        We're committed to providing you with the best assistance possible. Please allow us some time to review your query and get back to you with a comprehensive response.

                        In the meantime, feel free to explore our [FAQs](#) or [Customer Support Portal](#) for immediate answers to common questions.

                        Best regards,
                        The Car Insurance Management Team

                        ---

                        Customer Support: support@carinsurancemanagement.com
                        Phone: +84-384-804-325
                        Website: www.carinsurancemanagement.com
                        ",
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(email);

                // Gửi email
                await smtpClient.SendMailAsync(mailMessage);

                TempData["MessageSendMail"] = "An email has been sent to you";
            }
            catch (Exception ex)
            {
                TempData["MessageSendMail"] = $"Error: {ex.Message}";
            }

            // Quay lại trang trước đó hoặc trang mặc định
            return Redirect(Request.Headers["Referer"].ToString() ?? Url.Action("Index", "Home"));
        }

        // Kiểm tra định dạng email
        private bool IsValidEmail(string email)
        {
            // Regex kiểm tra email hợp lệ
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        [HttpPost]
        public async Task<IActionResult> Question(string username, string email, string subject)
        {
            // Kiểm tra nếu người dùng đã đăng nhập
            if (!User.Identity.IsAuthenticated)
            {
                TempData["MessageSendMailSupportRequestError"] = "You must be logged in to submit a support request.";
                return RedirectToAction("Login", "Account"); // Chuyển hướng tới trang đăng nhập
            }

            // Kiểm tra nếu email hợp lệ
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                TempData["MessageSendMailSupportRequestError"] = "Please provide a valid email address.";
                return Redirect(Request.Headers["Referer"].ToString() ?? Url.Action("Index", "Home"));
            }

            // Tạo nội dung email tùy thuộc vào loại bảo hiểm
            string bodyContent = GetEmailBodyContent(username, subject);

            if (string.IsNullOrEmpty(bodyContent))
            {
                TempData["MessageSendMailSupportRequestError"] = "Invalid subject.";
                return Redirect(Request.Headers["Referer"].ToString() ?? Url.Action("Index", "Home"));
            }

            try
            {
                // Gửi email thông báo
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("insurancecarsore@gmail.com", "bfuv iniw uecz xgyl"), // This should be secured!
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("insurancecarsore@gmail.com"),
                    Subject = "Support Request - Car Insurance Management",
                    Body = bodyContent,
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(email);

                // Gửi email
                await smtpClient.SendMailAsync(mailMessage);

                // Lấy thông tin khách hàng từ session (hoặc User.Identity.Name nếu bạn có thông tin khách hàng trong cơ sở dữ liệu)
                var customerId = HttpContext.Session.GetInt32("customer_id");

                if (!customerId.HasValue)
                {
                    TempData["MessageSendMailSupportRequestError"] = "Customer ID not found in session. Please log in again.";
                    return RedirectToAction("Login", "Account");
                }

                var customer = await _context.customers.FirstOrDefaultAsync(c => c.customer_id == customerId.Value);
                if (customer == null)
                {
                    TempData["MessageSendMailSupportRequestError"] = "Customer not found.";
                    return Redirect(Request.Headers["Referer"].ToString() ?? Url.Action("Index", "Home"));
                }

                // Lưu yêu cầu hỗ trợ vào cơ sở dữ liệu
                var supportRequest = new CustomerSupportRequest
                {
                    customer_id = customer.customer_id,
                    support_type = subject,
                    support_description = "N/A", // Mô tả có thể được lấy từ một form khác
                    support_payment = "N/A", // Thanh toán có thể được lấy từ form
                    support_status = "Open", // Trạng thái ban đầu là "Open"
                    created_at = DateTime.Now,
                    resolved_by = null, // Chưa có nhân viên xử lý
                };

                _context.customer_support_requests.Add(supportRequest);
                await _context.SaveChangesAsync();

                // Thông báo gửi thành công
                TempData["MessageSendMailSupportRequestSuccess"] = "Your support request has been submitted successfully!";
            }
            catch (Exception ex)
            {
                TempData["MessageSendMailSupportRequestError"] = $"Error: {ex.Message}";
            }

            return Redirect(Request.Headers["Referer"].ToString() ?? Url.Action("Index", "Home"));
        }

        private string GetEmailBodyContent(string username, string subject)
        {
            string bodyContent = string.Empty;

            // Xác định nội dung email tùy thuộc vào lựa chọn của khách hàng
            if (subject == "Giá Bảo Hiểm xe Ô tô")
            {
                bodyContent = $@"Hello {username}, 

                        Thank you for your interest in car insurance! 

                        Here is the pricing information for car insurance:
                        - Price: $500 per year
                        - Coverage: Full coverage, including accidents, theft, and natural disasters.

                        If you have any further questions, feel free to contact us.

                        Best regards,
                        The Car Insurance Team";
            }
            else if (subject == "Giá Bảo Hiểm xe Mô tô")
            {
                bodyContent = $@"Hello {username}, 

                        Thank you for your interest in motorcycle insurance! 

                        Here is the pricing information for motorcycle insurance:
                        - Price: $200 per year
                        - Coverage: Accidents, theft, and liability.

                        If you have any further questions, feel free to contact us.

                        Best regards,
                        The Car Insurance Team";
            }
            else if (subject == "Giá Bảo Hiểm Dành Cho Xe Tải")
            {
                bodyContent = $@"Hello {username}, 

                        Thank you for your interest in truck insurance! 

                        Here is the pricing information for truck insurance:
                        - Price: $800 per year
                        - Coverage: Comprehensive coverage including cargo, accidents, and liability.

                        If you have any further questions, feel free to contact us.

                        Best regards,
                        The Car Insurance Team";
            }

            return bodyContent;
        }
    }
}


