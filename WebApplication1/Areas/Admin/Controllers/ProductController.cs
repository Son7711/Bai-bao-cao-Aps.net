using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebApplication1.Context;
using static WebApplication1.common;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {

        WebsiteBanHangEntities2 objWebsiteBanHangEntities2 = new WebsiteBanHangEntities2();
        // GET: Admin/Product


        public ActionResult Index(string currentFilter, string SearchString, int? page)
        { 
            var lstProduct = new List<Product>();
            if (SearchString != null)
            {
            page = 1;
            }
            else
            { 
            SearchString = currentFilter;
            }
           
            if (!string.IsNullOrEmpty(SearchString))
            {
                //lấy do sản phẩm theo từ khóa tìm kiếm
                lstProduct = objWebsiteBanHangEntities2.Products.Where(n => n.Name.Contains(SearchString)).ToList();

            }
            else
            {
                //lay all sån phâm trong bång product
                lstProduct = objWebsiteBanHangEntities2.Products.ToList();

            }
            ViewBag.CurrentFilter = SearchString;
            // số lượng item của 1 trang = 4
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            //sắp xếp theo id sản phẩm sp mới đưa lên đầu
            lstProduct = lstProduct.OrderByDescending (n => n. Id).ToList();
            return View(lstProduct.ToPagedList(pageNumber, pageSize));
        }
 


        [HttpGet]
        public ActionResult Create()
        {
            
            this.LoadData();
            return View();
        }



        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Product objProduct)
        {
            this.LoadData();
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(objProduct.Name))
            {
                if (string.IsNullOrWhiteSpace(objProduct.Name))
                {
                    ModelState.AddModelError("Name", "Tên sản phẩm không được để trống.");
                }

                // Nếu model không hợp lệ, trở lại view với thông báo lỗi
                return View(objProduct);
            }

            if (objProduct.ImageUpload != null && objProduct.ImageUpload.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                    string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                    fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")) + extension;
                    objProduct.Avatar = fileName;
                    string path = Path.Combine(Server.MapPath("~/Content/images/ "), fileName);
                    objProduct.ImageUpload.SaveAs(path);
                }
                else
                {
                    objProduct.Avatar = "default-image.jpg"; // Gán hình ảnh mặc định hoặc xử lý theo cách khác
                }

                objWebsiteBanHangEntities2.Products.Add(objProduct);
                objWebsiteBanHangEntities2.SaveChanges();
                return RedirectToAction("Index");
          
           
            
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            var objProduct = objWebsiteBanHangEntities2.Products.Where( n => n.Id == Id).FirstOrDefault();
            return View(objProduct);
        }



        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var objProduct = objWebsiteBanHangEntities2.Products.Where(n => n.Id == Id).FirstOrDefault();
            
            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Delete(Product objPro)
        {
            var objProduct = objWebsiteBanHangEntities2.Products.Where(n => n.Id == objPro.Id).FirstOrDefault();
            objWebsiteBanHangEntities2.Products.Remove(objProduct);
            objWebsiteBanHangEntities2.SaveChanges();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var objProduct = objWebsiteBanHangEntities2.Products.Where(n => n.Id == Id).FirstOrDefault();

            return View(objProduct);
        }


        [HttpPost]
        public ActionResult Edit(Product objProduct)
        {
            if (objProduct.ImageUpload != null && objProduct.ImageUpload.ContentLength > 0)
            {
                string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")) + extension;
                objProduct.Avatar = fileName;
                string path = Path.Combine(Server.MapPath("~/Content/images/ "), fileName);
                objProduct.ImageUpload.SaveAs(path);
            }
            else
            {
                objProduct.Avatar = "koloaddchinhanh.jpg"; // Gán hình ảnh mặc định hoặc xử lý theo cách khác
            }

            objWebsiteBanHangEntities2.Entry(objProduct).State = EntityState.Modified;
            objWebsiteBanHangEntities2.SaveChanges();
            return RedirectToAction("Index");
           
        }


        void LoadData()
        {
            common objcommon = new common();
            var lstCat = objWebsiteBanHangEntities2.Categories.ToList();
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtCategory = converter.ToDataTable(lstCat);
            ViewBag.ListCategory = objcommon.ToSelectList(dtCategory, "Id", "Name");




            var lstBrand = objWebsiteBanHangEntities2.Brands.ToList();
            DataTable dtBrand = converter.ToDataTable(lstBrand);

            ViewBag.ListBrand = objcommon.ToSelectList(dtBrand, "Id", "Name");

            //loai sp
            List<ProductType> lstProductType = new List<ProductType>();
            ProductType objProductType = new ProductType();
            objProductType.Id = 01;
            objProductType.Name = "Giảm giá sốc";
            lstProductType.Add(objProductType);

            objProductType = new ProductType();
            objProductType.Id = 02;
            objProductType.Name = "Đề xuất";
            lstProductType.Add(objProductType);

            DataTable dtProductType = converter.ToDataTable(lstProductType);
            ViewBag.ProductType = objcommon.ToSelectList(dtProductType, "Id", "Name");

        }
    }
}

