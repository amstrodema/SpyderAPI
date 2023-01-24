using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models
{
    public class StatusEnum
    {
        public Guid ID { get; set; }
        public int Code { get; set; }
        public string Status { get; set; }
    }
}
