using AspProject.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.ViewModels.Admin
{
    public class TeacherVM
    {
        public IFormFile Image { get; set; }
        public Teacher Teacher { get; set; }
        public List<Skill> Skills { get; set; }
        public List<int> Percents { get; set; }
    }
}
