using MainAPI.Data.Interface.Spyder.Feature;
using MainAPI.Models.Spyder.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder.Feature
{
    public class FeatureTypeRepository: GenericRepository<FeatureType>, IFeatureType
    {
        public FeatureTypeRepository(MainAPIContext db) : base(db) { }
        public async Task<IEnumerable<FeatureType>> GetFeatureTypesByGroup(Guid featureGroupID)
        {
            return await GetBy(p => p.FeatureGroupID == featureGroupID);
        }
    }
}
