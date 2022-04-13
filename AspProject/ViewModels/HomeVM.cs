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
        public List<Course> Courses { get; set; }
        public List<Notice> Notices { get; set; }
        public List<Event> Events { get; set; }
        public List<Speakers> Speakers { get; set; }
        public List<Testimonial> Testimonials { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
