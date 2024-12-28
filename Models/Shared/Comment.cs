using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarInsuranceManage.Models
{
    public class Comment
    {
        [Key]
        public int comment_id { get; set; }

        [ForeignKey("Customer")]
        public int customer_id { get; set; }

        [ForeignKey("ParentComment")]
        public int? parent_comment_id { get; set; } // Trường này để lưu mối quan hệ trả lời

        [Required]
        public string? comment_text { get; set; }

        public int? rating { get; set; }

        public DateTime created_at { get; set; } = DateTime.Now;

        [Required]
        public string? status { get; set; }  // 'Active' or 'Archived'

        public virtual Customer? Customer { get; set; }
        public virtual Comment? ParentComment { get; set; }  // Liên kết với bình luận gốc (nếu có)
        public virtual ICollection<Comment> Replies { get; set; } // Các bình luận trả lời (nếu có)
    }

}
