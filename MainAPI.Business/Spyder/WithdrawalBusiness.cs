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
    public class WithdrawalBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly WalletBusiness walletBusiness;

        public WithdrawalBusiness(IUnitOfWork unitOfWork, WalletBusiness walletBusiness)
        {
            _unitOfWork = unitOfWork;
            this.walletBusiness = walletBusiness;
        }

        public async Task<List<Withdrawal>> GetWithdrawals() =>
         await _unitOfWork.Withdrawals.GetAll();

        public async Task<Withdrawal> GetWithdrawalByID(Guid id) =>
                  await _unitOfWork.Withdrawals.Find(id);
        public async Task<IEnumerable<Withdrawal>> GetWithdrawalsByUserID(Guid userID) =>
                  await _unitOfWork.Withdrawals.GetWithdrawalsByUserID(userID);
        public async Task<ResponseMessage<IEnumerable<Withdrawal>>> Create(Guid userID)
        {
            ResponseMessage<IEnumerable<Withdrawal>> responseMessage = new ResponseMessage<IEnumerable<Withdrawal>>();
            try
            {
                decimal amount = await walletBusiness.ProcessRefPayment(userID);

                Withdrawal withdrawal = new Withdrawal()
                {
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    Amount = amount,
                    Status = "Pending",
                    StatusCode = 0,
                    UserID = userID
                };

                Notification notification = new Notification()
                {
                    RecieverID = userID,
                    CreatedBy = default,
                    DateCreated = withdrawal.DateCreated,
                    ID = Guid.NewGuid(),
                    IsActive = true,
                    IsRead = false,
                    IsSpyder = true,
                    Message = $"Debit: Withdrawal Amt: ₦{withdrawal.Amount} Date: {withdrawal.DateCreated.ToString("F")} Desc: WIT/SPYDER TrxID: {withdrawal.ID}. Bal:{0}"

                };

                await _unitOfWork.Notifications.Create(notification);
                await _unitOfWork.Withdrawals.Create(withdrawal);

                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Withdraw in progress!";
                    responseMessage.Data = await GetWithdrawalsByUserID(userID);
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Withdrawal not successful!";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Withdrawal failed. Try Again!";
            }

            return responseMessage;
        }
    }
}
