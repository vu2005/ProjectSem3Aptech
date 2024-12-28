using Microsoft.AspNetCore.Mvc;
using CarInsuranceManage.Services;
using CarInsuranceManage.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using CarInsuranceManage.Database;
using System.Globalization;
using System.IO;


namespace CarInsuranceManage.Controllers
{
    public class PayPalController : Controller
    {
        private readonly PayPalService _payPalService;


        private readonly EmailService _emailService;
        private readonly CarInsuranceDbContext _dbContext;


        public PayPalController(PayPalService payPalService, CarInsuranceDbContext dbContext, EmailService emailService)
        {
            _payPalService = payPalService;
            _dbContext = dbContext;


            _emailService = emailService;
        }


        [HttpGet]
        [Route("insurance/car_insurance/{id}")]
        public IActionResult Checkout(int id)
        {
            // Use 'id' to retrieve package details or any necessary information
            var package = _dbContext.insurance_services.Find(id);
            if (package == null)
            {
                return NotFound("Insurance package not found.");
            }


            ViewBag.Package = package;
            return View("~/Views/Customer/Insurance/Car_Insurance.cshtml");
        }


        [HttpPost]
        public async Task<IActionResult> Checkout(decimal insurance_price, string description, InsuranceInfo insuranceDetails)
        {
            // Ghi log dữ liệu nhận được từ form
            Console.WriteLine("Received data from form:");
            Console.WriteLine($"Insurance Price: {insurance_price}");
            Console.WriteLine($"Description: {description}");
            Console.WriteLine($"Insurance Details: {JsonConvert.SerializeObject(insuranceDetails)}");


            string insuranceData = JsonConvert.SerializeObject(insuranceDetails);
            string returnUrl = Url.Action("Success", "PayPal", new { insuranceData }, Request.Scheme);
            string cancelUrl = Url.Action("Cancel", "PayPal", null, Request.Scheme);


            try
            {
                // Create payment
                var approvalUrl = await _payPalService.CreatePayment(insurance_price, "USD", description, returnUrl, cancelUrl);


                return Redirect(approvalUrl);
            }
            catch (Exception ex)
            {
                // Handle error and display message
                TempData["WarningMessage"] = "Có lỗi xảy ra khi tạo thanh toán: " + ex.Message;
                return RedirectToAction("Login", "Account");
            }
        }


