using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Spyder
{
    public interface IInbox : IGeneric<Inbox>
    {
        Task<int> UnreadMessages(Guid userID);
    }
}
