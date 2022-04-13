using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Models
{
    public class Event:BaseEntity
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Desc { get; set; }
    }
}
