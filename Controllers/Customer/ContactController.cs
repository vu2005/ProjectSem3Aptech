using Microsoft.AspNetCore.Mvc;
using CarInsuranceManage.Models;
using CarInsuranceManage.Database;
using Microsoft.EntityFrameworkCore; // for ToListAsync
using System;
using System.Security.Claims;
using System.Threading.Tasks; // for async/await
using Microsoft.AspNetCore.Http; // for session management

namespace CarInsuranceManage.Controllers
{
    public class ContactController : Controller
    {
        private readonly CarInsuranceDbContext _context;

        // Constructor to inject CarInsuranceDbContext
        public ContactController(CarInsuranceDbContext context)
        {
            _context = context;
        }

        // GET: /Contact/Index
        [HttpGet]
        public async Task<IActionResult> Index()  // Make it async to use await
        { // Fetch active services for the view
            var services = await _context.insurance_services
                .ToListAsync();
            ViewData["Services"] = services;

            return View("~/Views/Customer/Contact/Index.cshtml");
        }

        // POST: /Contact/Send
        [HttpPost]
        public IActionResult Send(Contact contact)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Store a message in TempData before redirecting to login
                TempData["ErrorMessage"] = "You must be logged in to submit a contact form.";
                return RedirectToAction("Login", "Account"); // Redirect to login page
            }

            // Get the customer_id from the session
            var customerId = HttpContext.Session.GetInt32("customer_id");

            // Validate the form data
            if (ModelState.IsValid)
            {
                // Check if the customerId is null
                if (customerId == null)
                {
                    TempData["ErrorMessage"] = "Customer information is missing.";
                    return RedirectToAction("Login", "Account");
                }

                // Assign the customer_id to the contact object
                contact.customer_id = customerId.Value;

                // Set the status and date fields
                contact.status = true;
                contact.date_added = DateTime.Now;
                contact.date_modified = DateTime.Now;

                // Add the contact to the database and save changes
                _context.contacts.Add(contact);
                _context.SaveChanges();

                // Store a success message in TempData
                TempData["SuccessMessage"] = "Your message has been successfully sent!";

                // Redirect back to the Contact Index page
                return RedirectToAction("Index", "Contact");
            }

            // If the form is not valid, stay on the contact page and show validation errors
            TempData["ErrorMessage"] = "There was an issue with your form submission. Please check the inputs.";
            return View("~/Views/Customer/Contact/Index.cshtml", contact);
        }
    }
}
