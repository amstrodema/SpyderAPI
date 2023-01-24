using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class Image
    {
        public Guid ID { get; set; }
        public Guid ItemID { get; set; }
        public string Small { get; set; }
        public string Medium { get; set; }
        public string Original { get; set; }

        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
