using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class Death
    {
        public Guid ID { get; set; }
        public Guid CountryID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CauseOfDeath { get; set; }
        public string DetailsOfPerson { get; set; }
        public string DeathCertNo { get; set; }
        public string BirthDate { get; set; }
        public string DeathDate { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Image { get; set; }

        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
    }
}
