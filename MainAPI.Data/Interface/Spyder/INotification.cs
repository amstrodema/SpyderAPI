using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Spyder
{
    public interface INotification: IGeneric<Notification>
    {
        Task<IEnumerable<Notification>> GetAllNotificationBySenderID(Guid SenderID);
        Task<IEnumerable<Notification>> GetAllNotificationByRecieverID(Guid RecieverID);
        Task<IEnumerable<Notification>> GetUnReadNotificationByRecieverID(Guid RecieverID);
        Task<int> UnreadNotification(Guid userID);
    }
}
