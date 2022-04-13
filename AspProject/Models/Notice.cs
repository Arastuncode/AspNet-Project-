using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Models
{
    public class Notice :BaseEntity
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
