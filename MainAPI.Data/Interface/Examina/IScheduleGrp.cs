using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Examina
{
    public interface IScheduleGrp : IGeneric<ScheduleGroup>
    {
        Task<IEnumerable<ScheduleGroup>> GetSchedulesGroupByScheduleID(Guid ScheduleID);
        Task<ScheduleGroup> GetScheduleGroupByScheduleID(Guid ScheduleID);
        Task<IEnumerable<ScheduleGroup>> GetScheduleGroupsByNodeID(Guid nodeID);
    }
}
