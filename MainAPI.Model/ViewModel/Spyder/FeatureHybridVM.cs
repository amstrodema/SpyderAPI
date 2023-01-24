using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel.Spyder
{
    public class FeatureHybridVM
    {
        public Guid ID { get; set; }
        public Guid ItemID { get; set; }
        public Guid FeatureTypeID { get; set; }
        public Guid FeatureGroupID { get; set; }
        public string FeatureTypeName { get; set; }
        public string FeatureGroupName { get; set; }

        public string Value { get; set; }
    }
}
