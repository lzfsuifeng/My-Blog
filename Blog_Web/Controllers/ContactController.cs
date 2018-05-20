using Blog_Web.Common;
using Blog_Web.Data;
using Blog_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly BlogContext blogContext;
        public ContactController(BlogContext context)
        {
            blogContext = context;
        }
        //后台主页
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

            var contacts = from a in blogContext.Contacts
                           select a;
            if (!String.IsNullOrEmpty(searchString))
            {
                contacts = contacts.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    contacts = contacts.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    contacts = contacts.OrderBy(s => s.Time);
                    break;
                case "date_desc":
                    contacts = contacts.OrderByDescending(s => s.Time);
                    break;
                default:
                    contacts = contacts.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 6;
            return View(await PaginatedList<Contact>.CreatepagingAsync(contacts.AsNoTracking(), page ?? 1, pageSize));
        }

        #region 联系博主
        public IActionResult Contact()
        {
            ViewData["tag"] = "联系博主";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact([Bind("Name,Email,Message")] Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            contact.Time = DateTime.Now;
            blogContext.Add(contact);
            await blogContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region 私信详情
        public async Task<IActionResult> Details(int ? id)
        {
            if (id == null)
                return NotFound();

            var contact = await blogContext.Contacts
                .SingleOrDefaultAsync(m => m.Contact_Id == id);
            if (contact == null)
                return NotFound();           
            return View(contact);
        }
        #endregion

        #region 删除私信
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
                return NotFound();

            var contact = await blogContext.Contacts.AsNoTracking()
                .SingleOrDefaultAsync(m => m.Contact_Id == id);
            if (contact == null)
                return NotFound();

            if (saveChangesError.GetValueOrDefault())
                ViewBag.SaveError = "删除失败。请再次尝试，如果尝试失败，请联系系统管理员。";

            return View(contact);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await blogContext.Contacts.SingleOrDefaultAsync(m => m.Contact_Id == id);
            blogContext.Contacts.Remove(contact);
            await blogContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}