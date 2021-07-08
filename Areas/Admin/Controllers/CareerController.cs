using FindJob.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindJob.Areas.Admin.Controllers
{
    public class CareerController : Controller
    {
        private FindJobEntities db = new FindJobEntities();
        // GET: Admin/Career
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var model = db.Careers.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
            return View(model);
        }


        public JsonResult Delete(long ID)
        {

            try
            {
                var career = db.Careers.Find(ID);
                db.Careers.Remove(career);
                db.SaveChanges();
                return Json(new
                {
                    status = true
                });
            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }

        }


        [HttpPost]
        public ActionResult addCareer(Career entity)
        {
            try
            {
                var prv = new Career();
                prv.CareerName = entity.CareerName;
                db.Careers.Add(prv);
                db.SaveChanges();
                TempData["add_success"] = "Thêm ngành tuyển dụng thành công";
                return RedirectToAction("Index");

            }
            catch
            {
                TempData["add_success"] = "Thêm ngành tuyển dụng KHÔNG thành công";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult editcareer(Career entity)
        {
            try
            {
                var prv = db.Careers.Find(entity.ID);
                prv.CareerName = entity.CareerName;
                db.SaveChanges();
                TempData["add_success"] = "Sửa ngành tuyển dụng thành công";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["add_success"] = "Sửa ngành tuyển dụng KHÔNG thành công";
                return RedirectToAction("Index");
            }
        }

        public JsonResult GetCareerByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var career = db.Careers.Find(ID);
            return Json(career, JsonRequestBehavior.AllowGet);
        }
    }
}