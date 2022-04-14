using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Models
{
    public class TeacherSkill:BaseEntity
    {
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public int SkillId { get; set; }
        public Skill Skill { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
