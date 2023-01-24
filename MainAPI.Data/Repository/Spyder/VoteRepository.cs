using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class VoteRepository: GenericRepository<Vote>, IVote
    {
        public VoteRepository(MainAPIContext db): base(db) { }
        public async Task<Vote> GetVoteByUserID_ItemID(Guid userID, Guid itemID)
        {
            return await GetOneBy(x => x.ItemID == itemID && x.UserID == userID);
        }
        public async Task<IEnumerable<Vote>> GetVotesByItemID(Guid itemID)
        {
            return await GetBy(x => x.ItemID == itemID);
        }
        public async Task<IEnumerable<Vote>> GetVotesByUserID(Guid userID)
        {
            return await GetBy(x => x.UserID == userID);
        }
        public async Task<IEnumerable<Vote>> GetUpVotesByItemID(Guid itemID)
        {
            return await GetBy(x => x.ItemID == itemID && x.IsLike && x.IsReact);
        }
    }
}
