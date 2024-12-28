using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarInsuranceManage.Models
{
    public class CustomerSupportRequest
    {
        [Key]
        public int support_id { get; set; }

        [ForeignKey("Customer")]
        public int customer_id { get; set; }
        [Required]
        [StringLength(100)]
        public string? support_type { get; set; }
        [Required]
        public string? support_description { get; set; }
        [Required]
        public string? support_payment { get; set; }
        [Required]
        public string? support_status { get; set; }  // 'Open', 'Closed', 'Pending'

        public DateTime created_at { get; set; } = DateTime.Now;
        public DateTime? resolved_at { get; set; }

        [ForeignKey("User")]
        public int? resolved_by { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual User? User { get; set; }
    }

}
