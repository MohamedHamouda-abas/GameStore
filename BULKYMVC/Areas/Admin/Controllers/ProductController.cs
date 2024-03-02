using BULKY.DataAccess.Repository.IRepository;
using BULKY.Models.Models;
using BULKY.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.IdentityModel.Tokens;
using Utilities;

namespace BULKYMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> GetAll = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();          
            return View(GetAll);
        }
        [HttpGet]
        public IActionResult Upsert(int? Id)
        {
            #region Hash
            //How To Asign Category IN DropDownList
            //How To Send Data With Viewbag
            //ViewBag.categoryList = categoryList;
            //How To Send Data With ViewData
            //ViewData["CategoryList"]=categoryList; 
            #endregion
            ProductVm productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                }),
                product = new Product()
            };
            if (Id == null || Id == 0)
            {
                //Craete
                return View(productVM);
            }
            else
            {
                //Update
                productVM.product=_unitOfWork.Product.Get(p=>p.Id == Id);
                return View(productVM);
            }    
        }
        [HttpPost]
        public IActionResult Upsert(ProductVm model , IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _WebHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName); 
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product");

                    if(!string.IsNullOrEmpty(model.product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, model.product.ImageUrl.Trim('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream =new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    model.product.ImageUrl =@"\Images\Product\"+fileName;
                }

                if (model.product.Id == 0)
                {
                    _unitOfWork.Product.Add(model.product);
                    TempData["success"] = "Product Create successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(model.product);
                    TempData["success"] = "Product Update successfully";
                }
               
                _unitOfWork.Save();             
                return RedirectToAction(nameof(Index));
            }
            else
            {
                model.CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                });               
            return View(model);
            }
        }
        #region Edit
        //[HttpGet]
        //public IActionResult Edit(int? Id)
        //{
        //    if (Id == null || Id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var Product = _unitOfWork.Product.Get(c => c.Id == Id);
        //    return View(Product);
        //}
        //[HttpPost]
        //public IActionResult Edit(Product model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(model);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product Update successfully";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //} 
        #endregion
        #region Delete
        //[HttpGet]
        //public IActionResult Delete(int? Id)
        //{
        //    if (Id == null || Id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var Product = _unitOfWork.Product.Get(c => c.Id == Id);
        //    return View(Product);
        //}
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeletePost(int? Id)
        //{
        //    Product? obj = _unitOfWork.Product.Get(c => c.Id == Id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Remove(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product Deleted successfully";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //}

        #endregion
        #region CallApi
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objproductlist = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objproductlist });
        }
        [HttpDelete]
        public IActionResult Delete(int? Id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u=>u.Id==Id);
            if (productToBeDeleted == null)
            {
            return Json(new{ success =false, Message = "Product Deleted successfully" });
            }
            //delete the old image
            var oldImagePath = Path.Combine(_WebHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.Trim('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, Message = "Product Deleted successfully" });
        }
        #endregion
    }
}
