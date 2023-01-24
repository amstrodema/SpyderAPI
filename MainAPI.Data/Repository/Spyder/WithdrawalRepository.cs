using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class WithdrawalRepository : GenericRepository<Withdrawal>, IWithdrawal
    {
        public WithdrawalRepository(MainAPIContext db) : base(db) { }
        public async Task<IEnumerable<Withdrawal>> GetWithdrawalsByUserID(Guid userID)
        {
            return await GetBy(u => u.UserID == userID);
        }
    }
}
