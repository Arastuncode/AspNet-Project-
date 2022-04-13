using AspProject.Data;
using AspProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        public EventController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.ToListAsync();
            return View(events);
        }
        private async Task<Event> GetEventsById(int id)
        {
            return await _context.Events.FindAsync(id);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Event events = await _context.Events.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (events is null) return NotFound();
            return View(events);
        }
    }
}
