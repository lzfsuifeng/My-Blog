using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Blog_Web.Models;

namespace Blog_Web.Models
{
    public class Blog
    {
        [Key]
        public int Blog_Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "标题不能为空.")]
        [Column("Blog_Title")]
        public string Blog_Title { get; set; }

        [Required]      
        public DateTime Blog_Time { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "摘要不能为空.")]
        [Column("Blog_Digest")]
        public string Blog_Digest { get; set; }

        [Required]        
        [Column("Blog_Context")]
        public string Blog_Context { get; set; }

        [Display(Name = "博客图片")]
        [Required(ErrorMessage = "请上传你的博客图片！")]     
        public string Blog_Img { get; set; }

        public int Tally_Id { get; set; }

        public int Admin_Id { get; set; }

        public ICollection<Comment> Comments { get; set; }
        
        public Tally Tally { get; set; }
        public Administrator Administrator { get; set; }

    }
}
