using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspProject.Models
{
    public class Slider:BaseEntity
    {
        [Required]
        public string Image { get; set; }
        public int SliderDetailId { get; set; }
        public SliderDetail sliderDetails { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
    }
}
