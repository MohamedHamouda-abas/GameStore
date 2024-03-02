﻿using BULKY.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BULKY.DataAccess.Repository.IRepository
{
    public interface IOrderDetailsRepository:IRepository<OrderDetail>
    {
        public void Update(OrderDetail orderDetail);    
    }
}
