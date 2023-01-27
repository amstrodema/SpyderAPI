using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Spyder
{
    public interface ILogInMonitor : IGeneric<LogInMonitor>
    {
        Task<LogInMonitor> GetLogInMonitorByUserID(Guid userID);
        Task<LogInMonitor> GetLogInMonitorByAppIDAndUserID(Guid appID, Guid userID);
    }
}
