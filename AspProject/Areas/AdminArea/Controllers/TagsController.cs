using AspProject.Data;
using AspProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class TagsController : Controller
    {
        private readonly AppDbContext _context;
        public TagsController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var tags = await _context.Tags.ToListAsync();
            return View(tags);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tags tags)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool isExist = _context.Tags.Any(m => m.Name.ToLower().Trim() == tags.Name.ToLower().Trim());
            if (isExist)
            {
                ModelState.AddModelError("Name", "This category is already created");
                return View();
            }
            await _context.Tags.AddAsync(tags);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
