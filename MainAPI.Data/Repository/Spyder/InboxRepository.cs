using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class InboxRepository : GenericRepository<Inbox>, IInbox
    {
        public InboxRepository(MainAPIContext db) : base(db) { }

        public async Task<int> UnreadMessages(Guid userID)
        {
            return (await GetBy(o => o.ReceiverID == userID && !o.IsRead)).Count();
        }
    }
}
