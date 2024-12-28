using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarInsuranceManage.Models
{
    public class Notification
    {
        [Key]
        public int notification_id { get; set; }

        [ForeignKey("Customer")]
        public int customer_id { get; set; }

        [Required]
        [StringLength(100)]
        public string? message_type { get; set; }
        [Required]
        public string? message_content { get; set; }

        public DateTime sent_at { get; set; } = DateTime.Now;

        public bool is_read { get; set; } = false;

        public virtual Customer? Customer { get; set; }
    }

}
