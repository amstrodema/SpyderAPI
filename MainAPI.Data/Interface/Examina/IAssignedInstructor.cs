using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Examina
{
    public interface IAssignedInstructor : IGeneric<AssignedInstructor>
    {
        Task<IEnumerable<AssignedInstructor>> GetAssignedInstructorsByNodeID(Guid NodeID);
    }
}
