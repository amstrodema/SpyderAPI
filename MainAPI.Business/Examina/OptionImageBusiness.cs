using MainAPI.Data.Interface;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Examina
{
   public class OptionImageBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public OptionImageBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<OptionImage>> GetOptionImages() =>
          await _unitOfWork.OptionImages.GetAll();

        public async Task<OptionImage> GetOptionImageByID(Guid id) =>
                  await _unitOfWork.OptionImages.Find(id);

        public async Task<IEnumerable<OptionImage>> GetOptionImageObjectsByNodeID(Guid nodeID) =>
                  await _unitOfWork.OptionImages.GetOptionImageObjectsByNodeID(nodeID);

        public async Task<IEnumerable<OptionImage>> GetOptionImageByOptionID(Guid optionID) =>
                  await _unitOfWork.OptionImages.GetOptionImageByOptionID(optionID);

        public async Task Create(OptionImage OptionImage)
        {
            await _unitOfWork.OptionImages.Create(OptionImage);
            await _unitOfWork.Commit();
        }

        public async Task Update(OptionImage OptionImage)
        {
            _unitOfWork.OptionImages.Update(OptionImage);
            await _unitOfWork.Commit();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetOptionImageByID(id);
            _unitOfWork.OptionImages.Delete(entity);
            await _unitOfWork.Commit();
        }
    }
}
