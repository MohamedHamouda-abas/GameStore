using BULKYRAZOR_TEMP.Data;
using BULKYRAZOR_TEMP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BULKYRAZOR_TEMP.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public Category? Delete { get; set; }
        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet(int? Id)
        {
            if (Id != null && Id != 0)
            {
                Delete = _context.Categories.Find(Id);
            }
        }
        public IActionResult OnPost()
        {
            Category? obj = _context.Categories.Find(Delete.Id);
            if (obj == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(obj);
            _context.SaveChanges();
            TempData["success"] = "Category Delete successfully";
            return RedirectToPage("Index");
        }
    }
}
