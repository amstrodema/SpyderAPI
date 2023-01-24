using MainAPI.Models.Spyder;
using MainAPI.Models.Spyder.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel.Spyder
{
    public class MissingVM
    {
        public Missing missing { get; set; }
        public MissingDetails missingDetails { get; set; }
        public List<Feature> features { get; set; }
        public List<FeatureVM> featureVMs { get; set; }
    }
}
