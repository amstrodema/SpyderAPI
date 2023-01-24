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
    public class LinkBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly WalletBusiness walletBusiness;

        public LinkBusiness(IUnitOfWork unitOfWork, WalletBusiness walletBusiness)
        {
            _unitOfWork = unitOfWork;
            this.walletBusiness = walletBusiness;
        }

        public async Task<List<Link>> GetLinks() =>
         await _unitOfWork.Links.GetAll();
        public async Task<IEnumerable<Link>> GetLinksByItemID(Guid itemID) =>
         await _unitOfWork.Links.GetItemLinks(itemID);

        public async Task<Link> GetLinkByID(Guid id) =>
                  await _unitOfWork.Links.Find(id);
        public async Task<ResponseMessage<Link>> Create(Link Link)
        {
            ResponseMessage<Link> responseMessage = new ResponseMessage<Link>();
            try
            {
                Link.ID = Guid.NewGuid();
                Link.DateCreated = DateTime.Now;
                Link.IsActive = true;

                Params param = await _unitOfWork.Params.GetParamByCode("gallery_link_cost");
                decimal gallery_link_cost = 0;

                try
                {
                    gallery_link_cost = decimal.Parse(param.Value);
                }
                catch (Exception)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Try again...";
                    return responseMessage;
                }

                var res = await walletBusiness.Payment(Link.CreatedBy, gallery_link_cost, await _unitOfWork.Users.GetUserCountryByUserID(Link.CreatedBy), "Gallery Link", Link.ID.ToString());

                if (res.StatusCode != 200)
                {
                    responseMessage.StatusCode = res.StatusCode;
                    responseMessage.Message = res.Message;
                    return responseMessage;
                }

                await _unitOfWork.Links.Create(Link);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = Link;
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
