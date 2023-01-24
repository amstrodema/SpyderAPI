using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder
{
    public class InboxBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public InboxBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Inbox>> GetInboxes() =>
         await _unitOfWork.Inboxes.GetAll();

        public async Task<Inbox> GetInboxByID(Guid id) =>
                  await _unitOfWork.Inboxes.Find(id);
        public async Task<ResponseMessage<Inbox>> Create(Inbox Inbox)
        {
            ResponseMessage<Inbox> responseMessage = new ResponseMessage<Inbox>();
            try
            {
                Inbox.ID = Guid.NewGuid();
                Inbox.DateCreated = DateTime.Now;
                Inbox.IsActive = true;
                await _unitOfWork.Inboxes.Create(Inbox);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = Inbox;
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Operation successful!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Operation not successful!";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Something went wrong. Try Again!";
            }

            return responseMessage;
        }
    }
}
