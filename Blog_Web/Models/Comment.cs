using System;
using System.ComponentModel.DataAnnotations;

namespace Blog_Web.Models
{
    public class Comment
    {
        [Key]
        public int Comment_Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "内容不能为空.")]
        public string Comment_Context { get; set; }

        [Required]
        public DateTime Comment_Time { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "游客昵称不能为空.")]
        public string Visitor { get; set; }

        [Required]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "邮件格式不正确")]
        public string Email { get; set; }

        [Required]
        public int Blog_Id { get; set; }
    }
}
