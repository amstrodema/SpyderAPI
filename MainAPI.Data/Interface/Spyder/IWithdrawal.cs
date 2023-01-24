using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Spyder
{
    public interface IWithdrawal : IGeneric<Withdrawal>
    {
        Task<IEnumerable<Withdrawal>> GetWithdrawalsByUserID(Guid userID);
    }
}
