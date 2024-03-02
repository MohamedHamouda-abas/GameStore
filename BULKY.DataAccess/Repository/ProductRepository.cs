using BULKY.DataAccess.Data;
using BULKY.DataAccess.Repository.IRepository;
using BULKY.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BULKY.DataAccess.Repository
{
    public class ProductRepository:Repository<Product>,IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db):base(db)
        {
         _db = db;
        }

        public void Update(Product obj)
        {
            var objFormDb= _db.Products.FirstOrDefault(x=>x.Id==obj.Id);
            if (objFormDb!=null)
            {
                objFormDb.Title = obj.Title;
                objFormDb.ISBN = obj.ISBN;
                objFormDb.Price = obj.Price;
                objFormDb.Price50 = obj.Price50;
                objFormDb.ListPrice = obj.ListPrice;
                objFormDb.Price100 = obj.Price100;
                objFormDb.Description = obj.Description;
                objFormDb.CategoryId = obj.CategoryId;
                objFormDb.Author = obj.Author;
                if(objFormDb.ImageUrl != null)
                {
                    objFormDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
