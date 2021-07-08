using FindJob.Models;
using FindJob.Models.DTO;
using FindJob.Models.EF;
using Rotativa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FindJob.Controllers
{
    public class UserController : Controller
    {
        private FindJobEntities db = new FindJobEntities();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        //đăng nhập
        public ActionResult Login()
        {
            return View();
        }


        //Check đăng nhập
        [HttpPost]
        public ActionResult FormLogin(User entity)
        {
            if(db.Users.Count(x=> x.Account == entity.Account) > 0 && db.Users.Count(x => x.Password == entity.Password) > 0)
            {
                var user = db.Users.FirstOrDefault(x => x.Account == entity.Account && x.Password == entity.Password);
                if(user.Status == true)
                {
                    Session["user"] = user;
                    return Redirect("/");
                }
                else
                {
                    TempData["error"] = "Tài khoản của bạn đã bị khóa.";
                    return RedirectToAction("Login");
                }
                
            }
            else
            {
                TempData["error"] = "Tài khoản hoặc mật khẩu không chính xác.";
                return RedirectToAction("Login");
            }
        }


        //Đăng ký tài khoản
        public ActionResult Register()
        {
            return View();
        }


        //Lưu tài khoản đăng ký
        [HttpPost]
        public ActionResult FormRegister(User entity)
        {
            entity.Status = true;
            db.Users.Add(entity);
            db.SaveChanges();
            TempData["error"] = "Vui lòng đăng nhập lại sau khi đăng ký.";
            return RedirectToAction("Login");
        }


        //Đăng xuất
        public ActionResult Logout()
        {
            Session["user"] = null;
            return Redirect("/");
        }


        //Sửa thông tin ứng viên
        public ActionResult Edit_Profile()
        {
            var user = Session["user"] as User;
            if (user == null)
                return RedirectToAction("Login");

            ViewBag.Career = db.Careers.ToList();
            ViewBag.Candidate = db.Candidates.FirstOrDefault(x => x.User_ID == user.ID);
            return View();
        }


        //Lưu thông tin đã sửa
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditProfile(Candidate entity, HttpPostedFileBase Image)
        {
            var candi = db.Candidates.FirstOrDefault(x => x.User_ID == entity.User_ID);
            if(candi != null)
            {
                var can = db.Candidates.Find(entity.ID);
                can.Fullname = entity.Fullname;
                can.Birthday = entity.Birthday;
                can.Sex = entity.Sex;
                can.Phone = entity.Phone;
                can.Address = entity.Address;
                can.Email = entity.Email;
                can.Introduce = entity.Introduce;
                can.MainSkill = entity.MainSkill;
                can.Experience = entity.Experience;
                can.Status = true;
                if (Image != null && can.Image != Image.FileName)
                {
                    //Xóa file cũ
                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/images/candidate"), can.Image));
                    //Thêm hình ảnh
                    var path = Path.Combine(Server.MapPath("~/Assets/images/candidate"), Image.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        string extensionName = Path.GetExtension(Image.FileName);
                        string filename = Image.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName;
                        path = Path.Combine(Server.MapPath("~/Assets/images/candidate/"), filename);
                        Image.SaveAs(path);
                        can.Image = filename;
                    }
                    else
                    {
                        Image.SaveAs(path);
                        can.Image = Image.FileName;
                    }
                }
                db.SaveChanges();
            }
            else
            {
                //Thêm hình ảnh
                var path = Path.Combine(Server.MapPath("~/Assets/images/candidate"), Image.FileName);
                if (System.IO.File.Exists(path))
                {
                    string extensionName = Path.GetExtension(Image.FileName);
                    string filename = Image.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName;
                    path = Path.Combine(Server.MapPath("~/Assets/images/candidate/"), filename);
                    Image.SaveAs(path);
                    entity.Image = filename;
                }
                else
                {
                    Image.SaveAs(path);
                    entity.Image = Image.FileName;
                }

                db.Candidates.Add(entity);
                db.SaveChanges();
            }
            var  user = Session["user"] as User;
            return Redirect("/User/Profile_Candidate/" + user.ID);
        }


        //Hồ sơ ứng viên
        public ActionResult Profile_Candidate(long ID)
        {
            var profile = db.Candidates.FirstOrDefault(x => x.User_ID == ID);
            if(profile == null)
                return Redirect("/User/Edit_Profile");
            ViewBag.profile = profile;
            var sarary = db.Posts.Max(x => x.Salary);
            ViewBag.JobFeature = db.Posts.Where(x => x.Salary == sarary).ToList();
            if ( profile != null)
            {
                ViewBag.ListEvaluate = db.Evaluates.Where(x => x.Type == 2 && x.Candidate_ID == profile.ID).ToList();
            }
            return View();
        }


        //Đánh giá ứng viên => cho nhà tuyển dụng
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
                eval.Type = 2;//Dùng cho nhà tuyển dụng đánh giá người tuyển
                eval.Status = true;
            }

            db.Evaluates.Add(eval);
            db.SaveChanges();
            return Json(new
            {
                status = true
            });

        }

        //danh sách công việc đã lưu
        public ActionResult SaveJob()
        {
            var user = Session["user"] as User;
            if (user == null)
                return RedirectToAction("Login");

            var candi = db.Candidates.FirstOrDefault(x => x.User_ID == user.ID);
            if (candi == null)
            {
                return Redirect("/User/Edit_Profile");
            }

            
            ViewBag.saveJob = db.Saves.Where(x => x.Post_ID != null && x.Candidate_ID == candi.ID).OrderByDescending(x => x.DateSave).ToList();
            var sarary = db.Posts.Max(x => x.Salary);
            ViewBag.JobFeature = db.Posts.Where(x => x.Salary == sarary).ToList();
            return View();
        }


        //Danh sách nhà tuyển dụng đã lưu
        public ActionResult SaveEmployer()
        {
            var user = Session["user"] as User;
            if (user == null)
                return RedirectToAction("Login");

            var saveEmploy = db.Saves.Where(x => x.Employer_ID != null && x.Candidate_ID == db.Candidates.FirstOrDefault(a => a.User_ID == user.ID).ID).OrderByDescending(x => x.DateSave).ToList();
            
            if(saveEmploy == null)
            {
                return Redirect("/User/Edit_Profile");
            }

            ViewBag.saveEmployer = saveEmploy;
            var sarary = db.Posts.Max(x => x.Salary);
            ViewBag.JobFeature = db.Posts.Where(x => x.Salary == sarary).ToList();
            return View();
        }

        //Tạo cv online
        public ActionResult Created_CV()
        {
            var user = Session["user"] as User;
            if (user == null)
                return RedirectToAction("Login");

            var candi = db.Candidates.FirstOrDefault(x => x.User_ID == user.ID);

            if (candi == null)
            {
                return Redirect("/User/Edit_Profile");
            }

            if (db.Skills.Count(x => x.Candidate_ID == candi.ID) > 0)
                return RedirectToAction("success");

            ViewBag.Career = db.Careers.ToList();
            ViewBag.Candidate = candi;
            return View();
        }

        [HttpPost]
        public ActionResult CreateCV(CV entity, long Candidate_ID)
        {
            var user = Session["user"] as User;
            if (user == null)
                return RedirectToAction("Login");

            ViewBag.Candidate = db.Candidates.FirstOrDefault(x => x.User_ID == user.ID);

            foreach(var item in entity.LevelDTOs)
            {
                var skill = new Skill();
                skill.MainSkill = item.MainSkill;
                skill.Level = item.Level;
                skill.Candidate_ID = Candidate_ID;
                db.Skills.Add(skill);
                db.SaveChanges();
            }

            foreach(var item in entity.ExperienceDTOs)
            {
                var ex = new Experience();
                ex.Position = item.Position;
                ex.Place = item.Place;
                ex.FromDate = item.FromDate;
                ex.ToDate = item.ToDate;
                ex.Descriptions = item.Descriptions;
                ex.Candidate_ID = Candidate_ID;
                db.Experiences.Add(ex);
                db.SaveChanges();
            }

            foreach(var item in entity.EducationDTOs)
            {
                var educ = new Education();
                educ.Education1 = item.Education;
                educ.School = item.School;
                educ.FromDate = item.FromDate_Educ;
                educ.ToDate = item.ToDate_Educ;
                educ.Descriptions = item.Descriptions_Educ;
                educ.Candidate_ID = Candidate_ID;
                db.Educations.Add(educ);
                db.SaveChanges();
            }
            return RedirectToAction("CV");
        }

        public ActionResult CV()
        {
            var user = Session["user"] as User;
            if (user == null)
                return RedirectToAction("Login");

            var candi = db.Candidates.FirstOrDefault(x => x.User_ID == user.ID);

            if (candi == null)
            {
                return Redirect("/User/Edit_Profile");
            }

            ViewBag.Candidate = candi;
            ViewBag.Skill = db.Skills.Where(x => x.Candidate_ID == candi.ID).ToList();
            ViewBag.Education = db.Educations.Where(x => x.Candidate_ID == candi.ID).ToList();
            ViewBag.Experience = db.Experiences.Where(x => x.Candidate_ID == candi.ID).ToList();
            return View();
        }

        public ActionResult PrintCV(long ID)//ID : Candidate_ID
        {
            ViewBag.Candidate = db.Candidates.Find(ID);

            ViewBag.Skill = db.Skills.Where(x => x.Candidate_ID == ID).ToList();
            ViewBag.Education = db.Educations.Where(x => x.Candidate_ID == ID).ToList();
            ViewBag.Experience = db.Experiences.Where(x => x.Candidate_ID == ID).ToList();

            return View();
        }

        public ActionResult Print(long ID)
        {
            var report = new ActionAsPdf("PrintCV", new { ID = ID });
            return report;
        }

        public ActionResult Edit_CV(long ID)
        {
            ViewBag.Candidate = db.Candidates.Find(ID);

            ViewBag.Skill = db.Skills.Where(x => x.Candidate_ID == ID).ToList();
            ViewBag.Education = db.Educations.Where(x => x.Candidate_ID == ID).ToList();
            ViewBag.Experience = db.Experiences.Where(x => x.Candidate_ID == ID).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult EditCV(CV entity, long Candidate_ID)
        {
            //if(entity.LevelDTOs.Count > db.Skills.Count(x => x.Candidate_ID == Candidate_ID))
            //{
            //    int skill_count = db.Skills.Count(x => x.Candidate_ID == Candidate_ID);
            //    int dem = 0;
                foreach (var item in entity.LevelDTOs)
                {
                   if (item.MainSkill == null)//xóa
                    {
                        var skill = db.Skills.Find(item.ID);
                        db.Skills.Remove(skill);
                        db.SaveChanges();
                    }
                    else if(item.ID == 0)//thêm mới
                    {
                        var skill = new Skill();
                        skill.MainSkill = item.MainSkill;
                        skill.Level = item.Level;
                        skill.Candidate_ID = Candidate_ID;
                        db.Skills.Add(skill);
                        db.SaveChanges();
                    }
                    else
                    {
                        var skill = db.Skills.Find(item.ID);
                        skill.MainSkill = item.MainSkill;
                        skill.Level = item.Level;
                        skill.Candidate_ID = Candidate_ID;
                        db.SaveChanges();
                    }

                }
            //}else if(entity.LevelDTOs.Count <= db.Skills.Count(x => x.Candidate_ID == Candidate_ID))
            //{
            //    int entity_count = entity.LevelDTOs.Count;
            //    int dem = 0;
            //    foreach (var item in entity.LevelDTOs)
            //    {
            //        if (dem <= entity_count && item.MainSkill != null)
            //        {
            //            var skill = db.Skills.Find(item.ID);
            //            skill.MainSkill = item.MainSkill;
            //            skill.Level = item.Level;
            //            skill.Candidate_ID = Candidate_ID;
            //            db.SaveChanges();
            //        }
            //        else
            //        {
            //            var skill = db.Skills.Find(item.ID);
            //            db.Skills.Remove(skill);
            //            db.SaveChanges();
            //        }
            //        dem++;

            //    }
            //}

            //if (entity.ExperienceDTOs.Count > db.Experiences.Count(x => x.Candidate_ID == Candidate_ID))
            //{
            //    int exp_count = db.Experiences.Count(x => x.Candidate_ID == Candidate_ID);
            //    int dem = 0;

                foreach (var item in entity.ExperienceDTOs)
                {
                    
                    if(item.Position == null || item.Place == null || item.Descriptions == null)
                    {
                        var ex = db.Experiences.Find(item.ID);
                        db.Experiences.Remove(ex);
                        db.SaveChanges();
                    }
                    else if(item.ID == 0)
                    {
                        var ex = new Experience();
                        ex.Position = item.Position;
                        ex.Place = item.Place;
                        ex.FromDate = item.FromDate;
                        ex.ToDate = item.ToDate;
                        ex.Descriptions = item.Descriptions;
                        ex.Candidate_ID = Candidate_ID;
                        db.Experiences.Add(ex);
                        db.SaveChanges();
                    }
                    else
                    {
                        var ex = db.Experiences.Find(item.ID);
                        ex.Position = item.Position;
                        ex.Place = item.Place;
                        ex.FromDate = item.FromDate;
                        ex.ToDate = item.ToDate;
                        ex.Descriptions = item.Descriptions;
                        ex.Candidate_ID = Candidate_ID;
                        db.SaveChanges();
                    }
                }

            //}
            //else
            //{
            //    int entity_count = entity.ExperienceDTOs.Count;
            //    int dem = 0;

            //    foreach (var item in entity.ExperienceDTOs)
            //    {
            //        if (dem <= entity_count && item.Position != null)
            //        {
            //            var ex = db.Experiences.Find(item.ID);
            //            ex.Position = item.Position;
            //            ex.Place = item.Place;
            //            ex.FromDate = item.FromDate;
            //            ex.ToDate = item.ToDate;
            //            ex.Descriptions = item.Descriptions;
            //            ex.Candidate_ID = Candidate_ID;
            //            db.SaveChanges();
            //        }
            //        else
            //        {
            //            var ex = db.Experiences.Find(item.ID);
            //            db.Experiences.Remove(ex);
            //            db.SaveChanges();
            //        }
            //        dem++;
            //    }
            //}

            //if (entity.EducationDTOs.Count > db.Educations.Count(x => x.Candidate_ID == Candidate_ID))
            //{
            //    int educ_count = db.Experiences.Count(x => x.Candidate_ID == Candidate_ID);
            //    int dem = 0;
                foreach (var item in entity.EducationDTOs)
                {
                    
                    if(item.Education == null || item.School == null || item.Descriptions_Educ == null)
                    {
                        var educ = db.Educations.Find(item.ID);
                        db.Educations.Remove(educ);
                        db.SaveChanges();
                    }
                    else if(item.ID == 0)
                    {
                        var educ = new Education();
                        educ.Education1 = item.Education;
                        educ.School = item.School;
                        educ.FromDate = item.FromDate_Educ;
                        educ.ToDate = item.ToDate_Educ;
                        educ.Descriptions = item.Descriptions_Educ;
                        educ.Candidate_ID = Candidate_ID;
                        db.Educations.Add(educ);
                        db.SaveChanges();
                    }
                    else
                    {
                        var educ = db.Educations.Find(item.ID);
                        educ.Education1 = item.Education;
                        educ.School = item.School;
                        educ.FromDate = item.FromDate_Educ;
                        educ.ToDate = item.ToDate_Educ;
                        educ.Descriptions = item.Descriptions_Educ;
                        educ.Candidate_ID = Candidate_ID;
                        db.SaveChanges();
                    }
                }
            //}
            //else
            //{
            //    int entity_count = entity.EducationDTOs.Count;
            //    int dem = 0;
            //    foreach (var item in entity.EducationDTOs)
            //    {

            //        if (dem <= entity_count && item.Education != null)
            //        {
            //            var educ = db.Educations.Find(item.ID);
            //            educ.Education1 = item.Education;
            //            educ.School = item.School;
            //            educ.FromDate = item.FromDate_Educ;
            //            educ.ToDate = item.ToDate_Educ;
            //            educ.Descriptions = item.Descriptions_Educ;
            //            educ.Candidate_ID = Candidate_ID;
            //            db.SaveChanges();
            //        }
            //        else
            //        {
            //            var educ = db.Educations.Find(item.ID);
            //            db.Educations.Remove(educ);
            //            db.SaveChanges();
            //        }
            //        dem++;
            //    }
            //}
                
            
            return RedirectToAction("CV");
        }

        public JsonResult DeleteCV(long ID)
        {
            var candi = db.Candidates.Find(ID);

            new CVBusiness().Del_Skill(candi.ID);

            new CVBusiness().Del_Exper(candi.ID);

            new CVBusiness().Del_Educ(candi.ID);
            
            return Json(new
            {
                status = true
            });
        }

        public ActionResult success()
        {
            return View();
        }
    }
}