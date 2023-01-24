using MainAPI.Data.Interface.Examina;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Examina
{
    public class SettingRepository : GenericRepository<Setting>, ISetting
    {
        public SettingRepository(MainAPIContext db) : base(db) { }
        public async Task<Setting> GetSettingByKey(string key)
        {
            return await GetOneBy(u => u.Key == key);
        }

        public async Task<IEnumerable<Setting>> GetSettingsByNodeID(Guid nodeID)
        {
            return await GetBy(u => u.NodeID == nodeID);
        }
    }
}
