using MainAPI.Models.Spyder.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Spyder.Feature
{
    public interface IFeatureType: IGeneric<FeatureType>
    {
        Task<IEnumerable<FeatureType>> GetFeatureTypesByGroup(Guid featureGroupID);
    }
}
