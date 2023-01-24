using MainAPI.Data.Interface.Examina;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Examina
{
    public class ScheduleRepository : GenericRepository<Schedule>, ISchedule
    {
        public ScheduleRepository(MainAPIContext db) : base(db) { }

        public async Task<IEnumerable<Schedule>> GetActiveSchedules()
        {
            return await GetBy(u => u.IsActive == true);
        }
        public async Task<IEnumerable<Schedule>> GetActiveSchedulesByNodeID(Guid nodeID)
        {
            return await GetBy(u => u.NodeID == nodeID && u.IsActive == true);
        }
        public async Task<IEnumerable<Schedule>> GetSchedulesByNodeID(Guid NodeID)
        {
            return await GetBy(u => u.NodeID == NodeID);
        }
        public async Task<IEnumerable<Schedule>> GetSchedulesByExamID(Guid examID)
        {
            return await GetBy(u => u.ExamID == examID);
        }
    }
}
