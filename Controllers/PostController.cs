using FindJob.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindJob.Controllers
{
    public class PostController : Controller
    {
        private FindJobEntities db = new FindJobEntities();
        // GET: Post
        public ActionResult Index()
        {
            return View();
        }

        //Chi tiết bài đăng
        public ActionResult Detail(long ID)
        {
            var model = db.Posts.Find(ID);
            ViewBag.post = model;
            ViewBag.Employer = db.Employers.Find(model.Employer_ID);
            ViewBag.Candidate = db.Candidates.ToList();
            return View();
        }


        //Tham gia ứng tuyển => đối với ứng viên
        public JsonResult JoinJob(long post_id, long candidate_id)
        {
            var res = db.Joins.Count(x => x.Post_ID == post_id && x.Candidate_ID == candidate_id);
            if(res == 0)
            {
                var join = new Join();
                join.Post_ID = post_id;
                join.Candidate_ID = candidate_id;
                join.DateJoin = DateTime.Now;
                join.Status = false;
                db.Joins.Add(join);
                db.SaveChanges();

                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
            
        }

        //Lưu công việc => đối với ứng viên
        public JsonResult SaveJob(long post_id, long candidate_id)
        {


            var res = db.Saves.Count(x => x.Post_ID == post_id && x.Candidate_ID == candidate_id);
            if (res == 0)
            {
                var save = new Save();
                save.Post_ID = post_id;
                save.Candidate_ID = candidate_id;
                save.DateSave = DateTime.Now;
                db.Saves.Add(save);
                db.SaveChanges();

                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        //Lưu nhà tuyển dụng => đối với ứng viên
        public JsonResult SaveEmployer(long employer_id, long candidate_id)
        {
            var res = db.Saves.Count(x => x.Employer_ID == employer_id && x.Candidate_ID == candidate_id);
            if (res == 0)
            {
                var save = new Save();
                save.Employer_ID = employer_id;
                save.Candidate_ID = candidate_id;
                save.DateSave = DateTime.Now;
                db.Saves.Add(save);
                db.SaveChanges();

                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
                
        }
    }
}