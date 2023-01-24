using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class LikeRepository: GenericRepository<Like>, ILike
    {
        public LikeRepository(MainAPIContext db): base(db) { }
        public async Task<Like> GetLikeByUserID_ItemID(Guid userID, Guid itemID)
        {
            return await GetOneBy(x => x.ItemID == itemID && x.UserID == userID);
        }
        public async Task<IEnumerable<Like>> GetLikesByUserID(Guid userID)
        {
            return await GetBy(x => x.UserID == userID);
        }
        public async Task<IEnumerable<Like>> GetLikesByItemID(Guid itemID)
        {
            return await GetBy(x => x.ItemID == itemID);
        }
    }
}
