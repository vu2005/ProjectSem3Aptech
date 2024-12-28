using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarInsuranceManage.Models
{
    public class Contact
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("Customer")]
        public int customer_id { get; set; }

        [Required]
        [StringLength(255)]
        public string? full_name { get; set; }

        [Required]
        [StringLength(255)]
        public string? email { get; set; }

        [Required]
        [StringLength(20)]
        public string? phone { get; set; }

        [Required]
        [StringLength(255)]
        public string? subject { get; set; }

        [Required]
        public string? message { get; set; }

        public DateTime date_added { get; set; } = DateTime.Now;
        public DateTime date_modified { get; set; } = DateTime.Now;

        public bool status { get; set; }  // Active or Inactive

        public virtual Customer ?Customer { get; set; }
    }

}
