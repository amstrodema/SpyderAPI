using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Comment.Spyder;
using MainAPI.Models.Spyder;
using MainAPI.Models.ViewModel.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder
{
    public class CommentBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly WalletBusiness walletBusiness;

        public CommentBusiness(IUnitOfWork unitOfWork, WalletBusiness walletBusiness)
        {
            _unitOfWork = unitOfWork;
            this.walletBusiness = walletBusiness;
        }

        public async Task<List<Comment>> GetComments() =>
         await _unitOfWork.Comments.GetAll();

        public async Task<Comment> GetCommentByID(Guid id) =>
                  await _unitOfWork.Comments.Find(id);
        public async Task<IEnumerable<Comment>> GetCommentsByItemID(Guid itemID) =>
                  await _unitOfWork.Comments.GetCommentsByItemID(itemID);
        public async Task<IEnumerable<CommentVM>> GetCommentVMsByItemID(Guid itemID)
        {
            return from comment in await _unitOfWork.Comments.GetCommentsByItemID(itemID)
                   where comment.IsActive
                   join user in await _unitOfWork.Users.GetAll() on comment.UserID equals user.ID
                   select new CommentVM()
                   {
                       ID = comment.ID,
                       CommenterName = user.Username,
                       DateCreated = comment.DateCreated.ToString("f"),
                       Details = comment.Details,
                       UserID = user.ID
                   };
        }

        public async Task<ResponseMessage<IEnumerable<CommentVM>>> Comment(RequestObject<Comment> requestObject) 
        {
            Comment comment = requestObject.Data;
            Guid authorID = requestObject.AuthorID;

            ResponseMessage<IEnumerable<CommentVM>> responseMessage = new ResponseMessage<IEnumerable<CommentVM>>();
            try
            {
                Params param = await _unitOfWork.Params.GetParamByCode("comment_cost");
                decimal comment_cost = 0;

                try
                {
                    comment_cost = decimal.Parse(param.Value);
                }
                catch (Exception)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Try again...";
                    return responseMessage;
                }

                var res = await walletBusiness.Payment(comment.UserID, comment_cost, comment.UserCountryID, "Comment", comment.ItemID.ToString());

                if (res.StatusCode != 200)
                {
                    responseMessage.StatusCode = res.StatusCode;
                    responseMessage.Message = res.Message;
                    return responseMessage;
                }

                comment.ID = Guid.NewGuid();
                comment.DateCreated = DateTime.Now;
                comment.IsActive = true;
                await _unitOfWork.Comments.Create(comment);


                try
                {
                    Wallet authorWallet = await _unitOfWork.Wallets.GetWalletByUserID(requestObject.AuthorID);
                    Params gem_gain_percentage_param = await _unitOfWork.Params.GetParamByCode("gem_gain_percentage");
                    decimal gem_gain_percentage = decimal.Parse(gem_gain_percentage_param.Value);

                    authorWallet.Gem += comment_cost * 1.0M * gem_gain_percentage / 100;

                    _unitOfWork.Wallets.Update(authorWallet);
                }
                catch (Exception)
                {
                }

                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = await GetCommentVMsByItemID(comment.ItemID);
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
