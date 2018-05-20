using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Web.Models
{
    public class Contact
    {
        [Key]
        public int Contact_Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "内容不能为空.")]
        public string Message { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "姓名不能为空.")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "邮件格式不正确")]
        public string Email { get; set; }
    }
}
