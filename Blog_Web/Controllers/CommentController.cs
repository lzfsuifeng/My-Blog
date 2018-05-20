using Blog_Web.Common;
using Blog_Web.Data;
using Blog_Web.Models;
using Blog_Web.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly BlogContext blogContext;
        public CommentController(BlogContext context)
        {
            blogContext = context;
        }

        #region 后台主页
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
           
            //排序
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var comments = from a in blogContext.Comments
                        join b in blogContext.Blogs on a.Blog_Id equals b.Blog_Id
                        select new ViewsComment
                        {
                           Blog_Title=b.Blog_Title,
                           Comment_Id=a.Comment_Id,
                           Comment_Time=a.Comment_Time,
                           Visitor=a.Visitor,
                           Email=a.Email
                        };
            if (!String.IsNullOrEmpty(searchString))
            {
                comments = comments.Where(s => s.Blog_Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    comments = comments.OrderByDescending(s => s.Visitor);
                    break;
                case "Date":
                    comments = comments.OrderBy(s => s.Comment_Time);
                    break;
                case "date_desc":
                    comments = comments.OrderByDescending(s => s.Comment_Time);
                    break;
                default:
                    comments = comments.OrderBy(s => s.Visitor);
                    break;
            }

            int pageSize = 6;
            return View(await PaginatedList<ViewsComment>.CreatepagingAsync(comments.AsNoTracking(), page ?? 1, pageSize));
        }
        #endregion

        #region 详情
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var comment = await blogContext.Comments
                .SingleOrDefaultAsync(m => m.Comment_Id == id);
            if (comment == null)
                return NotFound();
            return View(comment);
        }
        #endregion

        #region 删除
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
                return NotFound();

            var comment = await blogContext.Comments.AsNoTracking()
                .SingleOrDefaultAsync(m => m.Comment_Id == id);
            if (comment == null)
                return NotFound();

            if (saveChangesError.GetValueOrDefault())
                ViewBag.SaveError = "删除失败。请再次尝试，如果尝试失败，请联系系统管理员。";

            return View(comment);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await blogContext.Comments.SingleOrDefaultAsync(m => m.Comment_Id == id);
            blogContext.Comments.Remove(comment);
            await blogContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region 发表评论

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Greate([Bind("Blog_Id,Comment_Context,Visitor,Email")] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            comment.Comment_Time = DateTime.Now;
            //comment.Blog_Id = int.Parse(ViewData["BlogId"].ToString());
            blogContext.Add(comment);
            await blogContext.SaveChangesAsync();
            return RedirectToAction("ReadBlog", "Home", new { id=comment.Blog_Id});
        }
        #endregion

        public PartialViewResult Menu()
        {
            return PartialView();
        }


    }
}