        [HttpGet]
        public async Task<IActionResult> SuccessAlternative(string paymentId, string PayerID, string insuranceData)
        {
            // Ghi log dữ liệu nhận được từ URL
            Console.WriteLine("Received data from URL:");
            Console.WriteLine($"Payment ID: {paymentId}");
            Console.WriteLine($"Payer ID: {PayerID}");
            Console.WriteLine($"Insurance Data: {insuranceData}");


            // Execute PayPal payment
            var result = await _payPalService.ExecutePayment(paymentId, PayerID);


            // Retrieve customer_id from session
            var customerId = HttpContext.Session.GetInt32("customer_id");
            if (customerId == null)
            {
                TempData["WarningMessage"] = "Vui lòng đăng nhập để thực hiện.";
                return RedirectToAction("Login", "Account");
            }


            // Kiểm tra insuranceData
            InsuranceInfo insuranceDetails = null;
            if (!string.IsNullOrEmpty(insuranceData))
            {
                try
                {
                    insuranceDetails = JsonConvert.DeserializeObject<InsuranceInfo>(insuranceData);
                }
                catch
                {
                    insuranceDetails = new InsuranceInfo(); // Gán đối tượng trống nếu lỗi
                }
            }
            else
            {
                insuranceDetails = new InsuranceInfo(); // Gán đối tượng trống nếu null
            }


            // Lưu dữ liệu JSON vào file
            var jsonData = JsonConvert.SerializeObject(insuranceDetails);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UserData.json");
            await System.IO.File.WriteAllTextAsync(filePath, jsonData);


            // Tạo giá trị ngẫu nhiên cho insurance_code
            string insuranceCode = GenerateUniqueCode();


            // Save insurance record to the database
            var insuranceRecord = new InsuranceInfo
            {
                customer_id = customerId.Value,
                username = insuranceDetails?.username ?? null,
                email = insuranceDetails?.email ?? null,
                phone = insuranceDetails?.phone ?? null,
                car_brand = insuranceDetails?.car_brand ?? null,
                vehicle_line = insuranceDetails?.vehicle_line ?? null,
                year_of_manufacture = insuranceDetails?.year_of_manufacture ?? null,
                registration_date = insuranceDetails?.registration_date ?? null,
                number_plate = insuranceDetails?.number_plate ?? null,
                frame_number = insuranceDetails?.frame_number ?? null,
                machine_number = insuranceDetails?.machine_number ?? null,
                insurance_package = insuranceDetails?.insurance_package ?? null,
                insurance_price = insuranceDetails?.insurance_price ?? 0, // Giá trị mặc định là 0
                insurance_code = insuranceCode,


                created_at = DateTime.Now
            };


            _dbContext.insurances_info.Add(insuranceRecord);
            await _dbContext.SaveChangesAsync();


            // Gửi email thông báo bảo hiểm mới
            await _emailService.SendEmailAsync(insuranceRecord.username, insuranceRecord.email, "New Insurance Record Created",
                $"Dear {insuranceRecord.username},\n\n" +
                $"Your insurance record has been created successfully.\n" +
                $"Insurance Code: {insuranceRecord.insurance_code}\n" +
                $"Insurance Package: {insuranceRecord.insurance_package}\n" +
                $"Insurance Start Date: {insuranceRecord.insurance_start_date}\n" +
                $"Insurance End Date: {insuranceRecord.insurance_end_date}\n\n" +
                "Thank you for choosing our service.\n\n" +
                "Best regards,\n" +
                "Car Insurance Team");




            // Create and save notification
            var notification = new Notification
            {
                customer_id = customerId.Value,
                message_type = "Payment Success",
                message_content = $"Your payment for the {insuranceDetails?.insurance_package ?? "Unknown"} package was successful. Amount: ${insuranceDetails?.insurance_price ?? 0:F2}.",
                sent_at = DateTime.Now,
                is_read = false
            };


            _dbContext.notifications.Add(notification);
            await _dbContext.SaveChangesAsync();

            // Create and save notification
            var Payment = new Payment
            {
                
            };


            _dbContext.notifications.Add(notification);
            await _dbContext.SaveChangesAsync();

            ViewData["Result"] = result;
            return View("~/Views/Customer/Payment/PaymentSuccess.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> Success(string paymentId, string PayerID, string insuranceData)
        {
            // Execute PayPal payment
            var result = await _payPalService.ExecutePayment(paymentId, PayerID);


            // Retrieve customer_id from session
            var customerId = HttpContext.Session.GetInt32("customer_id");
            if (customerId == null)
            {
                TempData["WarningMessage"] = "Vui lòng đăng nhập để thực hiện.";
                return RedirectToAction("Login", "Account");
            }


            // Kiểm tra insuranceData
            InsuranceInfo insuranceDetails = null;
            if (!string.IsNullOrEmpty(insuranceData))
            {
                try
                {
                    insuranceDetails = JsonConvert.DeserializeObject<InsuranceInfo>(insuranceData);
                }
                catch
                {
                    insuranceDetails = new InsuranceInfo(); // Gán đối tượng trống nếu lỗi
                }
            }
            else
            {
                insuranceDetails = new InsuranceInfo(); // Gán đối tượng trống nếu null
            }

            // Lưu dữ liệu JSON vào file
            var jsonData = JsonConvert.SerializeObject(insuranceDetails);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UserData.json");
            await System.IO.File.WriteAllTextAsync(filePath, jsonData);


            // Tính toán insurance_end_date
            DateTime? insuranceEndDate = insuranceDetails.insurance_start_date?.AddYears(1);


            // Tạo giá trị ngẫu nhiên cho insurance_code
            string insuranceCode = GenerateUniqueCode();


            // Save insurance record to the database
            var insuranceRecord = new InsuranceInfo
            {
                customer_id = customerId.Value,
                username = insuranceDetails?.username ?? null,
                email = insuranceDetails?.email ?? null,
                phone = insuranceDetails?.phone ?? null,
                car_brand = insuranceDetails?.car_brand ?? null,
                vehicle_line = insuranceDetails?.vehicle_line ?? null,
                year_of_manufacture = insuranceDetails?.year_of_manufacture ?? null,
                registration_date = insuranceDetails?.registration_date ?? null,
                number_plate = insuranceDetails?.number_plate ?? null,
                frame_number = insuranceDetails?.frame_number ?? null,
                machine_number = insuranceDetails?.machine_number ?? null,
                insurance_package = insuranceDetails?.insurance_package ?? null,
                insurance_price = insuranceDetails?.insurance_price ?? 0, // Giá trị mặc định là 0
                insurance_start_date = insuranceDetails.insurance_start_date,
                insurance_end_date = insuranceEndDate,
                insurance_code = insuranceCode,
                created_at = DateTime.Now
            };


            _dbContext.insurances_info.Add(insuranceRecord);
            await _dbContext.SaveChangesAsync();


            // Gửi email thông báo bảo hiểm mới
            var subject = "New Insurance Record Created";
            var body = $"Dear {insuranceRecord.username},\n\n" +
                       $"Your insurance record has been created successfully.\n" +
                       $"Insurance Code: {insuranceRecord.insurance_code}\n" +
                       $"Insurance Package: {insuranceRecord.insurance_package}\n" +
                       $"Insurance Start Date: {insuranceRecord.insurance_start_date}\n" +
                       $"Insurance End Date: {insuranceRecord.insurance_end_date}\n\n" +
                       "Thank you for choosing our service.\n\n" +
                       "Best regards,\n" +
                       "Car Insurance Team";


            await _emailService.SendEmailAsync(insuranceRecord.username, insuranceRecord.email, subject, body);


            // Create and save notification
            var notification = new Notification
            {
                customer_id = customerId.Value,
                message_type = "Payment Success",
                message_content = $"Your payment for the {insuranceDetails?.insurance_package ?? "Unknown"} package was successful. Amount: ${insuranceDetails?.insurance_price ?? 0:F2}.",
                sent_at = DateTime.Now,
                is_read = false
            };


            _dbContext.notifications.Add(notification);
            await _dbContext.SaveChangesAsync();


            ViewData["Result"] = result;
            return View("~/Views/Customer/Payment/PaymentSuccess.cshtml", insuranceRecord);
        }


        // Hàm tạo mã ngẫu nhiên không trùng lặp
        private string GenerateUniqueCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            var random = new Random();


            // Tạo 4 ký tự chữ cái ngẫu nhiên
            var charPart = new char[4];
            for (int i = 0; i < charPart.Length; i++)
            {
                charPart[i] = chars[random.Next(chars.Length)];
            }


            // Tạo 5 ký tự số ngẫu nhiên
            var digitPart = new char[5];
            for (int i = 0; i < digitPart.Length; i++)
            {
                digitPart[i] = digits[random.Next(digits.Length)];
            }


            // Kết hợp hai phần lại
            return new string(charPart) + new string(digitPart);
        }



        [HttpGet]
        public IActionResult Cancel()
        {
            return View("~/Views/Customer/Payment/PaymentCancel.cshtml");
        }
    }
}

