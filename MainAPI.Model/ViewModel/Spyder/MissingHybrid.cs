using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel.Spyder
{
    public class MissingHybrid
    {
        public Guid ID { get; set; }
        public Guid ItemTypeID { get; set; }
        public string ItemTypeName { get; set; }
        public Guid CountryID { get; set; }
        public string CountryName { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string FullInfo { get; set; }
        public string Image { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }

        public string LastSeen { get; set; }

        public int AwarenessTypeNo { get; set; }

        public Guid CreatedBy { get; set; }
        public string Author { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
