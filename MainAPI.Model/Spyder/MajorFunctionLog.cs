using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class MajorFunctionLog
    {
        public Guid ID { get; set; }
        public Guid CreatedBy { get; set; }
        public int EventNo { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
