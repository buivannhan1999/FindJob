using FindJob.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindJob.Areas.Admin.Controllers
{
    public class EmployerController : Controller
    {
        private FindJobEntities db = new FindJobEntities();

        //show danh sách nhà tuyển dụng
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var model = db.Employers.OrderByDescending(x => x.Fullname).ToPagedList(page, pageSize);
            return View(model);
        }


        //Thay đổi trạng thái nhà tuyển dụng
        public JsonResult changeStatus(long ID)
        {
            var empl = db.Employers.Find(ID);
            var user = db.Users.Find(empl.User_ID);
            if (empl.Status == true)
            {
                user.Status = false;
                empl.Status = false;
            }
            else
            {
                user.Status = true;
                empl.Status = true;
            }
                
            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }

        //Autocomplete tìm kiếm nhà tuyển dụng
        public JsonResult ListName(string q)
        {
            var query = db.Employers.Where(x => x.Fullname.Contains(q)).Select(x => x.Fullname);
            return Json(new
            {
                data = query,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        //tìm kiếm  nhà tuyển dụng
        public ActionResult Search(string keyword, int page = 1, int pagesize = 6)
        {
            string[] key = keyword.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            var empl_name = new List<Employer>();//Tìm theo tên sản phẩm
            foreach (var item in key)
            {
                empl_name = (from b in db.Employers
                             where b.Fullname.Contains(item)
                             select b).ToList();
            }
            ViewBag.KeyWord = keyword;
            return View(empl_name.ToPagedList(page, pagesize));
        }
    }
}