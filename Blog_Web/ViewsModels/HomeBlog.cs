using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Web.ViewsModels
{
    public class HomeBlog
    {
        public int  Id { get; set; }
        public string Title { get; set; }
        public string Digest { get; set; }
        public string BlogImg { get; set; }
        public DateTime dateTime { get; set; }
        public string tallyName { get; set; }
    }
}
