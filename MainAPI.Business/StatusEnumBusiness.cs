using MainAPI.Data.Interface;
using MainAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business
{
   public class StatusEnumBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatusEnumBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<StatusEnum>> GetStatusEnums() =>
          await _unitOfWork.StatusEnums.GetAll();

        public async Task<StatusEnum> GetStatusEnumByID(Guid id) =>
                  await _unitOfWork.StatusEnums.Find(id);

        public async Task Create(StatusEnum StatusEnum)
        {
            await _unitOfWork.StatusEnums.Create(StatusEnum);
            await _unitOfWork.Commit();
        }

        public async Task Update(StatusEnum StatusEnum)
        {
            _unitOfWork.StatusEnums.Update(StatusEnum);
            await _unitOfWork.Commit();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetStatusEnumByID(id);
            _unitOfWork.StatusEnums.Delete(entity);
            await _unitOfWork.Commit();
        }

        public async Task<StatusEnum> GetStatusEnumByKey(int code) =>
                  await _unitOfWork.StatusEnums.GetStatusByCode(code);
    }
}
