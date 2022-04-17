using AspProject.Data;
using AspProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;
        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categories categories)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool isExist = _context.Categories.Any(m => m.Category.ToLower().Trim() == categories.Category.ToLower().Trim());
            if (isExist)
            {
                ModelState.AddModelError("Name", "This category is already created");
                return View();
            }
            await _context.Categories.AddAsync(categories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Id)
        {
            Categories categories = await _context.Categories.Where(m => m.Id == Id).FirstOrDefaultAsync();
            if (categories == null) return NotFound();
            _context.Categories.Remove(categories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int Id)
        {
            Categories categories = await _context.Categories.Where(m => m.Id == Id).FirstOrDefaultAsync();
            if (categories == null) return NotFound();
            return View(categories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, Categories categories)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (Id != categories.Id) return NotFound();
            Categories dbCategory = await _context.Categories.AsNoTracking().Where(m => m.Id == Id).FirstOrDefaultAsync();
            if (dbCategory.Category.ToLower().Trim() == categories.Category.ToLower().Trim())
            {
                return RedirectToAction(nameof(Index));
            }
            bool isExist = _context.Categories.Any(m => m.Category.ToLower().Trim() == categories.Category.ToLower().Trim());
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu Skill artiq movcuddur");
                return View();
            }
            _context.Update(categories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
