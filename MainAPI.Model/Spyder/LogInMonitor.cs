using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class LogInMonitor
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public Guid UserCountryID { get; set; }
        public bool IsUserLoggedIn { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; } = default;
        public DateTime DateModified { get; set; } = default;
    }
}
