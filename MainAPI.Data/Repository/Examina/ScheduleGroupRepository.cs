using MainAPI.Data.Interface.Examina;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Examina
{
    public class ScheduleGroupRepository : GenericRepository<ScheduleGroup>, IScheduleGrp
    {
        public ScheduleGroupRepository(MainAPIContext db) : base(db) { }

        public async Task<ScheduleGroup> GetScheduleGroupByScheduleID(Guid ScheduleID)
        {
            return await GetOneBy(u => u.ScheduleID == ScheduleID);
        }
        public async Task<IEnumerable<ScheduleGroup>> GetSchedulesGroupByScheduleID(Guid ScheduleID)
        {
            return await GetBy(u => u.ScheduleID == ScheduleID);
        }
        public async Task<IEnumerable<ScheduleGroup>> GetScheduleGroupsByNodeID(Guid nodeID)
        {
            return await GetBy(u => u.NodeID == nodeID);
        }
    }
}
