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

namespace AspProject.Areas.AdminArea.Controllers.Home
{
    [Area("AdminArea")]
    public class TestimonialController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public TestimonialController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var testimonials = await _context.Testimonials.ToListAsync();
            return View(testimonials);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestimonialVM testimonialVM)
        {
            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Desc"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Name"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Position"].ValidationState == ModelValidationState.Invalid) return View();
            if (!testimonialVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File type is wrong");
                return View();
            }
            if (!testimonialVM.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "_" + testimonialVM.Photo.FileName;
            string path = Helper.GetFilePath(_env.WebRootPath, "img/testimonial", fileName);
            using(FileStream stream = new FileStream(path, FileMode.Create))
            {
                await testimonialVM.Photo.CopyToAsync(stream);
            }
            Testimonial testimonial = new Testimonial
            {
                Imge=fileName,
                Name=testimonialVM.Name,
                Desc=testimonialVM.Desc,
                Position=testimonialVM.Position,
            };
            await _context.Testimonials.AddAsync(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<Testimonial> GetTestimonialById(int id)
        {
            return await _context.Testimonials.FindAsync(id);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var testimonial = await GetTestimonialById(id);
            if (testimonial is null) return NotFound();
            return View(testimonial);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Testimonial testimonial = await _context.Testimonials.Where(m => m.Id == id).FirstOrDefaultAsync();
            _context.Testimonials.Remove(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            Testimonial testimonial = await _context.Testimonials.Where(m => m.Id == id).FirstOrDefaultAsync();
            TestimonialVM testimonialVM = new TestimonialVM
            {
                Imge = testimonial.Imge,
                Desc = testimonial.Desc,
                Name = testimonial.Name,
                Position=testimonial.Position,
            };
            if (testimonialVM is null) return View();
            return View(testimonialVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,TestimonialVM testimonialVM)
        {
            var dbTestimonial = await GetTestimonialById(id);
            if (dbTestimonial is null) return NotFound();
            if (!ModelState.IsValid) return View();
            if (!testimonialVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File type is wrong");
                return View();
            }
            if (!testimonialVM.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View();
            }
            string path = Helper.GetFilePath(_env.WebRootPath, "img/testimonial", dbTestimonial.Imge);
            Helper.DeleteFile(path);
            string fileName = Guid.NewGuid().ToString() + "_" + testimonialVM.Photo.FileName;
            string newPath = Helper.GetFilePath(_env.WebRootPath, "img/testimonial", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await testimonialVM.Photo.CopyToAsync(stream);
            }
            dbTestimonial.Imge = fileName;
            dbTestimonial.Name = testimonialVM.Name;
            dbTestimonial.Position = testimonialVM.Position;
            dbTestimonial.Desc = testimonialVM.Desc;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
