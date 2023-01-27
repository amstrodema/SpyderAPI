using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class RequestObject<T>
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string ItemID { get; set; }
        public Guid AuthorID { get; set; }
        public Guid AppID { get; set; }
        public string CountryID { get; set; }
        public int Flag { get; set; }
        public T Data { get; set; }
    }
}
