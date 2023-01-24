using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Examina
{
    public interface IUser : IGeneric<User>
    {
        Task<User> GetUserByStaffID(string staffID);
        Task<IEnumerable<User>> GetUsersByNodeID(Guid NodeID);
    }
}
