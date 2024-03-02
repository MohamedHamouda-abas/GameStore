using BULKY.DataAccess.Data;
using BULKY.DataAccess.Repository.IRepository;
using BULKY.Models.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BULKY.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _context;
        public CompanyRepository(ApplicationDbContext context) :base(context) 
        {
            _context= context;
        }
        public void Update(Company company)
        {
            _context.Companies.Update(company);
        }
    }
}
