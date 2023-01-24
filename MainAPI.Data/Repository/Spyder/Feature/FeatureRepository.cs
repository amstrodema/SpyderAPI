using MainAPI.Data.Interface.Spyder.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder.Feature
{
    public class FeatureRepository:GenericRepository<Models.Spyder.Feature.Feature>, IFeature
    {
        public FeatureRepository(MainAPIContext db) : base(db) { }
        public async Task<IEnumerable<Models.Spyder.Feature.Feature>> GetFeaturesByItemID(Guid itemID)
        {
            return await GetBy(p => p.ItemID == itemID);
        }
    }
}
