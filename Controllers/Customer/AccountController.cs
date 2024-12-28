using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using CarInsuranceManage.Database;
using CarInsuranceManage.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CarInsuranceManage.Controllers.Customer
{
    public class AccountController : Controller
    {
        private readonly CarInsuranceDbContext _context;

        public AccountController(CarInsuranceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Login_phone()
        {
            // Fetch active services for the view
            var services = await _context.insurance_services
                .ToListAsync();
            ViewData["Services"] = services;

            return View("~/Views/Customer/Account/Login_Phone.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> Verify_phone()
        {
          // Fetch active services for the view
            var services = await _context.insurance_services
                .ToListAsync();
            ViewData["Services"] = services;


            return View("~/Views/Customer/Account/Verify_phone.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            // Fetch active services for the view
            var services = await _context.insurance_services
                .ToListAsync();
            ViewData["Services"] = services;


            return View("~/Views/Customer/Account/Login.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> Forgot_Password()
        {
           // Fetch active services for the view
            var services = await _context.insurance_services
                .ToListAsync();
            ViewData["Services"] = services;

            return View("~/Views/Customer/Account/Forgot_Password.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            // Fetch active services for the view
            var services = await _context.insurance_services
                .ToListAsync();
            ViewData["Services"] = services;



            return View("~/Views/Customer/Account/Register.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> Blog()
        {
            // Fetch active services for the view
            var services = await _context.insurance_services
                .ToListAsync();
            ViewData["Services"] = services;

            // Pass the services data to the ViewData
            ViewData["Services"] = services;

            return View("~/Views/Customer/Account/Blog.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> ProfileForCustomer()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Warning"] = "You must be logged in to view your profile.";
                return RedirectToAction("Login");
            }

            var user = _context.users.FirstOrDefault(u => u.username == User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
 // Fetch active services for the view
            var services = await _context.insurance_services
                .ToListAsync();
            ViewData["Services"] = services;

            // Pass the services data to the ViewData
            ViewData["Services"] = services;

            return View("~/Views/Customer/Account/Profile.cshtml", user);
        }

        [HttpPost]
        public IActionResult Profile(User updatedUser)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Warning"] = "You must be logged in to view your profile.";
                return RedirectToAction("Login");
            }

            var user = _context.users.FirstOrDefault(u => u.username == User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            user.full_name = updatedUser.full_name;
            user.email = updatedUser.email;
            user.phone_number = updatedUser.phone_number;
            user.address = updatedUser.address;
            user.password = updatedUser.password;

            _context.SaveChanges();

            TempData["Success"] = "Profile updated successfully.";
            return View("~/Views/Customer/Account/Profile.cshtml", user);
        }

        [HttpGet]
        public async Task<IActionResult> Info_Insurance()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Warning"] = "You must be logged in to view your profile.";
                return RedirectToAction("Login");
            }

            var user = _context.users.FirstOrDefault(u => u.username == User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

           // Fetch active services for the view
            var services = await _context.insurance_services
                .ToListAsync();
            ViewData["Services"] = services;

            return View("~/Views/Customer/Account/Info_Insurance.cshtml", user);
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("customer_id");
            HttpContext.Session.Remove("user_id");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
