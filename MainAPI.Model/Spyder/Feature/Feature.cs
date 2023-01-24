using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder.Feature
{
    public class Feature
    {
        public Guid ID { get; set; }
        public Guid ItemID { get; set; }
        public Guid FeatureTypeID { get; set; }
        public Guid FeatureGroupID { get; set; }
        public Guid CountryID { get; set; }

        public string Value { get; set; }

        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
