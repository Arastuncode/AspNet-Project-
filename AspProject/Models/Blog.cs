using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Models
{
    public class Blog:BaseEntity
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Writer { get; set; }
        public DateTime Date { get; set; }
        public string Desc { get; set; }
    }
}
