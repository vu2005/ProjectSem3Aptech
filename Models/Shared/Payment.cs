using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarInsuranceManage.Models
{
    public class Payment
    {
        [Key]
        public int payment_id { get; set; }

        [ForeignKey("Customer")]
        public int customer_id { get; set; }

        [ForeignKey("InsuranceInfo")]
        public int? insurance_info_id { get; set; }

        [Required]
        [StringLength(50)]
        public string? bill_number { get; set; }

        public DateTime? payment_date { get; set; }

        public decimal? payment_amount { get; set; }

        // Thêm trường transaction_id và payment_status
        [StringLength(100)]
        public string? transaction_id { get; set; }  // Mã giao dịch từ Momo

        [Required]
        [StringLength(50)]
        public string? payment_status { get; set; }  // Trạng thái thanh toán

        public virtual Customer? Customer { get; set; }
        public virtual InsuranceInfo? InsuranceInfo { get; set; }
    }
}
