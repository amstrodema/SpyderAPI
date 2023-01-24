using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Comment.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class CommentRepository: GenericRepository<Comment>, IComment
    {
        public CommentRepository(MainAPIContext db) : base(db) { }
         
        public async Task<IEnumerable<Comment>> GetCommentsByItemID(Guid itemID)
        {
            return await GetBy(p => p.ItemID == itemID);
        }
    }
}
