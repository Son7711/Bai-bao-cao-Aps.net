using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        WebsiteBanHangEntities2 objWebsiteBanHangEntities2 = new WebsiteBanHangEntities2();
        public ActionResult Index()
        {
            HomeModel objHomeModel = new HomeModel();
            objHomeModel.ListCategory = objWebsiteBanHangEntities2.Categories.ToList();

            objHomeModel.ListProduct = objWebsiteBanHangEntities2.Products.ToList();

            return View(objHomeModel);
        }


        [HttpGet]
        public ActionResult Register()
        {


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            // Kiểm tra xem các trường email và mật khẩu có bị trống không
            if (string.IsNullOrEmpty(_user.Email) || string.IsNullOrEmpty(_user.Password))
            {
                ViewBag.error = "Email and Password are required.";
                return View();
            }

            if (ModelState.IsValid)
            {
                var check = objWebsiteBanHangEntities2.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    objWebsiteBanHangEntities2.Configuration.ValidateOnSaveEnabled = false;
                    objWebsiteBanHangEntities2.Users.Add(_user);
                    objWebsiteBanHangEntities2.SaveChanges();

                    // Chuyển hướng đến trang thông báo thành công
                    TempData["SuccessMessage"] = "Registration successful! What would you like to do next?";
                    return RedirectToAction("RegistrationSuccess");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }
            return View();
        }

        public ActionResult RegistrationSuccess()
        {
            // Trả về view thông báo thành công
            return View();
        }



        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }










        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            // Kiểm tra xem các trường email và mật khẩu có bị trống không
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.error = "Email and Password are required.";
                return View();
            }

            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = objWebsiteBanHangEntities2.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    // Thêm session
                    Session["FullName"] = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["idUser"] = data.FirstOrDefault().Id;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return View();
                }
            }
            return View();
        }



        [HttpGet]
        public ActionResult Logout()
        {
            // Xóa session khi người dùng đăng xuất
            Session.Clear(); // Hoặc Session.Abandon();
            Debug.WriteLine("User logged out. Redirecting to Index.");
            return RedirectToAction("Index");
        }





        public ActionResult Contact()
        {
            return View();

        }
    }
}