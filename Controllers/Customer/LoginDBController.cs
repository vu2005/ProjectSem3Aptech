using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using CarInsuranceManage.Models;
using CarInsuranceManage.Database;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Claim = System.Security.Claims.Claim;

namespace CarInsuranceManage.Controllers.Customer
{
    public class LoginDBController : Controller
    {
        private readonly CarInsuranceDbContext _context;

        public LoginDBController(CarInsuranceDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                TempData["WarningMessage"] = "Please enter both email and password.";
                return View("~/Views/Customer/Account/Login.cshtml");
            }

            // Check if the user exists
            var user = await _context.users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
            {
                TempData["WarningMessage"] = "Invalid email or password.";
                return View("~/Views/Customer/Account/Login.cshtml");
            }

            if (user.user_logs == "Google")
            {
                TempData["WarningMessage"] = "This account uses Google login. Please use Google to log in.";
                return View("~/Views/Customer/Account/Login.cshtml");
            }

            // Validate the password
            if (user.password != password) // Replace with a secure password hash check
            {
                TempData["WarningMessage"] = "Invalid email or password.";
                return View("~/Views/Customer/Account/Login.cshtml");
            }

            // Create claims for the user
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim(ClaimTypes.Email, user.email),
                    new Claim(ClaimTypes.Role, user.role == "admin" ? "admin" : "user")
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true // Enable "Remember Me" functionality
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // Store user_id, username, email in session
            HttpContext.Session.SetInt32("user_id", user.user_id); // Store user_id in session
            HttpContext.Session.SetString("username", user.username); // Store username in session
            HttpContext.Session.SetString("email", user.email); // Store email in session

            // Retrieve customer information from the Customer table
            var customer = await _context.customers.FirstOrDefaultAsync(c => c.user_id == user.user_id);
            if (customer != null)
            {
                // Store customer_id in session
                HttpContext.Session.SetInt32("customer_id", customer.customer_id);
                HttpContext.Session.SetString("customer_name", customer.full_name); // Example of storing customer name
            }
            else
            {
                // Handle case when customer info is not found
                TempData["WarningMessage"] = "Customer information not found.";
            }

            // Redirect to home after successful login
            return RedirectToAction("Index", "Home");
        }

    }
}
