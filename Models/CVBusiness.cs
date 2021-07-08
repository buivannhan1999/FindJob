using FindJob.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindJob.Models
{
    public class CVBusiness
    {
        private FindJobEntities db = new FindJobEntities();

        public void Del_Skill(long Candidate_ID)
        {
            var lstSkill = db.Skills.Where(x => x.Candidate_ID == Candidate_ID).ToList();
            foreach (var item in lstSkill)
            {
                db.Skills.Remove(item);
                db.SaveChanges();
            }
        }


        public void Del_Exper(long Candidate_ID)
        {
            var lstExper = db.Experiences.Where(x => x.Candidate_ID == Candidate_ID).ToList();
            foreach (var item in lstExper)
            {
                db.Experiences.Remove(item);
                db.SaveChanges();
            }
        }

        public void Del_Educ(long Candidate_ID)
        {
            var lstEduc = db.Educations.Where(x => x.Candidate_ID == Candidate_ID).ToList();
            foreach (var item in lstEduc)
            {
                db.Educations.Remove(item);
                db.SaveChanges();
            }
        }
    }
}