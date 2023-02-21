using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Spyder;
using MainAPI.Models.Spyder.Feature;
using MainAPI.Models.ViewModel.Spyder;
using MainAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder
{
    public class MissingBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly WalletBusiness walletBusiness;

        public MissingBusiness(IUnitOfWork unitOfWork, WalletBusiness walletBusiness)
        {
            _unitOfWork = unitOfWork;
            this.walletBusiness = walletBusiness;
        }

        public async Task<List<Missing>> GetMissings() =>
         await _unitOfWork.Missings.GetAll();

        public async Task<IEnumerable<Missing>> GetMissingByItemTypeID(RequestObject<int> requestObject)
        {
            if (requestObject.Data == 0)
            {
                return default;
            }
            if (requestObject.ItemID == "All")
            {
                var missingData = await _unitOfWork.Missings.GetAll();
                try
                {
                    var countryID = Guid.Parse(requestObject.CountryID);
                    missingData = missingData.Where(o => o.CountryID == countryID).ToList();
                }
                catch (Exception)
                {
                }
                var missings = from missing in missingData
                               where missing.AwarenessTypeNo == requestObject.Data
                       select new Missing()
                       {
                           ID = missing.ID,
                           Image = ImageService.GetImageFromFolder(missing.Image, "Missing"),
                           Title = missing.Title,
                           Desc = missing.Desc == null || missing.Desc.Length < 76? missing.Desc : missing.Desc.Substring(0,74),
                           FullInfo = missing.FullInfo == null || missing.FullInfo.Length < 145? missing.FullInfo : missing.FullInfo.Substring(0,143),
                           ItemTypeID = missing.ItemTypeID,
                           CreatedBy = missing.CreatedBy,
                           DateCreated = missing.DateCreated
                       };
                return missings.OrderByDescending(p => p.DateCreated);
            }
            else
            {
                try
                {
                    Guid id = Guid.Parse(requestObject.ItemID);
                    var missingData = await _unitOfWork.Missings.GetMissingByItemTypeID(id);

                    try
                    {
                    var countryID = Guid.Parse(requestObject.CountryID);
                        missingData = missingData.Where(o => o.CountryID == countryID).ToList();
                    }
                    catch (Exception)
                    {
                    }

                    var missings = from missing in missingData
                                   where missing.AwarenessTypeNo == requestObject.Data
                       select missing;
                return missings.OrderByDescending(p => p.DateCreated);
                }
                catch (Exception)
                {
                    return default;
                }
            }


        }
        public async Task<ResponseMessage<MissingVM>> GetMissingDetails(RequestObject<int> requestObject)
        {
            ResponseMessage<MissingVM> responseMessage = new ResponseMessage<MissingVM>();
            Request<int> request = new Request<int>();

            try
            {
                request.ItemID = Guid.Parse(requestObject.ItemID);
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Not successful!";
            }

            Missing missing = await _unitOfWork.Missings.Find(request.ItemID);
            User author = await _unitOfWork.Users.Find(missing.CreatedBy);
            FeatureGroup featureGroup = await _unitOfWork.FeatureGroups.Find(missing.ItemTypeID);
            Country country = await _unitOfWork.Countries.Find(missing.CountryID);
            MissingDetails missingDetails = new MissingDetails()
            {
                ID = missing.ID,
                AwarenessTypeNo = missing.AwarenessTypeNo,
                Country = country == null ? "" : country.Name,
                CreatedBy = $"{author.Username}",
                Day = missing.DateCreated.ToString("d"),
                Desc = missing.Desc,
                FullInfo = missing.FullInfo,
                Image = ImageService.GetImageFromFolder(missing.Image,"Missing"),
            IsActive = missing.IsActive,
                LastSeen = missing.LastSeen,
                Time = missing.DateCreated.ToString("t"),
                Title = missing.Title,
                ItemTypeName = featureGroup == null ? "" : featureGroup.Name,
                CreatorID = author.ID,
                Update = missing.Update,
                UpdateDay = missing.UpdateTime.ToString("d"),
                UpdateTime = missing.UpdateTime.ToString("t")
            };


            var featureVMs = (from feature in await _unitOfWork.Features.GetAll()
                                               where feature.ItemID == missing.ID
                                               join featureType in await _unitOfWork.FeatureTypes.GetAll() on feature.FeatureTypeID equals featureType.ID
                                               select new FeatureVM()
                                               {
                                                   FeatureType = featureType.Name,
                                                   Value = feature.Value
                                               }).ToList();

            MissingVM missingVM = new MissingVM() {
                featureVMs = featureVMs,
                missingDetails = missingDetails
            };

            responseMessage.Data = missingVM;
            responseMessage.StatusCode = 200;

            return responseMessage;
        }

        public async Task<Missing> GetMissingByID(Guid id) =>
                  await _unitOfWork.Missings.Find(id);
        
        public async Task<ResponseMessage<MissingDetails>> UpdateRecord(RequestObject<string> requestObject)
        {
            ResponseMessage<MissingDetails> responseMessage = new ResponseMessage<MissingDetails>();
            try
            {
                Guid userID = Guid.Parse(requestObject.UserID);
                Guid itemID = Guid.Parse(requestObject.ItemID);

                Missing missing = await GetMissingByID(itemID);
                if (missing.CreatedBy != userID || missing.Update != default)
                {
                    responseMessage.Message = "Update failed";
                    responseMessage.StatusCode = 201;
                }

                missing.Update = requestObject.Data;
                missing.UpdateTime = DateTime.Now;

                _unitOfWork.Missings.Update(missing);
                if (await _unitOfWork.Commit() >= 1)
                {

                    MissingDetails missingDetails = new MissingDetails()
                    {
                        UpdateDay = missing.UpdateTime.ToString("d"),
                        UpdateTime = missing.UpdateTime.ToString("t"),
                        Update = missing.Update
                    };

                    responseMessage.Data = missingDetails;
                    responseMessage.Message = "Update successful";
                    responseMessage.StatusCode = 200;
                }
                
            }
            catch (Exception)
            {
                responseMessage.Message = "Update failed";
                responseMessage.StatusCode = 1101;
            }

            return responseMessage;
        }
        public async Task<ResponseMessage<Guid>> Create(MissingVM missingVM)
        {
            ResponseMessage<Guid> responseMessage = new ResponseMessage<Guid>();
            try
            {
                Missing Missing = missingVM.missing;
                var features = missingVM.features;

                Missing.ID = Guid.NewGuid();
                Missing.DateCreated = DateTime.Now;
                Missing.IsActive = true;


                Params param = await _unitOfWork.Params.GetParamByCode("missing_cost");
                decimal missing_cost = 0;

                try
                {
                    missing_cost = decimal.Parse(param.Value);
                }
                catch (Exception)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Try again...";
                    return responseMessage;
                }

                var res = await walletBusiness.Payment(Missing.CreatedBy, missing_cost, Missing.CountryID, "Missing Type", Missing.ID.ToString());

                if (res.StatusCode != 200)
                {
                    responseMessage.StatusCode = res.StatusCode;
                    responseMessage.Message = res.Message;
                    return responseMessage;
                }

                Missing.Image = ImageService.SaveImageInFolder(Missing.Image, Guid.NewGuid().ToString(), "Missing");
                await _unitOfWork.Missings.Create(Missing);


                for (int i = 0; i < features.Count(); i++)
                {
                    features[i].ID = Guid.NewGuid();
                    features[i].ItemID = Missing.ID;
                    features[i].DateCreated = Missing.DateCreated;
                }

                await _unitOfWork.Features.CreateMultiple(features.ToArray());
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = Missing.ID;
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
