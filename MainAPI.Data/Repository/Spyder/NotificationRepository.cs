using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class NotificationRepository: GenericRepository<Notification>, INotification
    {
        public NotificationRepository(MainAPIContext db): base(db) { }
        public async Task<IEnumerable<Notification>> GetAllNotificationByRecieverID(Guid RecieverID)
        {
            return await GetBy(p => p.RecieverID == RecieverID);
        }
        public async Task<IEnumerable<Notification>> GetUnReadNotificationByRecieverID(Guid RecieverID)
        {
            return await GetBy(p => p.RecieverID == RecieverID && !p.IsRead);
        }
        public async Task<IEnumerable<Notification>> GetAllNotificationBySenderID(Guid SenderID)
        {
            return await GetBy(p => p.SenderID == SenderID);
        }
        public async Task<int> UnreadNotification(Guid userID)
        {
            return (await GetBy(o => o.RecieverID == userID && !o.IsRead)).Count();
        }
    }
}
