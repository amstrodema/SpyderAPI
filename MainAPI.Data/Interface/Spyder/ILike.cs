using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Spyder
{
    public interface ILike: IGeneric<Like>
    {
        Task<Like> GetLikeByUserID_ItemID(Guid userID, Guid itemID);
        Task<IEnumerable<Like>> GetLikesByUserID(Guid userID);
        Task<IEnumerable<Like>> GetLikesByItemID(Guid itemID);
    }
}
