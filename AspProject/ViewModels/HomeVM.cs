using AspProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public SliderDetail SliderDetails { get; set; }
        public List<Service> Services { get; set; }
        public List<About> Abouts { get; set; }
        public List<Courses> Courses { get; set; }
    }
}
