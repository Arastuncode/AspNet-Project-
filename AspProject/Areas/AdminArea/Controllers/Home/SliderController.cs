using AspProject.Data;
using AspProject.Models;
using AspProject.Utilities.File;
using AspProject.Utilities.Helper;
using AspProject.ViewModels.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.ToListAsync();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderVM sliderVM)
        {
            if (ModelState["Photos"].ValidationState == ModelValidationState.Invalid) return View();
            if (!sliderVM.Photos.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View();
            }
            if (!sliderVM.Photos.CheckFileSize(1500))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "_" + sliderVM.Photos.FileName;
            string path = Helper.GetFilePath(_env.WebRootPath, "img/slider", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await sliderVM.Photos.CopyToAsync(stream);

            };
            Slider slider = new Slider
            {
                Image = fileName,
                SliderDetailId = 1
            };
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var slider = await GetSliderById(id);
            if (slider is null) return NotFound();
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, SliderVM sliderVM)
        {
            var dbSlider = await GetSliderById(Id);
            if (dbSlider == null) return NotFound();
            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View();
            if (!sliderVM.Photos.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View(dbSlider);
            }
            if (!sliderVM.Photos.CheckFileSize(1400))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View(dbSlider);
            }
            string path = Helper.GetFilePath(_env.WebRootPath, "img/slider", dbSlider.Image);
            Helper.DeleteFile(path);
            string fileName = Guid.NewGuid().ToString() + "_" + sliderVM.Photos.FileName;
            string newPath = Helper.GetFilePath(_env.WebRootPath, "img/slider", fileName);
            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await sliderVM.Photos.CopyToAsync(stream);
            }
            dbSlider.Image = fileName;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private async Task<Slider> GetSliderById(int id)
        {
            return await _context.Sliders.FindAsync(id);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var slider = await GetSliderById(id);
            if (slider is null) return NotFound();
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Slider slider = await _context.Sliders.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (slider is null) return NotFound();
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
