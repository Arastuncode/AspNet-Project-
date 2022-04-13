using AspProject.Data;
using AspProject.Models;
using AspProject.Utilities.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Areas.AdminArea.Controllers.Home
{
    [Area("AdminArea")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        public ServiceController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var services = await _context.Services.ToListAsync();
            return View(services);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service services)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool isTitleExist = _context.Services.Any(m => m.Title.ToLower().Trim() == services.Title.ToLower().Trim());
            if (isTitleExist)
            {
                ModelState.AddModelError("Name", "This title is already available");
                return View();
            }
            bool isTextExist = _context.Services.Any(m => m.Text.ToLower().Trim() == services.Text.ToLower().Trim());
            if (isTextExist)
            {
                ModelState.AddModelError("Name", "This text is already available");
                return View();
            }
            await _context.Services.AddAsync(services);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Service");
        }
        public async Task<IActionResult>Detail(int id)
        {
            var service = await GetServiceById(id);
            if (service is null) return NotFound();
            return View(service);
        }
        private async Task<Service> GetServiceById(int id)
        {
            return await _context.Services.FindAsync(id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Service service = await _context.Services.Where(m=>m.Id == id).FirstOrDefaultAsync();
            if (service is null) return NotFound();
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            Service services = await _context.Services.Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
            if (services is null) return NotFound();
            return View(services);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Service service)
        {
            if (!ModelState.IsValid) return View();
            if (id != service.Id) return BadRequest();
            try
            {
                Service dbService = await _context.Services.AsNoTracking().Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
                if (dbService.Title.ToLower().Trim() == service.Title)
                {
                    return RedirectToAction(nameof(Index));
                }
                bool isTitleExist = _context.Services.Where(m => !m.IsDeleted).Any(m => m.Title.ToLower().Trim() == service.Title.ToLower().Trim());
                if (isTitleExist)
                {
                    ModelState.AddModelError("Name", "This title is already available");
                    return View();
                }
                bool isTextExist = _context.Services.Where(m => !m.IsDeleted).Any(m => m.Text.ToLower().Trim() == service.Text.ToLower().Trim());
                if (isTextExist)
                {
                    ModelState.AddModelError("Name", "This text is already available");
                    return View();
                }
                _context.Services.Update(service);
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
