using MainAPI.Models.Comment.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Spyder
{
    public interface IComment: IGeneric<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsByItemID(Guid itemID);
    }
}
