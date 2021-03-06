using AspProject.Data;
using AspProject.Models;
using AspProject.Services;
using AspProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly LayoutService _layoutService;
      
        public HomeController(AppDbContext context, LayoutService layoutService)
        {
            _context = context;
            _layoutService = layoutService;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.ToListAsync();
            SliderDetail detail = await _context.SliderDetails.FirstOrDefaultAsync();
            List<Service> services = await _context.Services.ToListAsync();
            List<About> abouts = await _context.Abouts.ToListAsync();
            List<Course> courses = await _context.Courses.ToListAsync();
            List<Notice> notices = await _context.Notices.ToListAsync();
            List<Event> events = await _context.Events.ToListAsync();
            List<Speakers> speakers = await _context.Speakers.ToListAsync();
            List<Testimonial> testimonials = await _context.Testimonials.ToListAsync();
            List<Blog> blogs = await _context.Blogs.ToListAsync();
            HomeVM homeVM = new HomeVM
            {
                Sliders = sliders,
                SliderDetails= detail,
                Services = services,
                Abouts=abouts,
                Courses=courses,
                Notices=notices,
                Events=events,
                Speakers=speakers,
                Testimonials=testimonials,
                Blogs=blogs,
            };

            return View(homeVM);
        }
       
    }
}
