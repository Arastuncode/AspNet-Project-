using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.ViewModels.Admin
{
    public class CourseVM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string Name { get; set; }
        public string Desc { get; set; }
        public int DetailId { get; set; }
        public string About { get; set; }
        public string Apply { get; set; }
        public string Certification { get; set; }
        public DateTime Starts { get; set; }
        public string Duration { get; set; }
        public string Class { get; set; }
        public string Skill { get; set; }
        public string Language { get; set; }
        public int Students { get; set; }
        public string Assesments { get; set; }
        public int Fee { get; set; }

    }
}
