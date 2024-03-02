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
    public class OrderDetailsRepository:Repository<OrderDetail> ,IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrderDetail orderDetail)
        {
           _context.orderDetails.Update(orderDetail);
        }
    }
}
