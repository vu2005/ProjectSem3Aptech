using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using CarInsuranceManage.Models;
using CarInsuranceManage.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CarInsuranceManage.Controllers
{
    public class HomeController : Controller
    {
        // Khai báo biến _context kiểu CarInsuranceDbContext
        private readonly CarInsuranceDbContext _context;

        // Constructor để inject CarInsuranceDbContext vào controller
        public HomeController(CarInsuranceDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.insurance_services
        .Where(service => service.is_active)  // Lọc chỉ lấy dịch vụ đang hoạt động
        .Select(service => new InsuranceService
        {
            service_id = service.service_id,
            service_name = service.service_name,
            description = service.description,
            price = service.price,
            image_url = service.image_url
        })
        .ToListAsync();

            // Truyền danh sách dịch vụ vào ViewData
            ViewData["Services"] = services;

            // Kiểm tra quyền của người dùng và trả về view tương ứng
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole == "admin")
            {
                return View("~/Views/Admin/Home/Index.cshtml");
            }

            return View("~/Views/Customer/Home/Index.cshtml");
        }


        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            // Lấy tất cả bình luận của khách hàng
            var comments = await _context.comments
                .Include(c => c.Customer)   // Lấy dữ liệu của khách hàng
                .ThenInclude(c => c.User)   // Lấy thông tin người dùng từ Customer
                .Where(c => c.status == "Active")  // Chỉ lấy các bình luận có trạng thái "Active"
                .OrderByDescending(c => c.created_at)  // Sắp xếp bình luận theo thời gian tạo
                .ToListAsync();

            if (userRole == "admin")
            {
                return View("~/Views/Admin/Home/Dashboard.cshtml", comments);  // Truyền dữ liệu vào view
            }

            // Redirect về trang mặc định cho khách hàng nếu không phải admin
            return RedirectToAction("Index", "Home");
        }

    }
}
