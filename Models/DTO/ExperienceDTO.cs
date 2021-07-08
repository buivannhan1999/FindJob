using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindJob.Models.DTO
{
    public class ExperienceDTO
    {
        public long ID { get; set; }
        public string Position { get; set; }
        public string Place { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Descriptions { get; set; }
    }
}