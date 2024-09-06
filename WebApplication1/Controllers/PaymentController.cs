using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult Index()
        {
            if (Session["idUser"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var lstCart = Session["cart"] as List<CartModel>;
            if (lstCart == null || !lstCart.Any())
            {
                ViewBag.ErrorMessage = "Giỏ hàng của bạn đang rỗng. Vui lòng thêm sản phẩm trước khi thanh toán.";
                return View();
            }

            using (var db = new WebsiteBanHangEntities2())
            {
                var objOrder = new Order
                {
                    Name = "DonHang-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    UserId = int.Parse(Session["idUser"].ToString()),
                    CreatedOnUtc = DateTime.Now,
                    Status = 1
                };

                db.Orders.Add(objOrder);
                db.SaveChanges();

                int intOrderId = objOrder.Id;

                var lstOrderDetail = new List<OrderDetail>();
                foreach (var item in lstCart)
                {
                    var objOrderDetail = new OrderDetail
                    {
                        Quantity = item.Quantity,
                        OrderId = intOrderId,
                        ProductId = item.Product.Id
                    };
                    lstOrderDetail.Add(objOrderDetail);
                }

                db.OrderDetails.AddRange(lstOrderDetail);
                db.SaveChanges();
            }

            // Xóa giỏ hàng sau khi thanh toán
            Session["cart"] = null;
            // Cập nhật số lượng sản phẩm trong giỏ hàng thành 0
            Session["count"] = 0;


            // Có thể thêm thông báo thành công hoặc chuyển hướng người dùng
            return View();
        }
    }
}
