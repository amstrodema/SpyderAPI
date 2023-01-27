using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Spyder;
using MainAPI.Models.ViewModel.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder
{
    public class LikeBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly WalletBusiness walletBusiness;

        public LikeBusiness(IUnitOfWork unitOfWork, WalletBusiness walletBusiness)
        {
            _unitOfWork = unitOfWork;
            this.walletBusiness = walletBusiness;
        }

        public async Task<List<Like>> GetLikes() =>
         await _unitOfWork.Likes.GetAll();

        public async Task<Like> GetLikeByID(Guid id) =>
                  await _unitOfWork.Likes.Find(id);
        public async Task<IEnumerable<Like>> GetLikesByItemID(Guid id) =>
                  await _unitOfWork.Likes.GetLikesByItemID(id);
        public async Task<Like> GetLikeByUserID_ItemID(Guid userID, Guid itemID) =>
                  await _unitOfWork.Likes.GetLikeByUserID_ItemID(userID, itemID);
        public async Task<ResponseMessage<VoteVM>> Like(RequestObject<Like> requestObject)
        {
            Like like = requestObject.Data;
            ResponseMessage<VoteVM> responseMessage = new ResponseMessage<VoteVM>();
            try
            {
                Params param = await _unitOfWork.Params.GetParamByCode("like_action_cost");
                decimal like_action_cost = 0;

                try
                {
                    like_action_cost = decimal.Parse(param.Value);
                }
                catch (Exception)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Try again...";
                    return responseMessage;
                }

                var res = await walletBusiness.Payment(like.UserID, like_action_cost, like.UserCountryID, "Like Action", like.ItemID.ToString());

                if (res.StatusCode != 200)
                {
                    responseMessage.StatusCode = res.StatusCode;
                    responseMessage.Message = res.Message;
                    return responseMessage;
                }

                Like getLike = await GetLikeByUserID_ItemID(like.UserID, like.ItemID);

                if (getLike == null)
                {
                    like.ID = Guid.NewGuid();
                    like.DateCreated = DateTime.Now;
                    await _unitOfWork.Likes.Create(like);
                }
                else
                {
                    if (!like.IsReact)
                    {
                        await Delete(getLike.ID);

                        responseMessage.StatusCode = 200;
                        responseMessage.Data = await CheckLikes(like.ItemID);
                        responseMessage.Message = "Operation successful!";
                        return responseMessage;
                    }
                    else
                    {
                        getLike.IsLike = like.IsLike;
                        getLike.IsReact = like.IsReact;
                        getLike.BtnBgTypeDisLike = like.BtnBgTypeDisLike;
                        getLike.BtnBgTypeLike = like.BtnBgTypeLike;
                        _unitOfWork.Likes.Update(getLike);
                    }                    
                }



                try
                {
                    Wallet authorWallet = await _unitOfWork.Wallets.GetWalletByUserID(requestObject.AuthorID);
                    Params gem_gain_percentage_param = await _unitOfWork.Params.GetParamByCode("gem_gain_percentage");
                    decimal gem_gain_percentage = decimal.Parse(gem_gain_percentage_param.Value);

                    authorWallet.Gem += like_action_cost * 1.0M * gem_gain_percentage / 100;

                    _unitOfWork.Wallets.Update(authorWallet);
                }
                catch (Exception)
                {
                }

                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Data = await CheckLikes(like.ItemID);
                    responseMessage.Message = "Operation successful!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Try again later!";
                }

            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Error. Try Again!";
            }

            return responseMessage;
        }

        public async Task<int> Delete(Guid id)
        {
            var entity = await GetLikeByID(id);
            _unitOfWork.Likes.Delete(entity);
            return await _unitOfWork.Commit();
        }

        public async Task<VoteVM> CheckLikes(Guid itemID)
        {
            var likes = await GetLikesByItemID(itemID);
            if (likes == null)
            {
                return new VoteVM()
                {
                    TotalDownVote = 0,
                    TotalUpVote = 0
                };
            }
            return new VoteVM()
            {
                TotalDownVote = likes.Where(x => x.IsReact && !x.IsLike).Count(),
                TotalUpVote = likes.Where(x => x.IsReact && x.IsLike).Count()
            };

        }
    }
}
