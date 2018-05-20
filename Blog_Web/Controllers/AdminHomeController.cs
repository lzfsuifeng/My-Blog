using Blog_Web.Common;
using Blog_Web.Data;
using Blog_Web.Models;
using Blog_Web.ViewsModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Web.Controllers
{
    public class AdminHomeController : Controller
    {
        private readonly BlogContext blogContext;
        const string AdminName ="_Name";
        const string AdminImg = "_Img";
        const string  AdminID ="_ID" ;
        public AdminHomeController(BlogContext context)
        {
            blogContext = context;
        }
       
        //后台主页
        public async Task<IActionResult> Index(string sortOrder,string currentFilter, string searchString, int? page)
        {
            //从session中获取用户名和头像
            ViewData["Name"] = HttpContext.Session.GetString(AdminName);
            ViewData["Img"] = HttpContext.Session.GetString(AdminImg);
            
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

            var blogs = from a in blogContext.Blogs
                        join b in blogContext.Tallys on a.Tally_Id equals b.Tally_Id
                        select new AdminHome
                        {
                            ID=a.Blog_Id,
                            Title =a.Blog_Title,
                            Time=a.Blog_Time,
                            Digest=a.Blog_Digest,
                            TallyName=b.Tally_Name
                        };
            if (!String.IsNullOrEmpty(searchString))
            {
                blogs = blogs.Where(s => s.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    blogs = blogs.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    blogs = blogs.OrderBy(s => s.Time);
                    break;
                case "date_desc":
                    blogs = blogs.OrderByDescending(s => s.Time);
                    break;
                default:
                    blogs = blogs.OrderBy(s => s.Title);
                    break;
            }

            int pageSize = 6;
            return View(await PaginatedList<AdminHome>.CreatepagingAsync(blogs.AsNoTracking(), page ?? 1, pageSize));
        }
       
        
        #region  管理员登录

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string  Admin_Name,string  Admin_Password)
        {
           
             if (ModelState.IsValid)
             {
                var admin =await blogContext.Administrators.SingleOrDefaultAsync(u => u.Admin_Name == Admin_Name && u.Admin_Password == Admin_Password);             
                if (admin == null)
                     return NotFound();
                else
                {
                    //利用session存储用户名和用户头像
                    HttpContext.Session.SetString(AdminName, admin.Admin_Name);
                    HttpContext.Session.SetString(AdminImg, admin.Admin_Img);
                    HttpContext.Session.SetInt32(AdminID,admin.Admin_ID);

                    return RedirectToAction(nameof(Index));
                }
                    
             }
          
            return View();
        }
        #endregion

        #region 编辑个人信息

        public async Task<IActionResult> UpdateUser()
        {
            int id = HttpContext.Session.GetInt32(AdminID).Value;
            var administrator = await blogContext.Administrators.SingleOrDefaultAsync(m => m.Admin_ID == id);
            if (administrator == null)
                return NotFound();
            return View(administrator);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser([FromServices]IHostingEnvironment env, [Bind("Admin_Name,Admin_Img,Admin_Password,Admin_Email")] AdminViews administrator)
        {
            int id = HttpContext.Session.GetInt32(AdminID).Value;
            if (ModelState.IsValid)
            {
                string filename = string.Empty;
                if (administrator.Admin_Img != null)
                {
                    filename = Path.Combine("Files/UserImg", Guid.NewGuid().ToString() + Path.GetExtension(administrator.Admin_Img.FileName));
                    using (var stream = new FileStream(Path.Combine(env.WebRootPath, filename), FileMode.CreateNew))
                    {
                        administrator.Admin_Img.CopyTo(stream);
                    }
                }
                try
                {
                    Administrator updata = new Administrator
                    {
                        Admin_ID=id,
                        Admin_Img=filename,
                        Admin_Name=administrator.Admin_Name,
                        Admin_Email=administrator.Admin_Email,
                        Admin_Password=administrator.Admin_Password
                    };

                    blogContext.Update(updata);
                    await blogContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
          
            return View(administrator);
        }
        #endregion

        #region  管理员注册

        public IActionResult Registered()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registered([FromServices]IHostingEnvironment env, [Bind("Admin_Name,Admin_Email,Admin_Password,Admin_Img")] AdminViews administrator)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    string filename = string.Empty;
                    if (administrator.Admin_Img != null)
                    {
                        filename = Path.Combine("Files/UserImg", Guid.NewGuid().ToString() + Path.GetExtension(administrator.Admin_Img.FileName));
                        using (var stream = new FileStream(Path.Combine(env.WebRootPath, filename), FileMode.CreateNew))
                        {
                            administrator.Admin_Img.CopyTo(stream);
                        }
                    }
                    Administrator add = new Administrator
                    {
                        Admin_Img = filename,
                        Admin_Name = administrator.Admin_Name,
                        Admin_Email = administrator.Admin_Email,
                        Admin_Password = administrator.Admin_Password
                    };
                    blogContext.Add(add);
                    await blogContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Login));
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                   "Try again, and if the problem persists " +
                   "see your system administrator.");
                }
            }
            return View(administrator);
        }
        #endregion

        #region 发布个人博客
        [HttpGet]
        public IActionResult GreateBlog()
        {
            //从session中获取用户名和头像
            ViewData["Name"] = HttpContext.Session.GetString(AdminName);
            ViewData["Img"] = HttpContext.Session.GetString(AdminImg);
            //绑定下拉框                    
            ViewBag.InstructorId = new SelectList(blogContext.Tallys, "Tally_Id", "Tally_Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GreateBlog([FromServices]IHostingEnvironment env, ViewsBlog addblog)
        {           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string filename = string.Empty;
            if (addblog.Blog_Img != null)
            {
                filename = Path.Combine("Files", Guid.NewGuid().ToString() + Path.GetExtension(addblog.Blog_Img.FileName));
                using (var stream = new FileStream(Path.Combine(env.WebRootPath, filename), FileMode.CreateNew))
                {
                    addblog.Blog_Img.CopyTo(stream);
                }
            }
            blogContext.Add(new Blog
            {
                Blog_Title = addblog.Blog_Title,
                Blog_Digest=addblog.Blog_Digest,
                Blog_Context=addblog.Blog_Context,
                Tally_Id = addblog.Tally_Id,
                Blog_Time = DateTime.Now,
                Admin_Id = HttpContext.Session.GetInt32(AdminID).Value,              
                Blog_Img = filename
            });
            await blogContext.SaveChangesAsync();
            //绑定下拉框                    
            ViewBag.InstructorId = new SelectList(blogContext.Tallys, "Tally_Id", "Tally_Name", addblog.Tally_Id);
            return RedirectToAction("Index");         
        }

        #endregion

        #region 删除博客
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var blog = await blogContext.Blogs
                 .Include(c => c.Tally).AsNoTracking()
                .SingleOrDefaultAsync(m => m.Blog_Id == id);
            if (blog == null)
                return NotFound();

            return View(blog);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await blogContext.Blogs.SingleOrDefaultAsync(m => m.Blog_Id == id);
            blogContext.Blogs.Remove(blog);
            await blogContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region 修改博客
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var blog = await blogContext.Blogs
                .SingleOrDefaultAsync(m => m.Blog_Id == id);
            if (blog == null)
                return NotFound();
            PopulateDepartmentsDropDownList(blog.Tally_Id);
          
            return View(blog);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromServices]IHostingEnvironment env, EditBlog blog)
        {
            if (ModelState.IsValid)
            {
                string filename = string.Empty;
                if (blog.Blog_Img != null)
                {
                    filename = Path.Combine("usersImg", Guid.NewGuid().ToString() + Path.GetExtension(blog.Blog_Img.FileName));
                    using (var stream = new FileStream(Path.Combine(env.WebRootPath, filename), FileMode.CreateNew))
                    {
                        blog.Blog_Img.CopyTo(stream);
                    }
                }
                try
                {
                    Blog updataBlog = new Blog
                    {
                        Blog_Id = id,
                        Blog_Title = blog.Blog_Title,
                        Blog_Digest = blog.Blog_Digest,
                        Blog_Context =blog.Blog_Context,
                        Tally_Id = blog.Tally_Id,
                        Blog_Time = DateTime.Now,
                        Admin_Id=HttpContext.Session.GetInt32(AdminID).Value,
                        Blog_Img = filename
                    };

                    blogContext.Update(updataBlog);
                    await blogContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(blog.Blog_Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateDepartmentsDropDownList();
            return View(blog);
        }
        #endregion

        private bool CourseExists(int id)
        {
            return blogContext.Blogs.Any(e => e.Blog_Id == id);
        }

        #region 下拉菜单栏

        /// <summary>
        ///     部门信息的下拉菜单
        /// </summary>
        /// <param name="selectedDepartment"></param>
        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var tally = from d in blogContext.Tallys orderby d.Tally_Name select d;

            ViewBag.Tally = new SelectList(tally.AsNoTracking(), "Tally_Id", "Tally_Name", selectedDepartment);
        }

        #endregion


        //用户登录的分部视图
        public PartialViewResult Division()
        {
           
            return PartialView();
        }
    
    }
}