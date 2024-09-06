using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Context;


namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {
        WebsiteBanHangEntities2 objWebsiteBanHangEntities2 = new WebsiteBanHangEntities2();

        // GET: Product
        public ActionResult Detail( int Id)
        {
            var objProduct = objWebsiteBanHangEntities2.Products.Where( n=>n.Id == Id).FirstOrDefault(); 
            return View(objProduct);
        }
    }
}