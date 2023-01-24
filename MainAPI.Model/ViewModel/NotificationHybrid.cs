using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel
{
    public class NotificationHybrid
    {
        public IEnumerable<NotificationVM> NotificationVMs;
        public int UnReadInbox { get; set; }
        public int UnReadNotification { get; set; }
    }
}
