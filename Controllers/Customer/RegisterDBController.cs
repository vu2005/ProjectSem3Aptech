using CarInsuranceManage.Database;
using CarInsuranceManage.Models;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace CarInsuranceManage.Controllers.Customer
{
    public class RegisterDBController : Controller
    {
        private readonly CarInsuranceDbContext _context;

        public RegisterDBController(CarInsuranceDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string username, string password, string confirmPassword, string fullName, string phoneNumber, string address)
        {
            // Check if fields are empty
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["WarningMessage"] = "All fields are required.";
                return View("~/Views/Customer/Account/Register.cshtml");
            }

            // Check if passwords match
            if (password != confirmPassword)
            {
                TempData["WarningMessage"] = "Passwords do not match.";
                return View("~/Views/Customer/Account/Register.cshtml");
            }

            // Check if email already exists
            var existingUser = await _context.users.FirstOrDefaultAsync(u => u.email == email);
            if (existingUser != null)
            {
                TempData["WarningMessage"] = "Email already exists.";
                return View("~/Views/Customer/Account/Register.cshtml");
            }

            // Create new user
            var user = new User
            {
                username = username,
                email = email,
                password = password, // Ideally, hash the password before saving it
                created_at = DateTime.Now,
                user_logs = "Email",
                role = "user"
            };

            _context.users.Add(user);
            await _context.SaveChangesAsync();

            // After user registration, create a new customer entry with provided details
            var customer = new Models.Customer
            {
                user_id = user.user_id, // Use the newly created user's ID
                full_name = string.IsNullOrEmpty(fullName) ? "N/A" : fullName,  // Default to "N/A" if null or empty
                phone_number = string.IsNullOrEmpty(phoneNumber) ? "N/A" : phoneNumber,  // Default to "N/A" if null or empty
                address = string.IsNullOrEmpty(address) ? "N/A" : address   // Default to "N/A" if null or empty
            };

            _context.customers.Add(customer);
            await _context.SaveChangesAsync();

            // Display success message
            TempData["Message"] = "Registration successful. Please login.";
            return RedirectToAction("Login", "Account");
        }


    }
}
