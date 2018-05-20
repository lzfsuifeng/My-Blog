using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog_Web.Data;
using Microsoft.AspNetCore.Http;
using Blog_Web.Models;

namespace Blog_Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogContext blogContext;
        public BlogController(BlogContext context)
        {
            blogContext = context;
        }

        #region 首页最新列表
       
        public PartialViewResult Menu()
        {
            
            return PartialView();
        }
        #endregion

      


    }
}