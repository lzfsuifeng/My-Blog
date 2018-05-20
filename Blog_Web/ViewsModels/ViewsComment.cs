using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Web.ViewsModels
{
    public class ViewsComment
    {

        public int Comment_Id { get; set; }      
        
        public DateTime Comment_Time { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "游客昵称不能为空.")]
        public string Visitor { get; set; }

        [Required]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "邮件格式不正确")]
        public string Email { get; set; }

        [Required]
        public string Blog_Title { get; set; }
    }
}
