using BULKYRAZOR_TEMP.Data;
using BULKYRAZOR_TEMP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BULKYRAZOR_TEMP.Pages.Categories
{
    public class UpdateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public Category? Update { get; set; }
        public UpdateModel(ApplicationDbContext context)
        {
            _context = context;   
        }
        public void OnGet(int? Id)
        {
            if (Id != null && Id != 0)
            {
                 Update = _context.Categories.Find(Id);  
            }
        }
        public IActionResult OnPost() 
        {
            _context.Categories.Update(Update);
            _context.SaveChanges();
            TempData["success"] = "Category Update successfully";
            return RedirectToPage("Index");
        }
    }
}
