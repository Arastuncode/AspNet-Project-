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
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CourseController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var course = await _context.Courses.ToListAsync();
            return View(course);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseVM courseVM)
        {
            if (ModelState["Photos"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Description"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Name"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["About"].ValidationState == ModelValidationState.Invalid) return View();
            if (!courseVM.Photos.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View();
            }
            if (!courseVM.Photos.CheckFileSize(1500))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "_" + courseVM.Photos.FileName;
            string path = Helper.GetFilePath(_env.WebRootPath, "img/course", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await courseVM.Photos.CopyToAsync(stream);
            };
            Courses courses = new Courses
            {
                Image = fileName,
                Name = courseVM.Name,
                Desc = courseVM.Description,
                About=courseVM.About,
            };
            await _context.Courses.AddAsync(courses);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<Courses> GetCourseById(int id)
        {
            return await _context.Courses.FindAsync(id);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var course = await GetCourseById(id);
            if (course is null) return NotFound();
            return View(course);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Courses course = await _context.Courses.Where(m => m.Id == id).FirstOrDefaultAsync();
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            Courses course = await _context.Courses.Where(m => m.Id == id).FirstOrDefaultAsync();
            CourseVM courseVM = new CourseVM
            {
                Image = course.Image,
                Name = course.Name,
                Description = course.Desc,
                About=course.About,
            };
            if (courseVM is null) return View();
            return View(courseVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseVM courseVM)
        {
            var dbCourse = await GetCourseById(id);
            if (dbCourse is null) return NotFound();
            if (!ModelState.IsValid) return View();
            if (!courseVM.Photos.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View(dbCourse);
            }
            if (!courseVM.Photos.CheckFileSize(1500))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View(dbCourse);
            }
            string path = Helper.GetFilePath(_env.WebRootPath, "img/course", dbCourse.Image);
            Helper.DeleteFile(path);
            string fileName = Guid.NewGuid().ToString() + "_" + courseVM.Photos.FileName;
            string newPath = Helper.GetFilePath(_env.WebRootPath, "img/course", fileName);
            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await courseVM.Photos.CopyToAsync(stream);
            }
            dbCourse.Image = fileName;
            dbCourse.Name = courseVM.Name;
            dbCourse.Desc = courseVM.Description;
            dbCourse.About = courseVM.About;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
