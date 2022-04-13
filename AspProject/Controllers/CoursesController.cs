using AspProject.Data;
using AspProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _context;
        public CoursesController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var course = await _context.Courses.ToListAsync();
            return View(course);
        }
        private async Task<Course> GetCourserById(int id)
        {
            return await _context.Courses.FindAsync(id);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Course course = await _context.Courses.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (course is null) return NotFound();
            return View(course);
        }
    }
}
