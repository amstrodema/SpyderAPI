using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models
{
   public class Email
    {
        public string senderEmail { get; set; }
        public string senderPassword { get; set; }
        public string recipientEmail { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }
}
