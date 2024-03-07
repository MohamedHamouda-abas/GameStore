using BULKY.DataAccess.Data;
using BULKY.DataAccess.Repository.IRepository;
using BULKY.Models.Models;
using BULKY.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Climate;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Utilities;

namespace BULKYMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<OrderHeader> orders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
            return View(orders);
        }
        public IActionResult Details(int orderId)
        {
            OrderVM = new()
            {
                orderHeader=_unitOfWork.OrderHeader.Get(u=>u.Id==orderId,includeProperties: "ApplicationUser"),
                orderDetails=_unitOfWork.OrderDetails.GetAll(u=>u.OrderHeaderId==orderId,includeProperties: "Product")

            };
            return View(OrderVM);
        }
        [HttpPost]
        [Authorize(Roles =SD.Role_Admin)]
        public IActionResult UpdateOrderDetails()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id ==OrderVM.orderHeader.Id);
            orderHeaderFromDb.Name=OrderVM.orderHeader.Name;
            orderHeaderFromDb.PhoneNumber=OrderVM.orderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress=OrderVM.orderHeader.StreetAddress;
            orderHeaderFromDb.PostalCode=OrderVM.orderHeader.PostalCode;
            orderHeaderFromDb.City=OrderVM.orderHeader.City;
            orderHeaderFromDb.State=OrderVM.orderHeader.State;

            if (!string.IsNullOrEmpty(OrderVM.orderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier=OrderVM.orderHeader.Carrier;  
            }
            if (!string.IsNullOrEmpty(OrderVM.orderHeader.TrackingNumber))
            {
                orderHeaderFromDb.Carrier = OrderVM.orderHeader.TrackingNumber;
            }
            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Updated is done";
            return RedirectToAction(nameof(Details), new { orderId =orderHeaderFromDb.Id});
        }


        #region Api Ajax
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> objOrderlist = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();

            switch (status)
            {
                case "pending":
                    objOrderlist = objOrderlist.Where(u => u.OrderStatus == SD.PaymentStatusPending);
                    break;

                case "inprocess":
                    objOrderlist = objOrderlist.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;

                case "completed":
                    objOrderlist = objOrderlist.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;

                case "approved":
                    objOrderlist = objOrderlist.Where(u => u.OrderStatus == SD.PaymentStatusApproved);
                    break;

                case "approvedfordelayedpayment":
                    objOrderlist = objOrderlist.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;

                default:
                    break;

            }
            return Json(new { data = objOrderlist });
        }
        #endregion

    }
}
