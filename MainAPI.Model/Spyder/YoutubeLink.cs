using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class YoutubeLink
    {
        public Guid ID { get; set; }
        public string ItemID { get; set; }
        public string Link { get; set; }

        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; } = default;
        public DateTime DateModified { get; set; } = default;
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
