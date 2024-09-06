using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Context;


namespace WebApplication1.Controllers
{
    public class CategoryController : Controller
    {
        WebsiteBanHangEntities2 objWebsiteBanHangEntities2 = new WebsiteBanHangEntities2();

        // GET: Category
        public ActionResult Category()
        {
            var lstCategory = objWebsiteBanHangEntities2.Categories.ToList();
            return View(lstCategory);
        }

        public ActionResult ProductCategory(int Id)
        {

            var lstProduct = objWebsiteBanHangEntities2.Products.Where(n => n.CategoryId == Id).ToList();
            return View(lstProduct);
        }
















    }
}