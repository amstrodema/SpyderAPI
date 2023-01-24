using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Spyder;
using MainAPI.Models.ViewModel.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder.Feature
{
    public class FeatureBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public FeatureBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Models.Spyder.Feature.Feature>> GetFeatures() =>
         await _unitOfWork.Features.GetAll();

        public async Task<IEnumerable<FeatureVM>> GetFeaturesByItemID(Guid itemID)
        {
            return from feature in await _unitOfWork.Features.GetAll()
                   where feature.ItemID == itemID
                   join featureGroup in await _unitOfWork.FeatureGroups.GetAll()
                   on feature.FeatureGroupID
                   equals featureGroup.ID
                   join featureType in await _unitOfWork.FeatureTypes.GetAll()
                   on feature.FeatureTypeID equals featureType.ID
                   select new FeatureVM
                   {
                       FeatureType = featureType.Name,
                       Value = feature.Value,
                       FeatureGroup = featureGroup.Code
                   };
        }

        public async Task<Models.Spyder.Feature.Feature> GetFeatureByID(Guid id) =>
                  await _unitOfWork.Features.Find(id);
        public async Task<ResponseMessage<Models.Spyder.Feature.Feature>> Create(Models.Spyder.Feature.Feature feature)
        {
            ResponseMessage<Models.Spyder.Feature.Feature> responseMessage = new ResponseMessage<Models.Spyder.Feature.Feature>();
            try
            {
                feature.ID = Guid.NewGuid();
                feature.DateCreated = DateTime.Now;
                feature.IsActive = true;
                await _unitOfWork.Features.Create(feature);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = feature;
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

        public async Task<int> Update(Models.Spyder.Feature.Feature feature)
        {
            feature.DateModified = DateTime.Now;
            _unitOfWork.Features.Update(feature);
            return await _unitOfWork.Commit();
        }

    }
}
