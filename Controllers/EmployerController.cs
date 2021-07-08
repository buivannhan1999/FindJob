using FindJob.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FindJob.Controllers
{
    public class EmployerController : Controller
    {
        private FindJobEntities db = new FindJobEntities();
        // GET: Employer
        // Hồ sơ nhà tuyển dụng
        public ActionResult Index(long ID)
        {
            var employer = db.Employers.FirstOrDefault(x => x.User_ID == ID);
            ViewBag.profile = employer;
            var sarary = db.Posts.Max(x => x.Salary);
            ViewBag.JobFeature = db.Posts.Where(x => x.Salary == sarary).ToList();
            if(employer != null)
            {
                ViewBag.ListEvaluate = db.Evaluates.Where(x => x.Type == 1 && x.Employer_ID == employer.ID).ToList();
            }
            return View();
        }


        //Chỉnh sửa hồ sơ nhà tuyển dụng
        public ActionResult Edit()
        {
            var user = Session["user"] as User;
            if (user == null)
                return Redirect("/User/Login");
            ViewBag.Employer = db.Employers.FirstOrDefault(x => x.User_ID == user.ID);
            return View();
        }


        //Lưu chỉnh sửa hồ sơ
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit_Employer(Employer entity, HttpPostedFileBase Image)
        {
            var employ = db.Employers.FirstOrDefault(x => x.User_ID == entity.User_ID);
            if (employ != null)
            {
                var emp = db.Employers.Find(entity.ID);
                emp.Fullname = entity.Fullname;
                emp.Place = entity.Place;
                emp.Descriptions = entity.Descriptions;
                emp.CompanyName = entity.CompanyName;
                emp.Address = entity.Address;
                emp.Email = entity.Email;
                emp.User_ID = entity.User_ID;
                emp.Status = true;
                if (Image != null && emp.Image != Image.FileName)
                {
                    //Xóa file cũ
                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/images/employer"), emp.Image));
                    //Thêm hình ảnh
                    var path = Path.Combine(Server.MapPath("~/Assets/images/employer"), Image.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        string extensionName = Path.GetExtension(Image.FileName);
                        string filename = Image.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName;
                        path = Path.Combine(Server.MapPath("~/Assets/images/employer/"), filename);
                        Image.SaveAs(path);
                        emp.Image = filename;
                    }
                    else
                    {
                        Image.SaveAs(path);
                        emp.Image = Image.FileName;
                    }
                }
                db.SaveChanges();
            }
            else
            {
                //Thêm hình ảnh
                var path = Path.Combine(Server.MapPath("~/Assets/images/employer"), Image.FileName);
                if (System.IO.File.Exists(path))
                {
                    string extensionName = Path.GetExtension(Image.FileName);
                    string filename = Image.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName;
                    path = Path.Combine(Server.MapPath("~/Assets/images/employer/"), filename);
                    Image.SaveAs(path);
                    entity.Image = filename;
                }
                else
                {
                    Image.SaveAs(path);
                    entity.Image = Image.FileName;
                }

                db.Employers.Add(entity);
                db.SaveChanges();
            }
            var user = Session["user"] as User;
            return Redirect("/Employer/Index/" + user.ID);
        }

        //Đăng bài tuyển dụng
        public ActionResult Post()
        {
            var user = Session["user"] as User;
            if (user == null)
                return Redirect("/User/Login");
            ViewBag.Employer = db.Employers.FirstOrDefault(x => x.User_ID == user.ID);
            ViewBag.Career = db.Careers.ToList();
            return View();
        }


        //Lưu bài tuyển dụng
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddPost(Post entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.Status = true;
            db.Posts.Add(entity);
            db.SaveChanges();
            TempData["add_success"] = "Đăng thông tin tuyển dụng thành công";
            return RedirectToAction("Post");
        }

        //Sửa bài tuyển dụng
        public ActionResult EditPost(long ID)
        {
            var user = Session["user"] as User;
            if (user == null)
                return Redirect("/User/Login");
            ViewBag.Employer = db.Employers.FirstOrDefault(x => x.User_ID == user.ID);
            ViewBag.Career = db.Careers.ToList();
            ViewBag.Post = db.Posts.Find(ID);
            return View();
        }


        //Lưu chỉnh sửa
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit_Post(Post entity)
        {
            var post = db.Posts.Find(entity.ID);
            post.Title = entity.Title;
            post.Location = entity.Location;
            post.Career_ID = entity.Career_ID;
            post.Form = entity.Form;
            post.Dealine = entity.Dealine;
            post.Salary = entity.Salary;
            post.Descriptions = entity.Descriptions;
            post.Experience = entity.Experience;
            db.SaveChanges();
            TempData["add_success"] = "Sửa thông tin tuyển dụng thành công";
            return RedirectToAction("ListPost");
        }


        //Thêm đánh giá cho nhà tuyển dụng
        public JsonResult addEvaluate(string json_eval)
        {
            var JsonReview = new JavaScriptSerializer().Deserialize<List<Evaluate>>(json_eval);
            var eval = new Evaluate();
            foreach (var item in JsonReview)
            {
                eval.Candidate_ID = item.Candidate_ID;
                eval.Employer_ID = item.Employer_ID;
                eval.CreatedDate = DateTime.Now;
                eval.Rate = item.Rate;
                eval.Descriptions = item.Descriptions;
                eval.Type = 1;//Dùng cho người dùng đánh giá nhà tuyển dụng
                eval.Status = true;
            }

            db.Evaluates.Add(eval);
            db.SaveChanges();
            return Json(new
            {
                status = true
            });

        }


        //Bài tuyển dụng đã đăng
        public ActionResult ListPost(int page = 1, int pagesize = 10)
        {
            var user = Session["user"] as User;
            if (user == null)
                return Redirect("/User/Login");
            var employer = db.Employers.FirstOrDefault(x => x.User_ID == user.ID);
            if (employer == null)
                return RedirectToAction("Edit");
            var model = db.Posts.Where(x => x.Employer_ID == employer.ID).OrderByDescending(x => x.CreatedDate).ToPagedList(page, pagesize);
            return View(model);
        }


        //Danh sách ứng viên tham gia ứng tuyển
        public ActionResult ListJoin(long ID)
        {
            var user = Session["user"] as User;
            if (user == null)
                return Redirect("/User/Login");

            ViewBag.ListJoin = db.Joins.Where(x => x.Post_ID == ID && x.Status == false).ToList();
            ViewBag.Post = db.Posts.Find(ID);
            return View();
        }


        //Danh sách ứng viên đc chấp nhận
        public ActionResult Accept(long ID)
        {
            var user = Session["user"] as User;
            if (user == null)
                return Redirect("/User/Login");
            ViewBag.ListAccept = db.Joins.Where(x => x.Post_ID == ID && x.Status == true).ToList();
            ViewBag.Post = db.Posts.Find(ID);
            return View();
        }


        //Chọn ứng viên
        public JsonResult ChosesCandidate(long ID)
        {
            var join = db.Joins.Find(ID);
            join.Status = true;
            db.SaveChanges();
            return Json(new
            {
                status = true
            });

        }


        //XÓa ứng viên khỏi danh sách chấp nhận
        public JsonResult DeleteCandidate(long ID)
        {
            var join = db.Joins.Find(ID);
            join.Status = false;
            db.SaveChanges();
            return Json(new
            {
                status = true
            });

        }
        

        //Xóa bài viết
        public JsonResult DeletePost(long ID)
        {
            var join = db.Joins.Where(x => x.Post_ID == ID).ToList();
            foreach(var item in join)
            {
                db.Joins.Remove(item);
            }
            var save = db.Saves.Where(x => x.Post_ID == ID).ToList();
            foreach (var item in save)
            {
                db.Saves.Remove(item);
            }
            var post = db.Posts.Find(ID);
            db.Posts.Remove(post);
            db.SaveChanges();
            return Json(new
            {
                status = true
            });

        }
    }
}