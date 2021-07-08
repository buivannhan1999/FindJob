using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindJob.Models.DTO
{
    public class EducationDTO
    {
        public long ID { get; set; }
        public string Education { get; set; }
        public string School { get; set; }
        public DateTime FromDate_Educ { get; set; }
        public DateTime ToDate_Educ { get; set; }
        public string Descriptions_Educ { get; set; }
    }
}