using AspProject.Data;
using AspProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace AspProject.ViewComponents
{
    public class TeacherViewComponent: ViewComponent
    {
        private readonly AppDbContext _context;
        public TeacherViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int take)
        {

            List<Teacher> teacher = await _context.Teachers.Where(m => m.IsActive == true).Take(take).ToListAsync();
            return (await Task.FromResult(View(teacher)));
        }
    }
}
