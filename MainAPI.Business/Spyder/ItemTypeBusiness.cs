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
    public class ItemTypeBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItemTypeBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ItemType>> GetItemTypes() =>
         await _unitOfWork.ItemTypes.GetAll();

        public async Task<ItemType> GetItemTypeByID(Guid id) =>
                  await _unitOfWork.ItemTypes.Find(id);
        public async Task<ResponseMessage<ItemType>> Create(ItemType ItemType)
        {
            ResponseMessage<ItemType> responseMessage = new ResponseMessage<ItemType>();
            try
            {
                ItemType.ID = Guid.NewGuid();
                ItemType.DateCreated = DateTime.Now;
                ItemType.IsActive = true;
                await _unitOfWork.ItemTypes.Create(ItemType);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = ItemType;
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
