using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BULKY.Models.Models;

namespace BULKY.Models.ViewModels
{
    public class ShoppingCartVm
    {
        public IEnumerable<ShoppingCart> shoppingCartList { get; set; }
        public OrderHeader OrderHeader { get; set; }

    }
}
