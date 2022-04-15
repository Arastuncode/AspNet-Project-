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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Id)
        {
            Tags tags = await _context.Tags.Where(m => m.Id == Id).FirstOrDefaultAsync();
            if (tags == null) return NotFound();
            _context.Tags.Remove(tags);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int Id)
        {
            Tags tags = await _context.Tags.Where(m => m.Id == Id).FirstOrDefaultAsync();
            if (tags == null) return NotFound();
            return View(tags);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, Tags tags)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (Id != tags.Id) return NotFound();
            Tags dbTags = await _context.Tags.AsNoTracking().Where(m => m.Id == Id).FirstOrDefaultAsync();
            if (dbTags.Name.ToLower().Trim() == tags.Name.ToLower().Trim())
            {
                return RedirectToAction(nameof(Index));
            }
            bool isExist = _context.Tags.Any(m => m.Name.ToLower().Trim() == tags.Name.ToLower().Trim());
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu Skill artiq movcuddur");
                return View();
            }
            _context.Update(tags);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
