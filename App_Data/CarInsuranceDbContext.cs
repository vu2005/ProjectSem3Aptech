using Microsoft.EntityFrameworkCore;
using CarInsuranceManage.Models;
using System;

namespace CarInsuranceManage.Database
{
    public class CarInsuranceDbContext : DbContext
    {
        public CarInsuranceDbContext(DbContextOptions<CarInsuranceDbContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<Claim> claims { get; set; }
        public DbSet<CustomerSupportRequest> customer_support_requests { get; set; }
        public DbSet<Notification> notifications { get; set; }
        public DbSet<Contact> contacts { get; set; }
        public DbSet<InsuranceService> insurance_services { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<History> histories { get; set; }
        public DbSet<InsuranceInfo> insurances_info { get; set; }
        public object InsuranceInfos { get; internal set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data for User
            modelBuilder.Entity<User>().HasData(
                new User { user_id = 1, username = "admin", password = "admin123", full_name = "Admin User", email = "admin@gmail.com", phone_number = "1234567890", address = "123 Admin Street", avatar = "admin_avatar.jpg", role = "admin", user_logs = "", created_at = DateTime.Now },
                new User { user_id = 2, username = "user1", password = "user123", full_name = "User One", email = "vunnth2307024@gmail.com", phone_number = "1234567891", address = "123 User Street", avatar = "user1_avatar.jpg", role = "customer", user_logs = "", created_at = DateTime.Now },
                new User { user_id = 3, username = "user2", password = "user456", full_name = "User Two", email = "user2@example.com", phone_number = "1234567892", address = "123 Another Street", avatar = "user2_avatar.jpg", role = "customer", user_logs = "", created_at = DateTime.Now }
            );

            // Seed data for Customer
            modelBuilder.Entity<Customer>().HasData(
                new Customer { customer_id = 1, user_id = 1, full_name = "Admin User", phone_number = "1234567890", address = "123 Admin Street" },
                new Customer { customer_id = 2, user_id = 2, full_name = "User One", phone_number = "1234567891", address = "123 User Street" },
                new Customer { customer_id = 3, user_id = 3, full_name = "User Two", phone_number = "1234567892", address = "123 Another Street" }
            );

            // Seed data for Payment
            modelBuilder.Entity<Payment>().HasData(
                new Payment { payment_id = 1, customer_id = 1, insurance_info_id = 1, bill_number = "BILL001", payment_date = DateTime.Now.AddMonths(-1), payment_amount = 100.00m, transaction_id = "TXN001", payment_status = "Completed" },
                new Payment { payment_id = 2, customer_id = 2, insurance_info_id = 2, bill_number = "BILL002", payment_date = DateTime.Now.AddMonths(-2), payment_amount = 200.00m, transaction_id = "TXN002", payment_status = "Pending" },
                new Payment { payment_id = 3, customer_id = 3, insurance_info_id = 3, bill_number = "BILL003", payment_date = DateTime.Now.AddMonths(-3), payment_amount = 300.00m, transaction_id = "TXN003", payment_status = "Failed" }
            );

            // Seed data for Claim
            modelBuilder.Entity<Claim>().HasData(
                new Claim { claim_id = 1, customer_id = 1, customer_full_name = "Admin User", customer_email = "admin@example.com", customer_phone = "1234567890", description = "Claim for accident", status = ClaimStatus.Pending, created_at = DateTime.Now },
                new Claim { claim_id = 2, customer_id = 2, customer_full_name = "User One", customer_email = "user1@example.com", customer_phone = "1234567891", description = "Claim for windshield damage", status = ClaimStatus.Resolved, created_at = DateTime.Now },
                new Claim { claim_id = 3, customer_id = 3, customer_full_name = "User Two", customer_email = "user2@example.com", customer_phone = "1234567892", description = "Claim for theft", status = ClaimStatus.Rejected, created_at = DateTime.Now }
            );

            // Seed data for CustomerSupportRequest
            modelBuilder.Entity<CustomerSupportRequest>().HasData(
                new CustomerSupportRequest { support_id = 1, customer_id = 1, support_type = "General Inquiry", support_description = "Help with insurance details.", support_payment = "Free", support_status = "Pending", created_at = DateTime.Now },
                new CustomerSupportRequest { support_id = 2, customer_id = 2, support_type = "Claim Support", support_description = "Issue with a claim.", support_payment = "Paid", support_status = "Resolved", resolved_at = DateTime.Now, resolved_by = 1 },
                new CustomerSupportRequest { support_id = 3, customer_id = 3, support_type = "Policy Inquiry", support_description = "Renewal question.", support_payment = "Free", support_status = "Closed", created_at = DateTime.Now }
            );

            // Seed data for Notification
            modelBuilder.Entity<Notification>().HasData(
                new Notification { notification_id = 1, customer_id = 1, message_type = "Reminder", message_content = "Policy renewal reminder.", sent_at = DateTime.Now, is_read = false },
                new Notification { notification_id = 2, customer_id = 2, message_type = "Claim Update", message_content = "Your claim has been processed.", sent_at = DateTime.Now, is_read = true },
                new Notification { notification_id = 3, customer_id = 3, message_type = "Promotion", message_content = "Special discounts for renewals!", sent_at = DateTime.Now, is_read = false }
            );

            // Seed data for Contact
            modelBuilder.Entity<Contact>().HasData(
                new Contact { id = 1, customer_id = 1, full_name = "Admin User", email = "admin@example.com", phone = "1234567890", subject = "Policy Details", message = "Can I upgrade my policy?", status = true },
                new Contact { id = 2, customer_id = 2, full_name = "User One", email = "user1@example.com", phone = "1234567891", subject = "Claim Issue", message = "I need help with my claim.", status = true },
                new Contact { id = 3, customer_id = 3, full_name = "User Two", email = "user2@example.com", phone = "1234567892", subject = "General Inquiry", message = "I have a question about your services.", status = false }
            );

            // Dữ liệu mẫu cho bảng InsuranceService
            modelBuilder.Entity<InsuranceService>().HasData(
                new InsuranceService { service_id = 1, insurance_info_id = 1, service_name = "Moto Insurance", description = "Basic vehicle insurance", price = 50.00m, image_url = "customer-assets/uploads/product/moto.jpg", created_at = DateTime.Now },
                new InsuranceService { service_id = 2, insurance_info_id = 2, service_name = "Car Insurance", description = "Premium vehicle insurance with more benefits", price = 50.00m, image_url = "customer-assets/uploads/product/car.jpg", created_at = DateTime.Now },
                new InsuranceService { service_id = 3, insurance_info_id = 3, service_name = "Truck Insurance", description = "Comprehensive coverage for all types of damage", price = 50.00m, image_url = "customer-assets/uploads/product/truck.jpg", created_at = DateTime.Now }
            );
            // Seed data for Comment
            modelBuilder.Entity<Comment>().HasData(
                new Comment { comment_id = 1, customer_id = 1, parent_comment_id = null, comment_text = "Great service!", rating = 5, status = "Active", created_at = DateTime.Now },
                new Comment { comment_id = 2, customer_id = 2, parent_comment_id = 1, comment_text = "I agree, excellent support.", rating = 4, status = "Active", created_at = DateTime.Now },
                new Comment { comment_id = 3, customer_id = 3, parent_comment_id = null, comment_text = "Service was okay.", rating = 3, status = "Active", created_at = DateTime.Now }
            );

            // Seed data for History
            modelBuilder.Entity<History>().HasData(
                new History { history_id = 1, customer_id = 1, username = "admin", email = "admin@example.com", phone = "1234567890", car_brand = "Toyota", vehicle_line = "Corolla", year_of_manufacture = "2020", registration_date = DateTime.Now.AddMonths(-6), number_plate = "ABC123", frame_number = "FRAME001", machine_number = "MACHINE001", insurance_start_date = DateTime.Now.AddMonths(-6), insurance_end_date = DateTime.Now.AddMonths(6), insurance_package = "Basic Plan", insurance_code = "INS001", insurance_price = 500.00m, created_at = DateTime.Now },
                new History { history_id = 2, customer_id = 2, username = "user1", email = "user1@example.com", phone = "1234567891", car_brand = "Honda", vehicle_line = "Civic", year_of_manufacture = "2019", registration_date = DateTime.Now.AddMonths(-12), number_plate = "XYZ456", frame_number = "FRAME002", machine_number = "MACHINE002", insurance_start_date = DateTime.Now.AddMonths(-12), insurance_end_date = DateTime.Now.AddMonths(-6), insurance_package = "Comprehensive Plan", insurance_code = "INS002", insurance_price = 700.00m, created_at = DateTime.Now },
                new History { history_id = 3, customer_id = 3, username = "user2", email = "user2@example.com", phone = "1234567892", car_brand = "Ford", vehicle_line = "Focus", year_of_manufacture = "2021", registration_date = DateTime.Now.AddMonths(-3), number_plate = "DEF789", frame_number = "FRAME003", machine_number = "MACHINE003", insurance_start_date = DateTime.Now.AddMonths(-3), insurance_end_date = DateTime.Now.AddMonths(9), insurance_package = "Premium Plan", insurance_code = "INS003", insurance_price = 900.00m, created_at = DateTime.Now }
            );

            // Seed data for InsuranceInfo
            modelBuilder.Entity<InsuranceInfo>().HasData(
                new InsuranceInfo { insurance_info_id = 1, customer_id = 1, username = "admin", email = "admin@example.com", phone = "1234567890", car_brand = "Toyota", vehicle_line = "Corolla", year_of_manufacture = "2020", registration_date = DateTime.Now, number_plate = "ABC123", frame_number = "FRAME001", machine_number = "MACHINE001", insurance_start_date = DateTime.Now, insurance_end_date = null, insurance_package = "Basic Plan", insurance_price = 500.00m, created_at = DateTime.Now },
                new InsuranceInfo { insurance_info_id = 2, customer_id = 2, username = "user1", email = "user1@example.com", phone = "1234567891", car_brand = "Honda", vehicle_line = "Civic", year_of_manufacture = "2019", registration_date = DateTime.Now, number_plate = "XYZ456", frame_number = "FRAME002", machine_number = "MACHINE002", insurance_start_date = DateTime.Now, insurance_end_date = null, insurance_package = "Comprehensive Plan", insurance_price = 700.00m, created_at = DateTime.Now },
                new InsuranceInfo { insurance_info_id = 3, customer_id = 3, username = "user2", email = "user2@example.com", phone = "1234567892", car_brand = "Ford", vehicle_line = "Focus", year_of_manufacture = "2021", registration_date = DateTime.Now, number_plate = "DEF789", frame_number = "FRAME003", machine_number = "MACHINE003", insurance_start_date = DateTime.Now, insurance_end_date = null, insurance_package = "Premium Plan", insurance_price = 900.00m, created_at = DateTime.Now }
            );
        }
    }

}
