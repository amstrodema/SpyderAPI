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
        public async Task<IEnumerable<Withdrawal>> GetWithdrawalsByUserID(Guid userID)
        {
            return (await _unitOfWork.Withdrawals.GetWithdrawalsByUserID(userID)).OrderByDescending(p=> p.DateCreated);
        }
        public async Task<ResponseMessage<WithdrawalVM>> PrintPayOut(int statusCode)
        {
            IEnumerable<Withdrawal> withdrawls = new List<Withdrawal>();
            if (statusCode != 0)
            {
                withdrawls = await _unitOfWork.Withdrawals.GetWithdrawalsByStatusCode(statusCode);
            }
            else
            {
                withdrawls = await _unitOfWork.Withdrawals.GetAll();
            }

            ResponseMessage<WithdrawalVM> responseMessage = new ResponseMessage<WithdrawalVM>();
            var users = await _unitOfWork.Users.GetValidUsers();
            var withdrawals = (from withdrawal in withdrawls
                               join user in users on withdrawal.UserID equals user.ID
                               select withdrawal).ToArray();

            int count = 1;

            var payrolls = from withdrw in withdrawals
                     join user in users on withdrw.UserID equals user.ID
                     select new Payroll()
                     {
                         Amount = withdrw.Amount.ToString(),
                         PayDate = withdrw.DateModified== default ? "Not Paid" : withdrw.DateModified.ToString("d"),
                         //RequestDate = withdrw.DateCreated.ToString("d"),
                         Sn = count++,
                         AcctNo = user.BankAccountNumber,
                         Bank = user.BankName,
                         AcctName = user.BankAccountName
                     };

         

            WithdrawalVM withdrawalVM = new WithdrawalVM();
            withdrawalVM.Payrolls = payrolls;
            withdrawalVM.Date = DateTime.Now.ToString("d");

            responseMessage.Data = withdrawalVM;
            responseMessage.StatusCode = 200;
            responseMessage.Message = "Completed";

            return responseMessage;
        }
        
        public async Task<ResponseMessage<WithdrawalVM>> PayOut()
        {
            var date = DateTime.Now;
            ResponseMessage<WithdrawalVM> responseMessage = new ResponseMessage<WithdrawalVM>();
            var users = await _unitOfWork.Users.GetValidUsers();
            var withdrawals = (from withdrawal in await _unitOfWork.Withdrawals.GetWithdrawalsByStatusCode(1)
                               join user in users on withdrawal.UserID equals user.ID
                               select withdrawal).ToArray();

            int count = 1;
            var payrolls = from withdrw in withdrawals
                     join user in users on withdrw.UserID equals user.ID
                     select new Payroll()
                     {
                         Amount = withdrw.Amount.ToString(),
                         PayDate = date.ToString("d"),
                         //RequestDate = withdrw.DateCreated.ToString("d"),
                         Sn = count++,
                         AcctNo = user.BankAccountNumber,
                         Bank = user.BankName,
                         AcctName = user.BankAccountName
                     };

            for (int i = 0; i < withdrawals.Length; i++)
            {
                withdrawals[i].DateModified = date;
                withdrawals[i].Status = "Paid";
                withdrawals[i].StatusCode = 5;
                withdrawals[i].BankAccountName = users.FirstOrDefault(user=> user.ID == withdrawals[i].UserID).BankAccountName;
                withdrawals[i].BankName = users.FirstOrDefault(user => user.ID == withdrawals[i].UserID).BankName;
                withdrawals[i].BankAccountNumber = users.FirstOrDefault(user => user.ID == withdrawals[i].UserID).BankAccountNumber;
            }

            _unitOfWork.Withdrawals.UpdateMultiple(withdrawals);

            try
            {
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Payout Successful";
                }
                else
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "No Payout Made";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Payout Not Successful";
            }

            var res = await GetWithdrawals(0);
            WithdrawalVM withdrawalVM = new WithdrawalVM();
            withdrawalVM.Withdrawals = res;
            withdrawalVM.Payrolls = payrolls;
            withdrawalVM.Date = DateTime.Now.ToString("d");

            responseMessage.Data = withdrawalVM;

            return responseMessage;
        }

        public async Task<IEnumerable<WithdrawalHybrid>> GetWithdrawals(int statusCode)
        {
            if (statusCode <= 0)
            {
                var withdrawals =  from withdrawal in await _unitOfWork.Withdrawals.GetAll()
                               join user in await _unitOfWork.Users.GetAll() on withdrawal.UserID equals user.ID
                               select new WithdrawalHybrid()
                               {
                                   ID = withdrawal.ID,
                                   Amount = withdrawal.Amount,
                                   UserID = withdrawal.UserID,
                                   DateCreated = withdrawal.DateCreated,
                                   DateRequested = withdrawal.DateCreated.ToString("d"),
                                   RefCode = user.RefCode,
                                   Status = withdrawal.Status,
                                   StatusCode = withdrawal.StatusCode,
                                   UserStatus = user.IsBanned || !user.IsActive ? "btn-danger" : "btn-success",
                                   DatePaid = withdrawal.DateModified == default ? "-" : withdrawal.DateModified.ToString("d")
                               };
                return withdrawals.OrderByDescending(o => o.DateCreated);
            }
            else
            {
                var withdrawals = from withdrawal in await _unitOfWork.Withdrawals.GetWithdrawalsByStatusCode(statusCode)
                                 join user in await _unitOfWork.Users.GetAll() on withdrawal.UserID equals user.ID
                                 select new WithdrawalHybrid()
                                 {
                                     ID = withdrawal.ID,
                                     Amount = withdrawal.Amount,
                                     UserID = withdrawal.UserID,
                                     DateCreated = withdrawal.DateCreated,
                                     DateRequested = withdrawal.DateCreated.ToString("d"),
                                     RefCode = user.RefCode,
                                     Status = withdrawal.Status,
                                     StatusCode = withdrawal.StatusCode,
                                     UserStatus = user.IsBanned || !user.IsActive ? "btn-danger" : "btn-success",
                                     DatePaid = withdrawal.DateModified == default ? "-" : withdrawal.DateModified.ToString("d")
                                 };
                return withdrawals.OrderByDescending(o => o.DateCreated);
            }
        }

        public async Task<ResponseMessage<IEnumerable<Withdrawal>>> Create(Guid userID)
        {
            ResponseMessage<IEnumerable<Withdrawal>> responseMessage = new ResponseMessage<IEnumerable<Withdrawal>>();
            try
            {
                var wallet = await _unitOfWork.Wallets.GetWalletByUserID(userID);
                if (wallet.IsBanned || wallet.IsLocked || !wallet.IsActive)
                {
                    throw new Exception();
                }
                decimal amount = await walletBusiness.ProcessRefPayment(userID);

                Withdrawal withdrawal = new Withdrawal()
                {
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    Amount = amount,
                    Status = "Pending",
                    StatusCode = 1,
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
