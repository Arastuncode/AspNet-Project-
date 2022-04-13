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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Areas.AdminArea.Controllers.Home
{
    [Area("AdminArea")]
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public AboutController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var about = await _context.Abouts.ToListAsync();
            return View(about);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutVM aboutVM)
        {
            if(ModelState["Photos"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Description"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Title"].ValidationState == ModelValidationState.Invalid) return View();
            if (!aboutVM.Photos.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File type is wrong");
                return View();
            }
            if (!aboutVM.Photos.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "_" + aboutVM.Photos.FileName;
            string path = Helper.GetFilePath(_env.WebRootPath, "img/about", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await aboutVM.Photos.CopyToAsync(stream);
            }
            About about = new About
            {
                Image=fileName,
                Description=aboutVM.Description,
                Title=aboutVM.Title,
            };
            await _context.Abouts.AddAsync(about);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var about = await GetAboutById(id);
            if (about is null) return NotFound();
            return View(about);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id,About about)
        {
            var dbAbout = await GetAboutById(Id);
            if (dbAbout == null) return NotFound();
            if (ModelState["Photos"].ValidationState == ModelValidationState.Invalid) return View();
            if (!about.Photos.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View(dbAbout);
            }
            if (!about.Photos.CheckFileSize(800))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View(dbAbout);
            }
            string path = Helper.GetFilePath(_env.WebRootPath, "img/about", dbAbout.Image);
            Helper.DeleteFile(path);
            string fileName = Guid.NewGuid().ToString() + "_" + about.Photos.FileName;
            string newPath = Helper.GetFilePath(_env.WebRootPath, "img/about", fileName);
            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await about.Photos.CopyToAsync(stream);
            }
            dbAbout.Image = fileName;
            ///////
           
            //if (!ModelState.IsValid) return View();
            //if (Id != about.Id) return BadRequest();

            //About dbAboutt = await _context.Abouts.AsNoTracking().Where(m => !m.IsDeleted && m.Id == Id).FirstOrDefaultAsync();
            //if (dbAboutt.Title.ToLower().Trim() == about.Title)
            //{
            //    return RedirectToAction(nameof(Index));
            //}
            //bool isTitleExist = _context.Abouts.Where(m => !m.IsDeleted).Any(m => m.Title.ToLower().Trim() == about.Title.ToLower().Trim());
            //if (isTitleExist)
            //{
            //    ModelState.AddModelError("Name", "This title is already available");
            //    return View();
            //}
            //bool isTextExist = _context.Abouts.Where(m => !m.IsDeleted).Any(m => m.Description.ToLower().Trim() == about.Description.ToLower().Trim());
            //if (isTextExist)
            //{
            //    ModelState.AddModelError("Name", "This text is already available");
            //    return View();
            //}
            //_context.Abouts.Update(about);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));


            ///
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Detail(int id)
        {
            var about = _context.Abouts.FirstOrDefault(m => m.Id == id);
            return View(about);
        }
        private async Task<About> GetAboutById(int id)
        {
            return await _context.Abouts.FindAsync(id);
        }
    }
}
