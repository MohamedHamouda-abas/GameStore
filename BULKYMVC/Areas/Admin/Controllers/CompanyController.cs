using BULKY.DataAccess.Repository.IRepository;
using BULKY.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace BULKYMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();
            return View(companies);
        }
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                //Create
                return View(new Company());
            }
            else
            {
                //Update
                Company? company = _unitOfWork.Company.Get(c => c.Id == id);
                if (company == null)
                {
                    return View(new Company());
                }
                return View(company);
            }

        }
        [HttpPost]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == null || obj.Id == 0)
                {
                    //Create
                    _unitOfWork.Company.Add(obj);
                    _unitOfWork.Save();
                    TempData["success"] = "The Company Created successfully";

                }
                else
                {
                    //Update
                    _unitOfWork.Company.Update(obj);
                    _unitOfWork.Save();
                    TempData["success"] = "The Company Updated successfully";

                }
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        #region CallApi
        public IActionResult GetAll()
        {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();
            return Json(new {data=companies});
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Company? company = _unitOfWork.Company.Get(c => c.Id == id);
            if (company == null)
            {
                return Json(new { success = false, Message = "Product Deleted successfully" });
            }
            _unitOfWork.Company.Remove(company);
            _unitOfWork.Save();
            return Json(new { success = false, Message = "Product Deleted successfully" });
        }

        #endregion

    }
}
