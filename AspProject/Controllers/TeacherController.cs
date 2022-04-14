using AspProject.Data;
using AspProject.Models;
using AspProject.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;
        public TeacherController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int Id)
        {
            Teacher teacher = await _context.Teachers.Where(m => m.Id == Id).FirstOrDefaultAsync();
            List<TeacherSkill> teacherSkills = await _context.TeacherSkills.Where(m => m.TeacherId == Id).ToListAsync();
            List<Skill> skillsData = new List<Skill>();
            List<int> skillsPercent = new List<int>();
            foreach (var skill in teacherSkills)
            {
                Skill skills = await _context.Skills.Where(m => m.Id == skill.SkillId).FirstOrDefaultAsync();
                skillsData.Add(skills);
                skillsPercent.Add(skill.Percent);
            }
            TeacherDetailVM teacherDetail = new TeacherDetailVM
            {
                teacher = teacher,  
                skills = skillsData,
                percents = skillsPercent
                
            };
            return View(teacherDetail);
        }
    }
}
