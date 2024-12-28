using Microsoft.AspNetCore.Mvc;
using CarInsuranceManage.Models;
using CarInsuranceManage.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace CarInsuranceManage.Controllers.Admin
{
    public class SettingsController : Controller
    {
        private readonly CarInsuranceDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SettingsController(CarInsuranceDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // Hiển thị danh sách dịch vụ bảo hiểm
        [HttpGet]
        public async Task<IActionResult> Services()
        {
            var services = await _context.insurance_services.ToListAsync();
            return View("~/Views/Admin/Settings/services.cshtml", services);
        }

        // Thêm mới dịch vụ bảo hiểm
        [HttpGet]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Settings/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsuranceService service, IFormFile avatar)
        {
            if (service == null)
            {
                TempData["ErrorMessage"] = "Dữ liệu dịch vụ không hợp lệ.";
                return View("~/Views/Admin/Settings/Create.cshtml");
            }

            if (ModelState.IsValid)
            {
                // Xử lý file upload
                if (avatar != null && avatar.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(avatar.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        TempData["ErrorMessage"] = "Chỉ hỗ trợ các định dạng ảnh .jpg, .jpeg, .png.";
                        return View("~/Views/Admin/Settings/Create.cshtml", service);
                    }

                    if (avatar.Length > 5 * 1024 * 1024)
                    {
                        TempData["ErrorMessage"] = "Dung lượng ảnh không được vượt quá 5MB.";
                        return View("~/Views/Admin/Settings/Create.cshtml", service);
                    }

                    var fileName = Guid.NewGuid() + extension;
                    var uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "customer-assets/uploads/product");
                    var filePath = Path.Combine(uploadPath, fileName);

                    // Tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Lưu file ảnh
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await avatar.CopyToAsync(stream);
                    }

                    service.image_url = "customer-assets/uploads/product/" + fileName;
                }

                _context.insurance_services.Add(service);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Dịch vụ đã được thêm thành công.";
                return RedirectToAction(nameof(Services));
            }

            TempData["ErrorMessage"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.";
            return View("~/Views/Admin/Settings/Create.cshtml", service);
        }

        // Sửa thông tin dịch vụ bảo hiểm
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _context.insurance_services.FindAsync(id);
            if (service == null)
            {
                TempData["ErrorMessage"] = "Dịch vụ không tồn tại.";
                return RedirectToAction(nameof(Services));
            }
            return View("~/Views/Admin/Settings/Edit.cshtml", service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InsuranceService service, IFormFile avatar)
        {
            if (id != service.service_id)
            {
                TempData["ErrorMessage"] = "ID dịch vụ không hợp lệ.";
                return RedirectToAction(nameof(Services));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (avatar != null && avatar.Length > 0)
                    {
                        // Xóa file ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(service.image_url))
                        {
                            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, service.image_url.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        var fileName = Guid.NewGuid() + Path.GetExtension(avatar.FileName);
                        var uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "customer-assets/uploads/product");
                        var filePath = Path.Combine(uploadPath, fileName);

                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await avatar.CopyToAsync(stream);
                        }

                        service.image_url = "customer-assets/uploads/product/" + fileName;
                    }

                    _context.Update(service);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Dịch vụ đã được cập nhật thành công.";
                    return RedirectToAction(nameof(Services));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceServiceExists(service.service_id))
                    {
                        TempData["ErrorMessage"] = "Dịch vụ không tồn tại.";
                        return RedirectToAction(nameof(Services));
                    }
                    throw;
                }
            }

            TempData["ErrorMessage"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.";
            return View("~/Views/Admin/Settings/Edit.cshtml", service);
        }

        // Xóa dịch vụ bảo hiểm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _context.insurance_services.FindAsync(id);
            if (service != null)
            {
                if (!string.IsNullOrEmpty(service.image_url))
                {
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, service.image_url.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.insurance_services.Remove(service);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Dịch vụ đã được xóa thành công.";
            }
            else
            {
                TempData["ErrorMessage"] = "Dịch vụ không tồn tại.";
            }

            return RedirectToAction(nameof(Services));
        }

        private bool InsuranceServiceExists(int id)
        {
            return _context.insurance_services.Any(e => e.service_id == id);
        }
    }
}
