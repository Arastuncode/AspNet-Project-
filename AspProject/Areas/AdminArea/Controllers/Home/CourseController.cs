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
            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Desc"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Name"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["About"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Apply"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Certification"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Starts"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Duration"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Class"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Skill"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Language"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Students"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Assesments"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Fee"].ValidationState == ModelValidationState.Invalid) return View();
            if (!courseVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View();
            }
            if (!courseVM.Photo.CheckFileSize(1500))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "_" + courseVM.Photo.FileName;
            string path = Helper.GetFilePath(_env.WebRootPath, "img/course", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await courseVM.Photo.CopyToAsync(stream);
            };
            Course courses = new Course
            {
                Image = fileName,
                Desc = courseVM.Desc,
                Name = courseVM.Name,
                About=courseVM.About,
                Apply=courseVM.Apply,
                Certification=courseVM.Certification,
                Starts=courseVM.Starts,
                Duration = courseVM.Duration,
                Class = courseVM.Class,
                Skill = courseVM.Skill,
                Language = courseVM.Language,
                Students = courseVM.Students,
                Assesments = courseVM.Assesments,
                Fee = courseVM.Fee,
            };
            await _context.Courses.AddAsync(courses);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<Course> GetCourseById(int id)
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
            Course course = await _context.Courses.Where(m => m.Id == id).FirstOrDefaultAsync();
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            Course course = await _context.Courses.Where(m => m.Id == id).FirstOrDefaultAsync();
            CourseVM courseVM = new CourseVM
            {
                Image = course.Image,
                Desc = course.Desc,
                Name = course.Name,
                About = course.About,
                Apply = course.Apply,
                Certification = course.Certification,
                Starts = course.Starts,
                Duration = course.Duration,
                Class = course.Class,
                Skill = course.Skill,
                Language = course.Language,
                Students = course.Students,
                Assesments = course.Assesments,
                Fee = course.Fee,
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
            if (!courseVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View(dbCourse);
            }
            if (!courseVM.Photo.CheckFileSize(1500))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View(dbCourse);
            }
            string path = Helper.GetFilePath(_env.WebRootPath, "img/course", dbCourse.Image);
            Helper.DeleteFile(path);
            string fileName = Guid.NewGuid().ToString() + "_" + courseVM.Photo.FileName;
            string newPath = Helper.GetFilePath(_env.WebRootPath, "img/course", fileName);
            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await courseVM.Photo.CopyToAsync(stream);
            }
            dbCourse.Image = fileName;
            dbCourse.Desc = courseVM.Desc;
            dbCourse.Name = courseVM.Name;
            dbCourse.About = courseVM.About;
            dbCourse.Apply = courseVM.Apply;
            dbCourse.Certification = courseVM.Certification;
            dbCourse.Starts = courseVM.Starts;
            dbCourse.Duration = courseVM.Duration;
            dbCourse.Class = courseVM.Class;
            dbCourse.Skill = courseVM.Skill;
            dbCourse.Language = courseVM.Language;
            dbCourse.Students = courseVM.Students;
            dbCourse.Assesments = courseVM.Assesments;
            dbCourse.Fee = courseVM.Fee;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
