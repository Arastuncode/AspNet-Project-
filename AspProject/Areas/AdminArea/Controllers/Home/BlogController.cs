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
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var blog = await _context.Blogs.ToListAsync();
            return View(blog);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogVM blogVM)
        {
            if(ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Desc"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Name"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Date"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Writer"].ValidationState == ModelValidationState.Invalid) return View();
            if (!blogVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File type is wrong");
                return View();
            }
            if (!blogVM.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "_" + blogVM.Photo.FileName;
            string path = Helper.GetFilePath(_env.WebRootPath, "img/blog", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await blogVM.Photo.CopyToAsync(stream);
            }
            Blog blog = new Blog 
            {
                Image=fileName,
                Name=blogVM.Name,
                Desc=blogVM.Desc,
                Date=blogVM.Date,
                Writer=blogVM.Writer,
            };
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<Blog> GetBlogById(int id)
        {
            return await _context.Blogs.FindAsync(id);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var blog = await GetBlogById(id);
            if (blog is null) return NotFound();
            return View(blog);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Blog blog = await _context.Blogs.Where(m => m.Id == id).FirstOrDefaultAsync();
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            Blog blog = await _context.Blogs.Where(m => m.Id == id).FirstOrDefaultAsync();
            BlogVM blogVM = new BlogVM
            {
                Image = blog.Image,
                Desc = blog.Desc,
                Name = blog.Name,
                Date= blog.Date,
                Writer= blog.Writer,
            };
            if (blogVM is null) return View();
            return View(blogVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,BlogVM blogVM)
        {
            var dbBlog = await GetBlogById(id);
            if (dbBlog is null) return NotFound();
            if (!ModelState.IsValid) return View();
            if (!blogVM.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View();
            }
            if (!blogVM.Photo.CheckFileType("imgae"))
            {
                ModelState.AddModelError("Photo", "File type is wrong");
                return View();
            }
            string path = Helper.GetFilePath(_env.WebRootPath, "img/blog", dbBlog.Image);
            Helper.DeleteFile(path);
            string fileName = Guid.NewGuid().ToString() + "_" + blogVM.Photo.FileName;
            string newPath = Helper.GetFilePath(_env.WebRootPath, "img/blog", fileName);
            using(FileStream stream = new FileStream(path, FileMode.Create))
            {
                await blogVM.Photo.CopyToAsync(stream);
            }
            dbBlog.Image = fileName;
            dbBlog.Name = blogVM.Name;
            dbBlog.Desc = blogVM.Desc;
            dbBlog.Date = blogVM.Date;
            dbBlog.Writer = blogVM.Writer;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
