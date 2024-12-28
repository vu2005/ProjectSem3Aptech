using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using CarInsuranceManage.Database;
using CarInsuranceManage.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System;

namespace CarInsuranceManage.Controllers.Customer
{
    public class LoginPhoneController : Controller
    {
        private readonly CarInsuranceDbContext _context;
        private readonly string _twilioAccountSid;
        private readonly string _twilioAuthToken;
        private readonly string _twilioPhoneNumber;

        public LoginPhoneController(IConfiguration configuration, CarInsuranceDbContext context)
        {
            _twilioAccountSid = configuration["Twilio:AccountSid"];
            _twilioAuthToken = configuration["Twilio:AuthToken"];
            _twilioPhoneNumber = configuration["Twilio:PhoneNumber"];
            _context = context;
        }

        [HttpPost]
        public IActionResult SendOtp(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                TempData["WarningMessage"] = "Phone number is required.";
                return RedirectToAction("Login_phone", "Account");
            }

            // Thêm mã quốc gia nếu chưa có
            if (!phone.StartsWith("+"))
            {
                phone = "+84" + phone; // Giả sử bạn đang làm việc với số điện thoại ở Việt Nam
            }

            try
            {
                var otp = new Random().Next(100000, 999999).ToString();
                HttpContext.Session.SetString("OTP", otp);
                HttpContext.Session.SetString("PhoneNumber", phone);

                TwilioClient.Init(_twilioAccountSid, _twilioAuthToken);
                var message = MessageResource.Create(
                    body: $"Your OTP code is: {otp}",
                    from: new PhoneNumber(_twilioPhoneNumber),
                    to: new PhoneNumber(phone)
                );

                TempData["SuccessMessage"] = "OTP sent successfully. Please check your phone.";
                return RedirectToAction("Verify_phone", "Account");
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("Login_phone", "Account");
            }
        }


        [HttpPost]
        public IActionResult VerifyOtp(string otp)
        {
            var storedOtp = HttpContext.Session.GetString("OTP");
            var storedPhoneNumber = HttpContext.Session.GetString("PhoneNumber");

            if (storedOtp == null || storedPhoneNumber == null)
            {
                TempData["WarningMessage"] = "Session expired. Please try again.";
                return RedirectToAction("Login_phone", "Account");
            }

            if (otp == storedOtp)
            {
                var user = new User
                {
                    phone_number = storedPhoneNumber,
                    username = "user" + storedPhoneNumber.Substring(storedPhoneNumber.Length - 4), // For example
                    password = "N/A",
                    email = "N/A",
                    user_logs = "phone",
                    role = "user",
                    created_at = DateTime.Now
                };

                _context.users.Add(user);
                _context.SaveChanges();


                TempData["SuccessMessage"] = "User account created successfully and logged in!";
                return RedirectToAction("Profile", "Account");
            }
            else
            {
                TempData["WarningMessage"] = "Invalid OTP. Please try again.";
                return RedirectToAction("Verify_phone", "Account");
            }
        }
    }
}
