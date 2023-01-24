using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel
{
   public class UserSession
    {
        public Guid UserID { get; set; }
        public string EmailAddress { get; set; }
        public string Fullname { get; set; }
    }
}
