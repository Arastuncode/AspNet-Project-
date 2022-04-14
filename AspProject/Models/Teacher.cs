using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Models
{
    public class Teacher:BaseEntity
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string About { get; set; }
        public string Degree { get; set; }
        public int Experience { get; set; }
        public string Faculty { get; set; }
        public string Mail { get; set; }
        public int Number { get; set; }
        public string Skype { get; set; }
        public List<TeacherSkill> Skills { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
