using MainAPI.Data.Interface.Examina;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Examina
{
    public class UserRepository: GenericRepository<User>, IUser
    {
        public UserRepository(MainAPIContext db) : base(db) { }

        public async Task<User> GetUserByStaffID(string staffID)
        {
            return await GetOneBy(u => u.StaffID == staffID);
        }
        public async Task<IEnumerable<User>> GetUsersByNodeID(Guid NodeID) => await GetBy(u => u.NodeID == NodeID);
        
    }
}
