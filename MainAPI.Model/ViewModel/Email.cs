using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel
{
    public class Email
    {
        public string Sender { get; set; }
        public List<string> Recipients { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string DisplayName { get; set; }
    }
}
