using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Models
{
    public class Categories:BaseEntity
    {
        public string Category { get; set; }
        public int Count { get; set; }
    }
}
