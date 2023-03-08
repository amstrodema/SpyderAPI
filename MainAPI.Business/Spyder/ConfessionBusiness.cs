using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Spyder;
using MainAPI.Models.ViewModel;
using MainAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder
{
    public class ConfessionBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly WalletBusiness walletBusiness;

        public ConfessionBusiness(IUnitOfWork unitOfWork, WalletBusiness walletBusiness)
        {
            _unitOfWork = unitOfWork;
            this.walletBusiness = walletBusiness;
        }

        public async Task<List<Confession>> GetConfessions() =>
         await _unitOfWork.Confessions.GetAll();
        public async Task<IEnumerable<Confession>> GetConfessionsByDialogueTypeNo(int dialogueTypeNo) =>
         await _unitOfWork.Confessions.GetConfessionsByDialogueTypeNo(dialogueTypeNo);

        public async Task<IEnumerable<ConfessionVM>> GetConfessionHeaders(RequestObject<int> requestObject)
        {
            Request<int> request = new Request<int>();
            if (requestObject.Data == 0)
            {
                return default;
            }

            var confessionData = await GetConfessionsByDialogueTypeNo(requestObject.Data);

            try
            {
                Guid countryID = Guid.Parse(requestObject.CountryID);
                confessionData = confessionData.Where(o => o.CountryID == countryID);
            }
            catch (Exception)
            {
            }

            var confessions = from confession in confessionData
                   join user in await _unitOfWork.Users.GetAll() on confession.CreatedBy equals user.ID
                   select new ConfessionVM()
                   {
                       CreatedBy = confession.IsAnonymous? "Anonymous" : user.Username,
                       ID = confession.ID,
                       Date = confession.DateCreated.ToString("MMM")+" "+ confession.DateCreated.ToString("dd")+", "+ confession.DateCreated.ToString("yyyy"),
                       Time = confession.DateCreated.ToString("t"),
                       Details = confession.Details.Length <=306? confession.Details: confession.Details.Substring(0,306),
                       Image = ImageService.GetSmallImageFromFolder(confession.Image, "Confession"),
                       IsAnonymous = confession.IsAnonymous,
                       Title = confession.Title,
                       DateFilter = confession.DateCreated
                   };

            return confessions.OrderByDescending(o => o.DateFilter);
        }

        public async Task<ResponseMessage<ConfessionVM>> GetConfessionDetails(RequestObject<string> requestObject)
        {
            ResponseMessage<ConfessionVM> res = new ResponseMessage<ConfessionVM>();


            try
            {
                Request<string> request = new Request<string>();
                try
                {
                    request.ItemID = Guid.Parse(requestObject.ItemID);
                }
                catch (Exception)
                {
                    res.StatusCode = 1018;
                    res.Message = "Failed. Try Again!";
                    return res;
                }
                try
                {
                    request.UserID = Guid.Parse(requestObject.UserID);
                    try
                    {
                        request.CountryID = Guid.Parse(requestObject.CountryID);
                    }
                    catch (Exception)
                    {
                        request.IsCountry = false;
                    }

                }
                catch (Exception)
                {
                    request.UserID = default;
                }
                List<Confession> confessions = new List<Confession>();
                confessions.Add(await _unitOfWork.Confessions.Find(request.ItemID));

                var x = (from confession in confessions
                         join user in await _unitOfWork.Users.GetAll() on confession.CreatedBy equals user.ID
                         join personalVote in await _unitOfWork.Likes.GetLikesByUserID(request.UserID) on confession.ID equals personalVote.ItemID
                         into allPersonalVotes
                         from personalVote in allPersonalVotes.DefaultIfEmpty()
                         join vote in await _unitOfWork.Likes.GetAll() on confession.ID equals vote.ItemID into allVotes
                         select new ConfessionVM()
                         {
                             CreatedBy = confession.IsAnonymous ? "Anonymous" : user.Username,
                             ID = confession.ID,
                             Date = confession.DateCreated.ToString("MMM") + " " + confession.DateCreated.ToString("dd") + ", " + confession.DateCreated.ToString("yyyy"),
                             Time = confession.DateCreated.ToString("t"),
                             Details = confession.Details,
                             Image = ImageService.GetImageFromFolder(confession.Image, "Confession"),
                             IsAnonymous = confession.IsAnonymous,
                             Title = confession.Title,
                             IsLike = personalVote == default ? false : personalVote.IsLike,
                             IsReact = personalVote == default ? false : personalVote.IsReact,
                             TotalDisLikes = allVotes.Where(p => p.IsReact && !p.IsLike).Count(),
                             TotalLikes = allVotes.Where(p => p.IsReact && p.IsLike).Count(),
                             CreatedByID =  confession.IsAnonymous ? default : user.ID
                         }).ToArray();

                res.StatusCode = 200;
                res.Message = "Success!";
                res.Data = x[0];
            }
            catch (Exception)
            {
                res.StatusCode = 1018;
                res.Message = "Failed. Try Again!";
            }
            return res;

        }

        public async Task<Confession> GetConfessionByID(Guid id) =>
                  await _unitOfWork.Confessions.Find(id);
        public async Task<ResponseMessage<Guid>> Create(Confession Confession)
        {
            ResponseMessage<Guid> responseMessage = new ResponseMessage<Guid>();
            try
            {
                Confession.ID = Guid.NewGuid();
                Confession.DateCreated = DateTime.Now;
                Confession.IsActive = true;

                Params param = await _unitOfWork.Params.GetParamByCode("confession_cost");
                decimal confession_cost = 0;

                try
                {
                    confession_cost = decimal.Parse(param.Value);
                }
                catch (Exception)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Try again...";
                    return responseMessage;
                }

                var res = await walletBusiness.Payment(Confession.CreatedBy, confession_cost, Confession.CountryID, "Confession Type", Confession.ID.ToString());

                if (res.StatusCode != 200)
                {
                    responseMessage.StatusCode = res.StatusCode;
                    responseMessage.Message = res.Message;
                    return responseMessage;
                }

                Confession.Image = ImageService.SaveImageInFolder(Confession.Image, Guid.NewGuid().ToString(), "Confession");
                await _unitOfWork.Confessions.Create(Confession);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = Confession.ID;
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
