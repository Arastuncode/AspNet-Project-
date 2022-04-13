using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspProject.ViewModels.Admin
{
    public class AboutVM 
    {
        public int Id { get; set; }
        [Required]
        public IFormFile Photos { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
