using FindJob.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindJob.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private FindJobEntities db = new FindJobEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {
            //đếm số ứng viên
            ViewBag.Count_Candidate = db.Candidates.Count();

            //số nhà tuyển dụng
            ViewBag.Count_Employer = db.Employers.Count();

            //số bài đăng tuyển
            ViewBag.Count_Post = db.Posts.Count();

            //Só bài đăng tuyển thành công
            ViewBag.Count_Join = db.Joins.Where(x => x.Status == true).Count();
            return View();
        }
    }
}