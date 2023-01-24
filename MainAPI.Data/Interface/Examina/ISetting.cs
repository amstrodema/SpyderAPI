using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Examina
{
    public interface ISetting : IGeneric<Setting>
    {
        Task<Setting> GetSettingByKey(string key);
        Task<IEnumerable<Setting>> GetSettingsByNodeID(Guid nodeID);
    }
}
