using FindJob.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindJob.Controllers
{
    public class HomeController : Controller
    {
        private FindJobEntities db = new FindJobEntities();
        // GET: Home
        //show danh sách bài viết tuyển dụng đã đăng, ứng viên nổi bật
        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            var model = db.Posts.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pagesize);
            var sarary = db.Posts.Max(x => x.Salary);
            ViewBag.JobFeature = db.Posts.Where(x => x.Salary == sarary).ToList();
            ViewBag.CadidateFeature = db.Candidates.Take(5).ToList();
            ViewBag.Career = db.Careers.ToList();
            return View(model);
        }

        //Tìm kiếm bài viết->dành cho ứng viên
        public ActionResult SearchJob(long Career_ID, string Experience, int page = 1, int pagesize = 10)
        {
            if(Career_ID != 0 && Experience != "0")
            {
                var model = db.Posts.Where(x => x.Career_ID == Career_ID && x.Experience == Experience).OrderByDescending(x => x.CreatedDate).ToPagedList(page, pagesize);
                ViewBag.keyword = "Ngành: " + db.Careers.FirstOrDefault(x => x.ID == Career_ID).CareerName + ", Kinh nghiệm " + Experience + " năm";
                var sarary = db.Posts.Max(x => x.Salary);
                ViewBag.JobFeature = db.Posts.Where(x => x.Salary == sarary).ToList();
                ViewBag.Career_ID = Career_ID;
                ViewBag.Experience = Experience;
                return View(model);
            }
            else if(Career_ID != 0)
            {
                var model = db.Posts.Where(x => x.Career_ID == Career_ID).OrderByDescending(x => x.CreatedDate).ToPagedList(page, pagesize);
                ViewBag.keyword = "Ngành: " + db.Careers.FirstOrDefault(x => x.ID == Career_ID).CareerName;
                var sarary = db.Posts.Max(x => x.Salary);
                ViewBag.JobFeature = db.Posts.Where(x => x.Salary == sarary).ToList();
                ViewBag.Career_ID = Career_ID;
                ViewBag.Experience = Experience;
                return View(model);
            }
            else if(Experience != "0")
            {
                var model = db.Posts.Where(x => x.Experience == Experience).OrderByDescending(x => x.CreatedDate).ToPagedList(page, pagesize);
                ViewBag.keyword = "Kinh nghiệm " + Experience + " năm";
                var sarary = db.Posts.Max(x => x.Salary);
                ViewBag.JobFeature = db.Posts.Where(x => x.Salary == sarary).ToList();
                ViewBag.Career_ID = Career_ID;
                ViewBag.Experience = Experience;
                return View(model);
            }
            else
            {
                var model = db.Posts.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pagesize);
                ViewBag.keyword = "";
                var sarary = db.Posts.Max(x => x.Salary);
                ViewBag.JobFeature = db.Posts.Where(x => x.Salary == sarary).ToList();
                ViewBag.Career_ID = Career_ID;
                ViewBag.Experience = Experience;
                return View(model);
            }
            
        }


        //Tìm kiếm ứng viên => dành cho nhà tuyển dụng
        public ActionResult SearchCandidate(string MainSkill, int Experience, int page = 1, int pagesize = 10)
        {
            if (MainSkill != "0" && Experience != 0)
            {
                var model = db.Candidates.Where(x => x.MainSkill.Contains(MainSkill) && x.Experience == Experience).OrderByDescending(x => x.Fullname).ToPagedList(page, pagesize);
                ViewBag.keyword = "Kỹ năng chính: " + MainSkill + ", Kinh nghiệm " + Experience + " năm";
                var sarary = db.Posts.Max(x => x.Salary);
                ViewBag.JobFeature = db.Posts.Where(x => x.Salary == sarary).ToList();
                ViewBag.MainSkill = MainSkill;
                ViewBag.Experience = Experience;
                return View(model);
            }
            else if (MainSkill != "0")
            {
                var model = db.Candidates.Where(x => x.MainSkill.Contains(MainSkill)).OrderByDescending(x => x.Fullname).ToPagedList(page, pagesize);
                ViewBag.keyword = "Kỹ năng chính: " + MainSkill;
                var sarary = db.Posts.Max(x => x.Salary);
                ViewBag.JobFeature = db.Posts.Where(x => x.Salary == sarary).ToList();
                ViewBag.MainSkill = MainSkill;
                ViewBag.Experience = Experience;
                return View(model);
            }
            else if (Experience != 0)
            {
                var model = db.Candidates.Where(x => x.Experience == Experience).OrderByDescending(x => x.Fullname).ToPagedList(page, pagesize);
                ViewBag.keyword = "Kinh nghiệm " + Experience + " năm";
                var sarary = db.Posts.Max(x => x.Salary);
                ViewBag.JobFeature = db.Posts.Where(x => x.Salary == sarary).ToList();
                ViewBag.MainSkill = MainSkill;
                ViewBag.Experience = Experience;
                return View(model);
            }
            else
            {
                var model = db.Candidates.OrderByDescending(x => x.Fullname).ToPagedList(page, pagesize);
                ViewBag.keyword = "";
                var sarary = db.Posts.Max(x => x.Salary);
                ViewBag.JobFeature = db.Posts.Where(x => x.Salary == sarary).ToList();
                ViewBag.MainSkill = MainSkill;
                ViewBag.Experience = Experience;
                return View(model);
            }

        }
    }
}