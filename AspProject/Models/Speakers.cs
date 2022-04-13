using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Models
{
    public class Speakers:BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Position { get; set; }
        public string Majorty { get; set; }
    }
}
