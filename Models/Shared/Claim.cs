using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarInsuranceManage.Models
{
    public class Claim
    {
        [Key]
        public int claim_id { get; set; }

        // Foreign Key to the Customer entity
        [ForeignKey("Customer")]
        public int? customer_id { get; set; }

        public string? customer_full_name { get; set; }

        public string? customer_email { get; set; }

        public string? customer_phone { get; set; }

        public string? description { get; set; }

        // Trường trạng thái
        public ClaimStatus status { get; set; } = ClaimStatus.Pending;

        public DateTime created_at { get; set; } = DateTime.Now;

        public virtual Customer? Customer { get; set; }
    }

    // Enum cho trạng thái của yêu cầu bồi thường
    public enum ClaimStatus
    {
        Pending,     // Đang xử lý
        Rejected,    // Từ chối
        Resolved     // Đã giải quyết
    }
}
