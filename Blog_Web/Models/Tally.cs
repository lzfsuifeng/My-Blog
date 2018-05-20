using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Web.Models
{
    public class Tally
    {
        [Key]
        public int Tally_Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Tally_Name { get; set; }

        public ICollection<Blog> Blogs { get; set; }
    }
}
