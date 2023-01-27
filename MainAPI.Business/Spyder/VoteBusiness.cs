using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Petition.Spyder;
using MainAPI.Models.Spyder;
using MainAPI.Models.ViewModel.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder
{
    public class VoteBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly WalletBusiness walletBusiness;
        private readonly PetitionBusiness petitionBusiness;

        public VoteBusiness(IUnitOfWork unitOfWork, WalletBusiness walletBusiness, PetitionBusiness petitionBusiness)
        {
            _unitOfWork = unitOfWork;
            this.walletBusiness = walletBusiness;
            this.petitionBusiness = petitionBusiness;
        }

        public async Task<List<Vote>> GetVotes() =>
         await _unitOfWork.Votes.GetAll();

        public async Task<Vote> GetVoteByID(Guid id) =>
                  await _unitOfWork.Votes.Find(id);
        public async Task<Vote> GetVoteByUserID_ItemID(Guid userID, Guid itemID) =>
                  await _unitOfWork.Votes.GetVoteByUserID_ItemID(userID, itemID);
        public async Task<IEnumerable<Vote>> GetVotesByItemID(Guid itemID) =>
                  await _unitOfWork.Votes.GetVotesByItemID(itemID);
        public async Task<ResponseMessage<VoteVM>> Vote(RequestObject<Vote> requestObject)
        {
            Vote vote = requestObject.Data;

            ResponseMessage<VoteVM> responseMessage = new ResponseMessage<VoteVM>();
            try
            {
                Vote getVote = await GetVoteByUserID_ItemID(vote.UserID, vote.ItemID);
              
                Params param = await _unitOfWork.Params.GetParamByCode("vote_cost");
                decimal votingCost = 0;

                try
                {
                    votingCost = decimal.Parse(param.Value);
                }
                catch (Exception)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Voting failed!";
                    responseMessage.Data = await CheckVotes(vote.ItemID);
                    return responseMessage;
                }

                var res = await walletBusiness.Payment(vote.UserID, votingCost, vote.UserCountryID, "Vote Action", vote.ItemID.ToString());

                if (res.StatusCode != 200)
                {
                    responseMessage.StatusCode = res.StatusCode;
                    responseMessage.Message = res.Message;
                    responseMessage.Data = await CheckVotes(vote.ItemID);
                    return responseMessage;
                }

                if (getVote == null)
                {
                    vote.ID = Guid.NewGuid();
                    vote.DateCreated = DateTime.Now;
                    await _unitOfWork.Votes.Create(vote);
                }
                else
                {
                    if (!vote.IsReact)
                    {
                        await Delete(getVote.ID);

                        responseMessage.StatusCode = 200;
                        responseMessage.Message = "Operation successful!";
                    responseMessage.Data = await CheckVotes(vote.ItemID);
                        return responseMessage;
                    }
                    else {
                        getVote.IsLike = vote.IsLike;
                        getVote.IsReact = vote.IsReact;
                        getVote.BtnBgTypeDisLike = vote.BtnBgTypeDisLike;
                        getVote.BtnBgTypeLike = vote.BtnBgTypeLike;
                        _unitOfWork.Votes.Update(getVote);
                    }                   
                }

                try
                {
                    Wallet authorWallet = await _unitOfWork.Wallets.GetWalletByUserID(requestObject.AuthorID);
                    Params gem_gain_percentage_param = await _unitOfWork.Params.GetParamByCode("gem_gain_percentage");
                    decimal gem_gain_percentage = decimal.Parse(gem_gain_percentage_param.Value);

                    authorWallet.Gem += votingCost * 1.0M * gem_gain_percentage / 100;

                    _unitOfWork.Wallets.Update(authorWallet);
                }
                catch (Exception)
                {
                }

                if (await _unitOfWork.Commit() >= 1)
                {

                    await petitionBusiness.Approve(vote.ItemID);
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Operation successful!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Operation not successful!";
                }
                responseMessage.Data = await CheckVotes(vote.ItemID);

            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Something went wrong. Try Again!";
                responseMessage.Data = await CheckVotes(vote.ItemID);
            }

            return responseMessage;
        }

        public async Task<VoteVM> CheckVotes(Guid petitionID)
        {
            var votes = await GetVotesByItemID(petitionID);
            Petition petition = await _unitOfWork.Petitions.Find(petitionID);
            if (votes == null)
            {
                return new VoteVM()
                {
                    TotalDownVote = 0,
                    TotalUpVote = 0,
                    VotePercentage =0
                };
            }
            return new VoteVM()
            {
                TotalDownVote = votes.Where(x => x.IsReact && !x.IsLike).Count(),
                TotalUpVote = votes.Where(x => x.IsReact && x.IsLike).Count(),
                VotePercentage = votes.Where(x => x.IsReact && x.IsLike).Count() * 1.0f / petition.RequiredVoters * 100
            };

        }
     
        public async Task<int> Delete(Guid id)
        {
            var entity = await GetVoteByID(id);
            _unitOfWork.Votes.Delete(entity);
            return await _unitOfWork.Commit();
        }
    }
}
