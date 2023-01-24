using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class Settings
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }

        public Guid ViewCountryID { get; set; }
        public string Country { get; set; }
        public bool IsLocalRange { get; set; }

        public bool IsAllowMessaging { get; set; }
        public bool IsRecieveAnoymousMessages { get; set; }
        public bool IsAnoymousMessaging { get; set; }

        public bool IsSendNotificationToMail { get; set; }
        public bool IsReactionNotification { get; set; }

        public bool IsAllowAccess { get; set; }
        public bool IsShowEmail { get; set; }
        public bool IsShowPhoneNo { get; set; }

        //public bool IsAllow { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; } = default;
        public DateTime DateModified { get; set; } = default;
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
