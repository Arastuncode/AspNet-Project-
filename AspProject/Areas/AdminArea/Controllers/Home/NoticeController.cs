using AspProject.Data;
using AspProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Areas.AdminArea.Controllers.Home
{
    [Area("AdminArea")]

    public class NoticeController : Controller
    {
        private readonly AppDbContext _context;
        public NoticeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var notice = await _context.Notices.ToListAsync();
            return View(notice);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Notice notice)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //bool isExist = _context.Notices.Any(m => m.Text.ToLower().Trim() == notice.Text.ToLower().Trim());
            //if (isExist)
            //{
            //    ModelState.AddModelError("Text", "This text is already used");
            //    return View();
            //}
            await _context.Notices.AddAsync(notice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int id)
        {
            var notice = await GetNoticeById(id);
            if (notice is null) return NotFound();
            return View(notice);
        }
        private async Task<Notice> GetNoticeById(int id)
        {
            return await _context.Notices.FindAsync(id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Notice notice = await _context.Notices.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (notice is null) return NotFound();
            _context.Notices.Remove(notice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            Notice notice = await _context.Notices.Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
            if (notice is null) return NotFound();
            return View(notice);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Notice notice)
        {
            if (!ModelState.IsValid) return View();
            if (id != notice.Id) return BadRequest();
            try
            {
                Notice dbNotice = await _context.Notices.AsNoTracking().Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
                if (dbNotice.Text.ToLower().Trim() == notice.Text.ToLower().Trim())
                {
                    return RedirectToAction(nameof(Index));
                }
                bool isExist = _context.Notices.Where(m => !m.IsDeleted).Any(m => m.Text.ToLower().Trim() == notice.Text.ToLower().Trim());
                if (isExist)
                {
                    ModelState.AddModelError("Text", "This text is already used");
                    return View();
                }
                _context.Notices.Update(notice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
    }
}
