using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.ViewModels.Admin
{
    public class EventsVM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Desc { get; set; }
    }
}
