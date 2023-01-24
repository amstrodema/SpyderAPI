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
    public class ParamsBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public ParamsBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Params>> GetParams() =>
         await _unitOfWork.Params.GetAll();

        public async Task<int> Update(Params param)
        {
            param.DateModified = DateTime.Now;
            _unitOfWork.Params.Update(param);
            return await _unitOfWork.Commit();
        }

        public async Task<Params> GetParamsByID(Guid id) =>
                  await _unitOfWork.Params.Find(id);
        public async Task<Params> GetParamsByCode(string code) =>
                  await _unitOfWork.Params.GetParamByCode(code);

        public async Task<ResponseMessage<Params>> Create(Params param)
        {
            ResponseMessage<Params> responseMessage = new ResponseMessage<Params>();
            try
            {
                param.ID = Guid.NewGuid();
                param.DateCreated = DateTime.Now;
                param.IsActive = true;
                await _unitOfWork.Params.Create(param);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = param;
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
