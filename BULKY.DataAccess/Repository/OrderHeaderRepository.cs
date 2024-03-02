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
    public class OrderHeaderRepository:Repository<OrderHeader>,IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }

        public void Update(OrderHeader orderHeader)
        {
           _context.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
           var orderFromDb=_context.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentId(int id, string sessionId, string PaymentIntentId)
        {
            var orderFromDb = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (!string.IsNullOrEmpty(sessionId))
            {
               orderFromDb.SessionId= sessionId;
            }
            if (!string.IsNullOrEmpty(PaymentIntentId))
            {
                orderFromDb.PaymentIntentId = PaymentIntentId;
              orderFromDb.PaymentDate= DateTime.Now;
            }
        }

    }
}
