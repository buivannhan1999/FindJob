using FindJob.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindJob.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private FindJobEntities db = new FindJobEntities();
        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }

        //Đăng nhập hệ thống
        public ActionResult Login()
        {
            return View();
        }

        //Check tài khoản đăng nhập
        [HttpPost]
        public ActionResult adminLogin(User model)
        {
            var result = db.Users.Where(x => x.Account == model.Account && x.Password == model.Password && x.Type == 0 ).ToList();
            if (result.Count > 0)
            {
                Session["admin_login"] = db.Users.SingleOrDefault(x => x.Account == model.Account && x.Password == model.Password && x.Type == 0);
                return Redirect("/Admin/Home");
            }
            else
            {
                TempData["error_login"] = "Tài khoản hoặc mật khẩu không chính xác";
                return RedirectToAction("Login");
            }
        }

        //đăng xuất
        public ActionResult Logout()
        {
            Session["admin_login"] = null;
            return RedirectToAction("Login");
        }

        
    }
}