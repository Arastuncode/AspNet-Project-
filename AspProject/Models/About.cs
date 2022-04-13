using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Models
{
    public class About :BaseEntity
    {
        public string Image { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photos { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
