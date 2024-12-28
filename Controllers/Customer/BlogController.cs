using Microsoft.AspNetCore.Mvc;
using CarInsuranceManage.Models;
using CarInsuranceManage.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CarInsuranceManage.Controllers.Customer
{
    public class BlogController : Controller
    {
        private readonly CarInsuranceDbContext _context;

        // Constructor để inject CarInsuranceDbContext
        public BlogController(CarInsuranceDbContext context)
        {
            _context = context;
        }

        // Hiển thị các bình luận
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var comments = await _context.comments
                .Include(c => c.Customer)               // Lấy dữ liệu Customer
                .ThenInclude(c => c.User)               // Lấy dữ liệu User của Customer
                .Include(c => c.Replies)                // Lấy danh sách trả lời của bình luận
                .Where(c => c.status == "Active")       // Lọc bình luận có trạng thái "Active"
                .OrderByDescending(c => c.created_at)   // Sắp xếp theo thời gian tạo
                .ToListAsync();
             // Fetch active services for the view
            var services = await _context.insurance_services
                .ToListAsync();
            ViewData["Services"] = services;

            return View("~/Views/Customer/Blog/Index.cshtml", comments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(string message, int? parentCommentId)
        {
            // Kiểm tra người dùng đã đăng nhập chưa
            var customerId = HttpContext.Session.GetInt32("customer_id");
            if (!customerId.HasValue)
            {
                TempData["WarningMessage"] = "You need to be logged in to comment.";
                return RedirectToAction("Login", "Account");
            }

            var customer = await _context.customers.FirstOrDefaultAsync(c => c.customer_id == customerId.Value);
            if (customer == null)
            {
                TempData["WarningMessage"] = "Customer not found.";
                return RedirectToAction("Index");
            }

            // Tạo một bình luận mới hoặc trả lời cho một bình luận mẹ
            var comment = new Comment
            {
                customer_id = customer.customer_id,
                comment_text = message,
                rating = null,
                status = "Active",
                created_at = DateTime.Now,
                parent_comment_id = parentCommentId // Gán ID của bình luận mẹ nếu có
            };

            // Thêm vào cơ sở dữ liệu và lưu thay đổi
            _context.comments.Add(comment);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Your comment has been submitted successfully.";
            return RedirectToAction("Index");
        }

    }
}
