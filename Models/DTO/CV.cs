using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindJob.Models.DTO
{
    public class CV
    {
        public List<LevelDTO> LevelDTOs { get; set; }
        public List<EducationDTO> EducationDTOs { get; set; }
        public List<ExperienceDTO> ExperienceDTOs { get; set; }
    }
}