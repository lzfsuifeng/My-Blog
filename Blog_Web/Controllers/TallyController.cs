using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Web.Common;
using Blog_Web.Data;
using Microsoft.AspNetCore.Mvc;
using Blog_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog_Web.Controllers
{
    public class TallyController : Controller
    {
        private readonly BlogContext blogContext;
        public TallyController(BlogContext context)
        {
            blogContext = context;
        }
        //首页导航列表
        public PartialViewResult Menu()
        {          
            return PartialView();
        }
        #region 标签后台管理
        //展示所有标签
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var tallys = from Tally in blogContext.Tallys select Tally;
            int pageSize = 8;
            return View(await PaginatedList<Tally>.CreatepagingAsync(tallys.AsNoTracking(), page ?? 1, pageSize));
        }

        #region 删除标签
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
                return NotFound();

            var tally = await blogContext.Tallys.AsNoTracking()
                .SingleOrDefaultAsync(m => m.Tally_Id == id);
            if (tally == null)
                return NotFound();

            if (saveChangesError.GetValueOrDefault())
                ViewBag.SaveError = "删除失败。请再次尝试，如果尝试失败，请联系系统管理员。";

            return View(tally);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var tally = await blogContext.Tallys.SingleOrDefaultAsync(m => m.Tally_Id == id);
            blogContext.Tallys.Remove(tally);
            await blogContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region 添加标签
        
        public IActionResult Create()
        {
            return View();
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tally tally)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = new Tally
                    {
                        Tally_Name = tally.Tally_Name
                    };


                    blogContext.Add(entity);
                    await blogContext.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "无法进行数据的保存，请仔细检查你的数据，是否异常。");
            }

            return View(tally);
        }
        #endregion

        #region 编辑标签     
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var tally = await blogContext.Tallys.SingleOrDefaultAsync(m => m.Tally_Id == id);
            if (tally == null)
                return NotFound();
            return View(tally);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Tally_Id,Tally_Name")] Tally tally)
        {
            if (id != tally.Tally_Id)
                return NotFound();

            var entity = await blogContext.Tallys.SingleOrDefaultAsync(a => a.Tally_Id == id);

            if (await TryUpdateModelAsync(entity,"",s=>s.Tally_Name))
                try
                {
                    await blogContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "无法进行数据的保存，请仔细检查你的数据，是否异常。");
                }


            return View(tally);
        }
        #endregion
        #endregion
    }
}