using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Spyder
{
    public interface IVote: IGeneric<Vote>
    {
        Task<Vote> GetVoteByUserID_ItemID(Guid userID, Guid itemID);
        Task<IEnumerable<Vote>> GetVotesByItemID(Guid itemID);
        Task<IEnumerable<Vote>> GetUpVotesByItemID(Guid itemID);
        Task<IEnumerable<Vote>> GetVotesByUserID(Guid userID);
    }
}
