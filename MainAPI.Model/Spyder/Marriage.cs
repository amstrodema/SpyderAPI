using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class Marriage
    {
        public Guid ID { get; set; }
        public Guid CountryID { get; set; }
        public string GroomFName { get; set; }
        public string GroomLName { get; set; }
        public string BrideFName { get; set; }
        public string BrideLName { get; set; }
        public string Type { get; set; }
        public string CertNo { get; set; }
        public string Status { get; set; }
        public string Toast { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Image { get; set; }
        public string WeddingDate { get; set; }

        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
    }
}
