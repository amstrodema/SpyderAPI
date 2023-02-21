using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Spyder.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder.Feature
{
    public class FeatureTypeBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public FeatureTypeBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FeatureType>> GetFeatureTypes() =>
         await _unitOfWork.FeatureTypes.GetAll();

        public async Task<FeatureType> GetFeatureTypeByID(Guid id) =>
                  await _unitOfWork.FeatureTypes.Find(id);
        private async Task<IEnumerable<FeatureType>> GetFeatureTypesByGroup(Guid id) =>
                  await _unitOfWork.FeatureTypes.GetFeatureTypesByGroup(id);
        public async Task<IEnumerable<FeatureType>> GetFeatureTypesByGroupCode(string groupCode)
        {
            var grp = await _unitOfWork.FeatureGroups.GetFeatureGroupByCode(groupCode);
            return (await GetFeatureTypesByGroup(grp.ID)).OrderBy(p => p.Name);
        }
        public async Task<ResponseMessage<FeatureType>> Create(FeatureType FeatureType)
        {
            ResponseMessage<FeatureType> responseMessage = new ResponseMessage<FeatureType>();
            try
            {
                FeatureType.ID = Guid.NewGuid();
                FeatureType.DateCreated = DateTime.Now;
                FeatureType.IsActive = true;
                await _unitOfWork.FeatureTypes.Create(FeatureType);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = FeatureType;
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
        public async Task<int> Update(FeatureType featureType)
        {
            featureType.DateModified = DateTime.Now;
            _unitOfWork.FeatureTypes.Update(featureType);
            return await _unitOfWork.Commit();
        }
    }
}
