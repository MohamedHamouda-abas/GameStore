using BULKY.Models.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BULKY.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        public void Update(Company company);
    }
}
