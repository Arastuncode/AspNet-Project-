using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.ViewModels.Admin
{
    public class TestimonialVM
    {
        public string Imge { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Position { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
    }
}
