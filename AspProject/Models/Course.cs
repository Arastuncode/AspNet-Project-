using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Models
{
    public class Course:BaseEntity
    {
        public string Image { get; set; }
        [NotMapped]
        [Required]
        public List<IFormFile> Photo { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int DetailId { get; set; }
        public string About { get; set; }
        public string Apply { get; set; }
        public string  Certification { get; set; }
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
