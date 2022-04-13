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
        [Required]
        public IFormFile Photos { get; set; }
        public string Image { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int CoursDetailId { get; set; }
        public string About { get; set; }

    }
}
