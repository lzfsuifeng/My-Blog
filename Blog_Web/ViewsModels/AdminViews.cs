using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Web.ViewsModels
{
    public class AdminViews
    {
        [Required]
        [StringLength(50, ErrorMessage = "用户名不能为空.")]
        [Column("Admin_Name")]
        public string Admin_Name { get; set; }

        public Microsoft.AspNetCore.Http.IFormFile Admin_Img { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "{0}必须至少包含{2}个字符")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Admin_Password { get; set; }

        [Required]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "邮件格式不正确")]
        public string Admin_Email { get; set; }
    }
}
