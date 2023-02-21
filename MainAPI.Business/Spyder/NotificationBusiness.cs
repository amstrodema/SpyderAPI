using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Spyder;
using MainAPI.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder
{
    public class NotificationBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Notification>> GetNotifications() =>
         await _unitOfWork.Notifications.GetAll();

        public async Task<Notification> GetNotificationByID(Guid id) =>
                  await _unitOfWork.Notifications.Find(id);
        public async Task<ResponseMessage<IEnumerable<NotificationVM>>> GetAllNotificationByRecieverID(Guid receiverID)
        {
            ResponseMessage<IEnumerable<NotificationVM>> responseMessage = new ResponseMessage<IEnumerable<NotificationVM>>();
            try
            {
                var users = await _unitOfWork.Users.GetAll();

                if (await _unitOfWork.LogInMonitors.GetLogInMonitorByUserID(receiverID) != default)
                {
                    var notif = await _unitOfWork.Notifications.GetAllNotificationByRecieverID(receiverID);

                    notif = notif.OrderByDescending(k => k.DateCreated);
                    responseMessage.Data = (from notification in await _unitOfWork.Notifications.GetAllNotificationByRecieverID(receiverID)
                                           select new NotificationVM()
                                           {
                                               DateCreated = notification.DateCreated.ToString("f"),
                                               ID = notification.ID,
                                               IsRead = notification.IsRead,
                                               Message = notification.Message,
                                               Sender = notification.IsSpyder ? "Spyder" : ResolveUser(users, notification.SenderID),
                                               Date = notification.DateCreated
                                           }).ToList();

                    notif = notif.Where(k => !k.IsRead);
                    List<Notification> IsRead = new List<Notification>();
                    foreach (var item in notif)
                    {
                        Notification noti = item;
                        noti.IsRead = true;
                        IsRead.Add(noti);
                    }

                    _unitOfWork.Notifications.UpdateMultiple(IsRead.ToArray());
                    await _unitOfWork.Commit();

                    responseMessage.Data = responseMessage.Data.OrderByDescending(a => a.Date);

                    responseMessage.StatusCode = 200;
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Access denied! Log in and try again.";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Error occured!";
            }

            return responseMessage;
        }
        public async Task<NotificationHybrid> GetAllNotificationAlert(Guid receiverID)
        {
            //var notifications = (from notification in await _unitOfWork.Notifications.GetUnReadNotificationByRecieverID(receiverID)
            //                    select new NotificationVM()
            //                    {
            //                        Message = notification.Message,
            //                        DateCreated = ResolveTime((DateTime.Now - notification.DateCreated).Seconds)
            //                    }).ToList();
            //notifications = (notifications.OrderBy(p => p.DateCreated)).Take(3).ToList();

            NotificationHybrid notificationHybrid = new NotificationHybrid();
            //notificationHybrid.NotificationVMs = notifications;
            notificationHybrid.UnReadInbox = await _unitOfWork.Inboxes.UnreadMessages(receiverID);
            notificationHybrid.UnReadNotification = await _unitOfWork.Notifications.UnreadNotification(receiverID);

            return notificationHybrid;
        }

        private string ResolveTime(int sec)
        {
            int hrs = sec / 3600;
            int mins = sec - (hrs * 3600);

            if (hrs > 0)
            {
                if (hrs == 1)
                {
                    return $"{hrs} hr " + CheckMins(mins);
                }
                return $"{hrs} hrs " + CheckMins(mins);
            }
            return CheckMins(mins);
        }
        private string CheckMins(int min)
        {
            if (min > 0)
            {
                if (min == 1)
                {
                    return $"{min} min ago";
                }
                return $"{min} mins ago";
            }
            return "";
        }
        private string ResolveUser(IEnumerable<User> users, Guid userID)
        {
            try
            {
                return users.FirstOrDefault(p=> p.ID == userID).Username;
            }
            catch (Exception)
            {
                return ("Anonymous");
            }
        }
        public async Task<IEnumerable<Notification>> GetAllNotificationBySenderID(Guid senderID) =>
                  await _unitOfWork.Notifications.GetAllNotificationBySenderID(senderID);

        public async Task<ResponseMessage<Notification>> Create(Notification notification)
        {
            ResponseMessage<Notification> responseMessage = new ResponseMessage<Notification>();
            try
            {
                if(notification.RecieverID == default || notification.SenderID == default)
                {
                    responseMessage.StatusCode = (int) HttpStatusCode.BadRequest;
                    responseMessage.Message = "Bad Request";
                }

                notification.ID = Guid.NewGuid();
                notification.DateCreated = DateTime.Now;
                notification.IsActive = true;
                await _unitOfWork.Notifications.Create(notification);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = notification;
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Alert sent!";
                }
                else
                {
                    responseMessage.StatusCode = (int)HttpStatusCode.BadGateway;
                    responseMessage.Message = "Not successful!";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Failed. Try Again!";
            }

            return responseMessage;
        }

    }
}
