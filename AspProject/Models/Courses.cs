using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Models
{
    public class Courses:BaseEntity
    {
        public string Image { get; set; }
        [NotMapped]
        [Required]
        public List<IFormFile> Photo { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int DetailId { get; set; }
        public string About { get; set; }
    }
}
