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
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public EventController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.ToListAsync();
            return View(events);
        }
        public IActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventsVM eventsVM)
        {
            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Name"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Location"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Date"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["StartTime"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["EndTime"].ValidationState == ModelValidationState.Invalid) return View();
            if (ModelState["Desc"].ValidationState == ModelValidationState.Invalid) return View();
            if (!eventsVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File type is wrong");
                return View();
            }
            if (!eventsVM.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "Image size wrong");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "_" + eventsVM.Photo.FileName;
            string path = Helper.GetFilePath(_env.WebRootPath, "img/event", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await eventsVM.Photo.CopyToAsync(stream);
            }
            Event events = new Event
            {
                Image = fileName,
                Name = eventsVM.Name,
                Location = eventsVM.Location,
                Date = eventsVM.Date,
                StartTime = eventsVM.StartTime,
                EndTime = eventsVM.EndTime,
                Desc=eventsVM.Desc
            };
            await _context.Events.AddAsync(events);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Event events = await _context.Events.Where(m => m.Id == id).FirstOrDefaultAsync();
            _context.Events.Remove(events);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private async Task<Event> GetEventsById(int id)
        {
            return await _context.Events.FindAsync(id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail(int id)
        {
            var events = await GetEventsById(id);
            if (events is null) return NotFound();
            return View(events);
        }
       
        public async Task<IActionResult> Edit(int id)
        {
            Event events = await _context.Events.Where(m => m.Id == id).FirstOrDefaultAsync();
            EventsVM eventsVM = new EventsVM
            {
                Image = events.Image,
                Date = events.Date,
                Name = events.Name,
                Location = events.Location,
                StartTime = events.StartTime,
                EndTime = events.EndTime,
                Desc=events.Desc,
            };
            if (eventsVM is null) return View();
            return View(eventsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,EventsVM eventsVM)
        {
            var dbEvents = await GetEventsById(id);
            if (dbEvents is null) return NotFound();
            if (!ModelState.IsValid) return View();
            if (!eventsVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View(dbEvents);
            }
            if (!eventsVM.Photo.CheckFileSize(1500))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View(dbEvents);
            }
            string path = Helper.GetFilePath(_env.WebRootPath, "img/event", dbEvents.Image);
            Helper.DeleteFile(path);
            string fileName = Guid.NewGuid().ToString() + "_" + eventsVM.Photo.FileName;
            string newPath = Helper.GetFilePath(_env.WebRootPath, "img/event", fileName);
            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await eventsVM.Photo.CopyToAsync(stream);
            }
            dbEvents.Image = fileName;
            dbEvents.Name = eventsVM.Name;
            dbEvents.Location = eventsVM.Location;
            dbEvents.Date = eventsVM.Date;
            dbEvents.StartTime = eventsVM.StartTime;
            dbEvents.EndTime = eventsVM.EndTime;
            dbEvents.Desc = eventsVM.Desc;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
