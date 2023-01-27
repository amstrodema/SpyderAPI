using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel
{
    public class NotificationVM
    {
        public Guid ID { get; set; }
        public string Sender { get; set; }
        public bool IsSpyder { get; set; }
        public bool IsRead { get; set; }
        public string Message { get; set; }
        public int NoOfUnread { get; set; }

        public bool IsActive { get; set; }
        public string DateCreated { get; set; } = default;
        public DateTime Date { get; set; }
    }
}
