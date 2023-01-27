using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class LogInMonitorRepository : GenericRepository<LogInMonitor>, ILogInMonitor
    {
        public LogInMonitorRepository(MainAPIContext db) : base(db) { }

        public async Task<LogInMonitor> GetLogInMonitorByUserID(Guid userID)
        {
            return await GetOneBy(y => y.UserID == userID);
        }
        public async Task<LogInMonitor> GetLogInMonitorByAppIDAndUserID(Guid appID, Guid userID)
        {
            return await GetOneBy(y => y.AppID == appID && y.UserID == userID);
        }
    }
}
