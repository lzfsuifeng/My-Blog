using Blog_Web.Common;
using Blog_Web.Data;
using Blog_Web.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogContext blogContext;
        public HomeController(BlogContext context)
        {
            blogContext = context;
        }
        //首页
        public async Task<IActionResult> Index(string blogtitle, string sortOrder,string currentFilter,string searchString, int ?page)
        {
            MenuList();
            ViewData["tag"] = "博客";
            ViewData["CurrentSort"] = sortOrder;

            //搜索的关键字
            ViewData["blogtitle"] = blogtitle;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var blogs =(from a in blogContext.Blogs
                        join c in blogContext.Tallys on a.Tally_Id equals c.Tally_Id
                        select new HomeBlog
                        {
                            Title=a.Blog_Title,
                            dateTime=a.Blog_Time,
                            BlogImg= a.Blog_Img,
                            tallyName=c.Tally_Name,
                            Digest=a.Blog_Digest,
                            Id=a.Blog_Id
                        });
            //模糊搜索
            if (!string.IsNullOrWhiteSpace(blogtitle))
                blogs = blogs.Where(a => a.Title.Contains(blogtitle));
            //每页显示数量
            int pageSize = 4;
            return View(await PaginatedList<HomeBlog>.CreatepagingAsync(blogs.AsNoTracking(), page ?? 1, pageSize));
        }

        #region 浏览博客
        public async Task<IActionResult> ReadBlog(int ?id)
        {
            MenuList();
            ViewData["tag"] = "阅读博客";
            //查询该博客下的评论
            var comments = from a in blogContext.Comments
                           where a.Blog_Id == id
                           select a;
            ViewBag.comments = comments;
            //查询该博客下的评论数量
            int num = comments.Count();
            ViewBag.num = num;
            var blog = await blogContext.Blogs
                .SingleOrDefaultAsync(m => m.Blog_Id == id);
            if (blog == null)
                return NotFound();              
            return View(blog);
        }

        #endregion
     

        public IActionResult About()
        {

            ViewData["tag"] = "关于博主";
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        

        //绑定右部导航栏数据
        private void MenuList()
        {
            var tallys = (blogContext.Tallys.Distinct().OrderBy(x => x.Tally_Id)).Take(5);
            ViewBag.tallysList = tallys;
            var comments = (blogContext.Comments.Distinct().OrderBy(x => x.Comment_Id)).Take(5);
            ViewBag.commentsList = comments;
            var blogs = (blogContext.Blogs.Distinct().OrderBy(x => x.Blog_Id)).Take(5);
            ViewBag.blogsList = blogs;

        }

     
    }
}
