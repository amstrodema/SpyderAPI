using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class Missing
    {
        public Guid ID { get; set; }
        public Guid ItemTypeID { get; set; }
        public Guid CountryID { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string FullInfo { get; set; }
        public string Image { get; set; }

        public string LastSeen { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Update { get; set; }
        public DateTime UpdateTime { get; set; }

        public int AwarenessTypeNo { get; set; }

        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
