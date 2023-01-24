using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class Request<T>
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public Guid ItemID { get; set; }
        public Guid CountryID { get; set; }
        public bool IsCountry { get; set; }
        public int Flag { get; set; }
        public T Data { get; set; }

    }
}
