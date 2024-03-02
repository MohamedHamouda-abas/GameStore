using BULKYRAZOR_TEMP.Data;
using BULKYRAZOR_TEMP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BULKYRAZOR_TEMP.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public Category Create { get; set; }
        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
        }
        public IActionResult Onpost()
        {
            _context.Categories.Add(Create);
            _context.SaveChanges();
            TempData["success"] = "Category Create successfully";
            return RedirectToPage("Index");
        }
    }
}
