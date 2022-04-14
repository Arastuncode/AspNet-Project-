using AspProject.Data;
using AspProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.ViewComponents
{
    public class TestimonialViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        public TestimonialViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int take)
        {

            List<Testimonial> testimonials = await _context.Testimonials.Where(m => m.IsDeleted == false).Take(take).ToListAsync();
            return (await Task.FromResult(View(testimonials)));
        }
    }
}
