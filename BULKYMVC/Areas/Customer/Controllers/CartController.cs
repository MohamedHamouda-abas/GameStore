using BULKY.DataAccess.Repository.IRepository;
using BULKY.Models.Models;
using BULKY.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Stripe;
using Stripe.Checkout;
using Stripe.Issuing;
using System.Security.Claims;
using Utilities;

namespace BULKYMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVm ShoppingCartVm { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVm = new()
            {
                shoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product").ToList(),
                OrderHeader = new OrderHeader()
            };

            foreach (var cart in ShoppingCartVm.shoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuentity(cart);
                ShoppingCartVm.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVm);
        }

        public IActionResult plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(s => s.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            TempData["success"] = "Product Added successfully";
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(s => s.Id == cartId);
            if (cartFromDb.Count <= 1)
            {
                // Remove That from Db
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
                TempData["success"] = "Product Removed successfully";
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            TempData["success"] = "Product Minused successfully";
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(s => s.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            TempData["success"] = "Product Deleted successfully";
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVm = new()
            {
                shoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product").ToList(),
                OrderHeader = new OrderHeader()
            };

            ShoppingCartVm.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            ShoppingCartVm.OrderHeader.Name = ShoppingCartVm.OrderHeader.ApplicationUser.Name;
            ShoppingCartVm.OrderHeader.PhoneNumber = ShoppingCartVm.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVm.OrderHeader.StreetAddress = ShoppingCartVm.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVm.OrderHeader.City = ShoppingCartVm.OrderHeader.ApplicationUser.City;
            ShoppingCartVm.OrderHeader.State = ShoppingCartVm.OrderHeader.ApplicationUser.State;
            ShoppingCartVm.OrderHeader.PhoneNumber = ShoppingCartVm.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVm.OrderHeader.PostalCode = ShoppingCartVm.OrderHeader.ApplicationUser.PostalCode;

            foreach (var cart in ShoppingCartVm.shoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuentity(cart);
                ShoppingCartVm.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVm);
        }
        
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVm.shoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product").ToList();

            ShoppingCartVm.OrderHeader.ApplicationUserId = userId;
            ShoppingCartVm.OrderHeader.OrderDate = DateTime.Now;

            //You don't need to map the value case the bindproperty is map it automaticlly
            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);


            foreach (var cart in ShoppingCartVm.shoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuentity(cart);
                ShoppingCartVm.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }


            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {//Noraml user
                ShoppingCartVm.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVm.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            { //company user
                ShoppingCartVm.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVm.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            _unitOfWork.OrderHeader.Add(ShoppingCartVm.OrderHeader);
            _unitOfWork.Save();
            foreach (var cart in ShoppingCartVm.shoppingCartList)   
            {
                OrderDetail detail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVm.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count,
                };
                _unitOfWork.OrderDetails.Add(detail);
                _unitOfWork.Save();
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                var domain = "https://localhost:7232/";
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVm.OrderHeader.Id}",
                    CancelUrl = domain + $"customer/cart/Index",
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                    Mode = "payment",
                };

                foreach (var item in ShoppingCartVm.shoppingCartList)
                {
                    var sessionLinItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Title
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLinItem);
                }

                var service = new Stripe.Checkout.SessionService();
                Session session = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStripePaymentId(ShoppingCartVm.OrderHeader.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVm.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader=_unitOfWork.OrderHeader.Get(u=>u.Id == id,includeProperties:"ApplicationUser");
            if (orderHeader.PaymentStatus!=SD.PaymentStatusDelayedPayment)
            {
                //this is customert
                 var service=new SessionService();
                Session session=service.Get(orderHeader.SessionId);
                if(session.PaymentStatus.ToLower()=="paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentId(id,session.Id,session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id,SD.StatusApproved,SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }
            List<ShoppingCart>shoppingCarts=_unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId==orderHeader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();
            return View(id);
        }
        private double GetPriceBasedOnQuentity(ShoppingCart cartQunrtity)
        {
            if (cartQunrtity.Count <= 50)
            {
                return cartQunrtity.Product.Price;
            }
            else
            {
                if (cartQunrtity.Count <= 99)
                {
                    return cartQunrtity.Product.Price50;
                }
                else
                {
                    return cartQunrtity.Product.Price100;
                }
            }
        }
    }
}
