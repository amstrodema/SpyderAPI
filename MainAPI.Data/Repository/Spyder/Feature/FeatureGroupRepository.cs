using MainAPI.Data.Interface.Spyder.Feature;
using MainAPI.Models.Spyder.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder.Feature
{
    public class FeatureGroupRepository:GenericRepository<FeatureGroup>,IFeatureGroup
    {
        public FeatureGroupRepository(MainAPIContext db) : base(db) { }
        public async Task<FeatureGroup> GetFeatureGroupByCode(string code)
        {
            return await GetOneBy(p => p.Code == code);
        }
        public async Task<IEnumerable<FeatureGroup>> GetFeatureGroupByGroupNo(int groupNo)
        {
            return await GetBy(p => p.GroupNo == groupNo);
        }
    }
}
