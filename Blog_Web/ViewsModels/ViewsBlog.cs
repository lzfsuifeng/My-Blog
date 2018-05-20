using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_Web.ViewsModels
{
    public class ViewsBlog
    {

        public int Blog_Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "标题不能为空.")]
        [Column("Blog_Title")]
        public string Blog_Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "摘要不能为空.")]
        [Column("Blog_Digest")]
        public string Blog_Digest { get; set; }

        [Required]
        [Column("Blog_Context")]
        public string Blog_Context { get; set; }

        [Display(Name = "博客图片")]
        [Required(ErrorMessage = "请上传你的博客图片！")]      
        public IFormFile Blog_Img { get; set; }

        [Required]
        [Column("Tally_Id")]
        public int Tally_Id { get; set; }
    }
}
