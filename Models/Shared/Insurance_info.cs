using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarInsuranceManage.Models
{
    public class InsuranceInfo
    {
        [Key]
        public int insurance_info_id { get; set; }

        [ForeignKey("Customer")]
        public int? customer_id { get; set; }

        public string? username { get; set; }

        public string? email { get; set; }

        public string? phone { get; set; }

        public string? car_brand { get; set; }

        public string? vehicle_line { get; set; }

        public string? year_of_manufacture { get; set; }

        public DateTime? registration_date { get; set; }

        public string? number_plate { get; set; }

        public string? frame_number { get; set; }

        public string? machine_number { get; set; }

        public DateTime? insurance_start_date { get; set; } = DateTime.Now;

        public string? insurance_package { get; set; }
        // Thêm trường insurance_code
    public string? insurance_code { get; set; }

        public decimal? insurance_price { get; set; }

        public DateTime? insurance_end_date 
        { 
            get
            {
                return insurance_start_date?.AddYears(1);
            }
            set
            {
                // Do nothing or add custom logic if needed
            }
        }

        public DateTime? created_at { get; set; } = DateTime.Now;

        public virtual Customer? Customer { get; set; }
    }
}
