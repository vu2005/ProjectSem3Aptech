using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarInsuranceManage.Models
{
    public class User    {
        [Key]
        public int user_id { get; set; }

        [Required]
        [StringLength(50)]
        public string? username { get; set; } // Required field

        [Required]
        [StringLength(255)]
        public string? password { get; set; } // Required field

        [StringLength(100)]
        public string? full_name { get; set; } // Optional field

        [Required]
        [StringLength(100)]
        public string? email { get; set; } // Required field

        [StringLength(15)]
        public string? phone_number { get; set; } // Optional field

        [StringLength(255)]
        public string? address { get; set; } // Optional field

        [Required]
        public string? user_logs { get; set; } // Required field
                                               // New avatar field
        [StringLength(255)]
        public string? avatar { get; set; } // Avatar field to store the avatar image path or URL
        [Required]
        [StringLength(50)]
        public string? role { get; set; } // Role field to distinguish Customer or admin

        public DateTime created_at { get; set; } = DateTime.Now;
    }
}
