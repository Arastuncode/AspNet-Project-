using AspProject.Data;
using AspProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.ViewComponents
{
    public class CategoriesViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        public CategoriesViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Categories> categories = await _context.Categories.Where(m => m.IsDeleted == false).ToListAsync();
            return (await Task.FromResult(View(categories)));
        }
    }
}
