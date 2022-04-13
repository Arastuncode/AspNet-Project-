using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Models
{
    public class Service:BaseEntity
    {
        [Required(ErrorMessage = "Do not leave empty"), MaxLength(100, ErrorMessage = "Cannot exceed 50 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Do not leave empty"), MaxLength(100, ErrorMessage = "Cannot exceed 50 characters")]
        public string Text { get; set; }

    }
}
