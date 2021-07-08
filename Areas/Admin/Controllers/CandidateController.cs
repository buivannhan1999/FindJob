using FindJob.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindJob.Areas.Admin.Controllers
{
    public class CandidateController : Controller
    {
        private FindJobEntities db = new FindJobEntities();
        // GET: Admin/Candidate

        //Show danh sách ứng viên
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var model = db.Candidates.OrderByDescending(x => x.Fullname).ToPagedList(page, pageSize);
            return View(model);
        }

        //Thay đổi trạng thái ứng viên
        public JsonResult changeStatus(long ID)
        {
            var cand = db.Candidates.Find(ID);
            var user = db.Users.Find(cand.User_ID);
            if (cand.Status == true)
            {
                user.Status = false;
                cand.Status = false;
            }
            else
            {
                user.Status = true;
                cand.Status = true;
            }

            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }


        //autocomplete tìm kiếm ứng viên
        public JsonResult ListName(string q)
        {
            var query = db.Candidates.Where(x => x.Fullname.Contains(q)).Select(x => x.Fullname);
            return Json(new
            {
                data = query,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        //Tìm kiếm ứng viên
        public ActionResult Search(string keyword, int page = 1, int pagesize = 6)
        {
            string[] key = keyword.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            var cand_name = new List<Candidate>();//Tìm theo tên sản phẩm
            foreach (var item in key)
            {
                cand_name = (from b in db.Candidates
                             where b.Fullname.Contains(item)
                             select b).ToList();
            }
            ViewBag.KeyWord = keyword;
            return View(cand_name.ToPagedList(page, pagesize));
        }
    }
}