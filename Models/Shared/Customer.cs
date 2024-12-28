using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarInsuranceManage.Models
{
    public class Customer
    {
        [Key]
        public int customer_id { get; set; }

        [ForeignKey("User")]
        public int user_id { get; set; }

        [Required]
        [StringLength(100)]
        public string? full_name { get; set; }
        [Required]
        [StringLength(15)]
        public string? phone_number { get; set; }
        [Required]
        [StringLength(255)]
        public string? address { get; set; }

        public virtual User? User { get; set; }
    }
}
