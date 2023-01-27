using MainAPI.Business.Spyder.Feature;
using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Spyder;
using MainAPI.Models.ViewModel.Spyder;
using MainAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder
{
    public class MarriageBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FeatureBusiness featureBusiness;
        private readonly WalletBusiness walletBusiness;

        public MarriageBusiness(IUnitOfWork unitOfWork, FeatureBusiness featureBusiness, WalletBusiness walletBusiness)
        {
            _unitOfWork = unitOfWork;
            this.featureBusiness = featureBusiness;
            this.walletBusiness = walletBusiness;
        }

        public async Task<List<Marriage>> GetMarriages() =>
         await _unitOfWork.Marriages.GetAll();

        public async Task<List<Marriage>> GetMarriages(string countryID)
        {
            var marriages = from mar in await _unitOfWork.Marriages.GetAll()
                            select new Marriage()
                            {
                                BrideFName = mar.BrideFName,
                                BrideLName = mar.BrideLName,
                                CertNo = mar.CertNo,
                                City = mar.City,
                                CountryID = mar.CountryID,
                                CreatedBy = mar.CreatedBy,
                                DateCreated = mar.DateCreated,
                                DateModified = mar.DateModified,
                                GroomFName = mar.GroomFName,
                                GroomLName = mar.GroomLName,
                                ID = mar.ID,
                                IsActive = mar.IsActive,
                                WeddingDate = mar.WeddingDate,
                                Toast = mar.Toast,
                                Status = mar.Status,
                                ModifiedBy = mar.ModifiedBy,
                                Type = mar.Type,
                                Image = ImageService.GetImageFromFolder(mar.Image, "Marriage"),
                            };
            try
            {
                Guid id = Guid.Parse(countryID);
                marriages = marriages.Where(c => c.CountryID == id);
            }
            catch (Exception)
            {

            }

            marriages = marriages.OrderByDescending(p => p.DateCreated);

            return marriages.ToList();
        }

        public async Task<Marriage> GetMarriageByID(Guid id)
        {
            var marriage = await _unitOfWork.Marriages.Find(id);
            marriage.Image = ImageService.GetImageFromFolder(marriage.Image, "Marriage");

            return marriage;
        }

        public async Task<MarriageVM> GetMarriageVMByID(Guid id)
        {
            MarriageVM marriageVM = new MarriageVM()
            {
                featureVMs = (await featureBusiness.GetFeaturesByItemID(id)).ToList(),
                marriage = await GetMarriageByID(id),
            };
            return marriageVM;
        }
        public async Task<ResponseMessage<Guid>> Create(MarriageVM marriageVM)
        {
            ResponseMessage<Guid> responseMessage = new ResponseMessage<Guid>();
            try
            {

                Marriage marriage = marriageVM.marriage;
                marriage.ID = Guid.NewGuid();
                marriage.DateCreated = DateTime.Now;
                marriage.IsActive = true;
                marriage.Status = "Married";

                Params param = await _unitOfWork.Params.GetParamByCode("marriage_cost");
                decimal marriage_cost = 0;

                try
                {
                    marriage_cost = decimal.Parse(param.Value);
                }
                catch (Exception)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Try again...";
                    return responseMessage;
                }

                var res = await walletBusiness.Payment(marriage.CreatedBy, marriage_cost, marriage.CountryID, "Marriage Register", marriage.ID.ToString());

                if (res.StatusCode != 200)
                {
                    responseMessage.StatusCode = res.StatusCode;
                    responseMessage.Message = res.Message;
                    return responseMessage;
                }

                marriage.Image = ImageService.SaveImageInFolder(marriage.Image, Guid.NewGuid().ToString(), "Marriage");
                await _unitOfWork.Marriages.Create(marriage);

                var features = marriageVM.features;
                for (int i = 0; i < features.Count(); i++)
                {
                    features[i].ID = Guid.NewGuid();
                    features[i].ItemID = marriage.ID;
                    features[i].DateCreated = marriage.DateCreated;
                }

                await _unitOfWork.Features.CreateMultiple(features.ToArray());

                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = marriage.ID;
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
