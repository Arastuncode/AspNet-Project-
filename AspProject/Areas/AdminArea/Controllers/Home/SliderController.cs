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
            List<Slider> sliders = await _context.Sliders.AsNoTracking().ToListAsync();
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
            if (ModelState["Description"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Title"].ValidationState == ModelValidationState.Invalid) return View();
            foreach (var photo in sliderVM.Photos)
            {
                if (!photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Image type is wrong");
                    return View();
                }
                if (!photo.CheckFileSize(1500))
                {
                    ModelState.AddModelError("Photo", "Image size is wrong");
                    return View();
                }
            }
            foreach (var photo in sliderVM.Photos)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string path = Helper.GetFilePath(_env.WebRootPath, "img/slider", fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);

                };
                Slider slider = new Slider
                {
                    Image = fileName,
                    SliderDetailId = 1
                };
                await _context.Sliders.AddAsync(slider);
            }
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
    }
}
