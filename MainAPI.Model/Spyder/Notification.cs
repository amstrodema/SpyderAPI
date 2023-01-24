using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class Notification
    {
        public Guid ID { get; set; }
        public Guid SenderID { get; set; }
        public Guid RecieverID { get; set; }
        public bool IsSpyder { get; set; }
        public bool IsRead { get; set; }
        public string Message { get; set; }

        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; } = default;
        public DateTime DateModified { get; set; } = default;
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
