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
    public class FeatureGroupBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public FeatureGroupBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FeatureGroup>> GetFeatureGroups() =>
         await _unitOfWork.FeatureGroups.GetAll();

        public async Task<FeatureGroup> GetFeatureGroupByID(Guid id) =>
                  await _unitOfWork.FeatureGroups.Find(id);
        public async Task<FeatureGroup> GetFeatureGroupByCode(string code) =>
                  await _unitOfWork.FeatureGroups.GetFeatureGroupByCode(code);
        public async Task<IEnumerable<FeatureGroup>> GetFeatureGroupByGroupNo(int groupNo) =>
                  await _unitOfWork.FeatureGroups.GetFeatureGroupByGroupNo(groupNo);
        public async Task<ResponseMessage<FeatureGroup>> Create(FeatureGroup FeatureGroup)
        {
            ResponseMessage<FeatureGroup> responseMessage = new ResponseMessage<FeatureGroup>();
            try
            {
                FeatureGroup.ID = Guid.NewGuid();
                FeatureGroup.DateCreated = DateTime.Now;
                FeatureGroup.IsActive = true;
                await _unitOfWork.FeatureGroups.Create(FeatureGroup);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = FeatureGroup;
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
        public async Task<int> Update(FeatureGroup featureGroup)
        {
            featureGroup.DateModified = DateTime.Now;
            _unitOfWork.FeatureGroups.Update(featureGroup);
            return await _unitOfWork.Commit();
        }
    }
}
