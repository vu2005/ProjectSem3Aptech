using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarInsuranceManage.Models
{
    public class InsuranceService
    {
        [Key]
        public int service_id { get; set; }  // Khóa chính

        [Required]
        [StringLength(100)]
        public string? service_name { get; set; }  // Tên dịch vụ bảo hiểm
        [ForeignKey("InsuranceInfo")]
        public int? insurance_info_id { get; set; }
        [Required]
        public string? description { get; set; }  // Mô tả dịch vụ bảo hiểm

        [Required]
        public decimal? price { get; set; }  // Giá của dịch vụ bảo hiểm

        [StringLength(255)]
        public string? image_url { get; set; }  // URL hình ảnh đại diện dịch vụ bảo hiểm

        public DateTime? created_at { get; set; } = DateTime.Now;  // Ngày tạo dịch vụ

        public DateTime? updated_at { get; set; }  // Ngày cập nhật dịch vụ

        public bool is_active { get; set; } = true;  // Trạng thái dịch vụ (Active or Inactive)
    }
}
