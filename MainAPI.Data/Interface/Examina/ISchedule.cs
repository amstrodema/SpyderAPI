using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Examina
{
    public interface ISchedule : IGeneric<Schedule>
    {
        Task<IEnumerable<Schedule>> GetActiveSchedules();
        Task<IEnumerable<Schedule>> GetActiveSchedulesByNodeID(Guid nodeID);
        Task<IEnumerable<Schedule>> GetSchedulesByNodeID(Guid NodeID);
        Task<IEnumerable<Schedule>> GetSchedulesByExamID(Guid examID);
    }
}
