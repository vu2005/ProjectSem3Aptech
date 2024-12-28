using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CarInsuranceManage.Database;
using CarInsuranceManage.Models;
using System.Net.Mail;


namespace CarInsuranceManage.Services
{
    public class InsuranceStatusService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public readonly PayPalService _payPalService;
        private readonly EmailService _emailService;
        private readonly ILogger<InsuranceStatusService> _logger;


        public InsuranceStatusService(IServiceScopeFactory scopeFactory, PayPalService payPalService, EmailService emailService, ILogger<InsuranceStatusService> logger)
        {
            _payPalService = payPalService;
            _emailService = emailService;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<CarInsuranceDbContext>();
                        var now = DateTime.UtcNow;


                        _logger.LogInformation("Bắt đầu lấy danh sách bảo hiểm sắp hết hạn trong 5 ngày tới.");


                        // Lấy danh sách bảo hiểm đã hết hạn
                        var expiringInsurances = await context.insurances_info
                            .Where(i => i.insurance_end_date <= now.AddDays(1)) //insurance_start_date đổi thành insurance_end_date
                            .ToListAsync(stoppingToken);


                        _logger.LogInformation($"Đã lấy được {expiringInsurances.Count} bảo hiểm sắp hết hạn.");


                        foreach (var insurance in expiringInsurances)
                        {
                            // Gửi email thông báo đến địa chỉ email cố định
                            _logger.LogInformation($"Đang gửi email thông báo hết hạn bảo hiểm tới {insurance.email} cho bảo hiểm {insurance.insurance_code}"); // đổi number_plate thành mã bảo hiểm (insurance_code)
                            await SendExpiryNotificationEmail(insurance.email, insurance);


                            // Copy to InsuranceHistory
                            var insuranceHistory = new History // trong bảng insurance_info có gì thì chỗ này có y hệt
                            {
                                customer_id = insurance.customer_id,
                                insurance_code = insurance.insurance_code,
                                username = insurance.username,
                                email = insurance.email,
                                phone = insurance.phone,
                                car_brand = insurance.car_brand,
                                vehicle_line = insurance.vehicle_line,
                                year_of_manufacture = insurance.year_of_manufacture,
                                registration_date = insurance.registration_date,
                                number_plate = insurance.number_plate,
                                frame_number = insurance.frame_number,
                                machine_number = insurance.machine_number,
                                insurance_start_date = insurance.insurance_start_date,
                                insurance_end_date = insurance.insurance_start_date,
                                insurance_package = insurance.insurance_package,
                                insurance_price = insurance.insurance_price

                            };


                            context.histories.Add(insuranceHistory);
                            // Delete related payment records
                            var relatedPayments = context.payments.Where(p => p.insurance_info_id == insurance.insurance_info_id);
                            context.payments.RemoveRange(relatedPayments);


                            // Remove the insurance record
                            context.insurances_info.Remove(insurance);
                        }


                        await context.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation("Cập nhật trạng thái bảo hiểm thành công.");
                    }


                    // Đợi một khoảng thời gian trước khi kiểm tra lại
                    await Task.Delay(TimeSpan.FromMinutes(0.5), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // Task was canceled, exit gracefully
                    _logger.LogInformation("Task was canceled.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Lỗi khi kiểm tra và cập nhật trạng thái bảo hiểm: {ex.Message}");
                }
            }
        }


        private async Task SendExpiryNotificationEmail(string email, InsuranceInfo insurance)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("insurancecarsore@gmail.com", "bfuv iniw uecz xgyl"),
                    EnableSsl = true,
                };


                var mailMessage = new MailMessage
                {
                    From = new MailAddress("insurancecarsore@gmail.com"),
                    Subject = "Insurance Expiry Notification",
                    Body = $"Dear {insurance.username},\n\nYour insurance with policy number {insurance.number_plate} is expiring soon.\n\nThank you.",
                    IsBodyHtml = false,
                };


                mailMessage.To.Add(email);


                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email thông báo hết hạn bảo hiểm đã được gửi tới {email}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi gửi email thông báo hết hạn bảo hiểm: {ex.Message}");
            }
        }
    }
}

