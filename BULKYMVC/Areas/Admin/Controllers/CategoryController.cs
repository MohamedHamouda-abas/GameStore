using BULKY.DataAccess.Data;
using BULKY.DataAccess.Repository;
using BULKY.DataAccess.Repository.IRepository;
using BULKY.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace BULKYMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {

            List<Category> GetAll = _unitOfWork.Category.GetAll().ToList();
            return View(GetAll);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(model);
                _unitOfWork.Save();
                TempData["success"] = "Category Create successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var category = _unitOfWork.Category.Get(c => c.Id == Id);
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(model);
                _unitOfWork.Save();
                TempData["success"] = "Category Update successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var category = _unitOfWork.Category.Get(c => c.Id == Id);
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? Id)
        {
            Category? obj = _unitOfWork.Category.Get(c => c.Id == Id);
            if (obj == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Remove(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        #region Delete in one point
        //public IActionResult DeletePost(int? Id)
        //{
        //    Category? obj = _context.Categories.Find(Id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        _context.Categories.Remove(obj);
        //        _context.SaveChanges();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //} 
        #endregion
    }
}
