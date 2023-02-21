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
         (await _unitOfWork.Params.GetAll()).OrderBy(o => o.Name).ToList();

        public async Task<int> Update(Params param)
        {
            var parm = await _unitOfWork.Params.Find(param.ID);
            parm.Name = param.Name;
            parm.Code = param.Code;
            parm.CreatedBy = param.CreatedBy;
            parm.DateCreated = param.DateCreated;
            parm.ModifiedBy = param.ModifiedBy;
            parm.Value = param.Value;

            parm.DateModified = DateTime.Now;

            _unitOfWork.Params.Update(parm);
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
                    responseMessage.Message = "Successful!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
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
