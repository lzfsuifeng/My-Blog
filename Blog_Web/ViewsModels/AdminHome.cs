using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Web.ViewsModels
{
    public class AdminHome
    {
        public int ID { get; set; }
        public string Title { get; set; }      
        public string Digest { get; set; }
        public string BlogImg { get; set; }
        public DateTime Time { get; set; }
        public string TallyName { get; set; }
    }
}
