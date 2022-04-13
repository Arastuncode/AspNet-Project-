using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.Utilities.Helper
{
    public class EmailRequest
    {
        public string SecretKey { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}
