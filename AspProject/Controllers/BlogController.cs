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
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        public BlogController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var blog = await _context.Blogs.ToListAsync();
            return View(blog);
        }
        private async Task<Blog> GetCourserById(int id)
        {
            return await _context.Blogs.FindAsync(id);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Blog blog = await _context.Blogs.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (blog is null) return NotFound();
            return View(blog);
        }
    }
}
