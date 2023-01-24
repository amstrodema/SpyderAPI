using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class SettingsRepository: GenericRepository<Settings>, ISettings
    {
        public SettingsRepository(MainAPIContext db): base(db) { }

        public async Task<Settings> GetByUserID(Guid userID)
        {
            return await GetOneBy(u => u.UserID == userID);
        }
    }
}
