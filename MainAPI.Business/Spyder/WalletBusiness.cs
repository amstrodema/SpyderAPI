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
    public class WalletBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public WalletBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Wallet>> GetWallets() =>
          await _unitOfWork.Wallets.GetAll();
        public async Task<List<Wallet>> GetWalletsByLegOneUserID(Guid userID) =>
          await _unitOfWork.Wallets.GetWalletsByLegOneUserID(userID);
        public async Task<List<Wallet>> GetWalletsByLegTwoUserID(Guid userID) =>
          await _unitOfWork.Wallets.GetWalletsByLegTwoUserID(userID);

        public async Task<Wallet> GetWalletByID(Guid id) =>
                  await _unitOfWork.Wallets.Find(id);

        public async Task<Wallet> GetWalletByRefCode(string refCode) =>
                  await _unitOfWork.Wallets.GetWalletByRefCode(refCode);
        public async Task<Wallet> GetWalletByUserID(Guid userID) =>
                  await _unitOfWork.Wallets.GetWalletByUserID(userID);
        public async Task<ResponseMessage<Wallet>> GetWalletDetailsByUserID(Guid userID)
        {
            ResponseMessage<Wallet> responseMessage = new ResponseMessage<Wallet>();
            Wallet wallet = await _unitOfWork.Wallets.GetWalletByUserID(userID);
            User user = await _unitOfWork.Users.Find(userID);

            bool isChanged = false;
            try
            {
                if (!user.IsActivated)
                {
                    try
                    {
                        Params param = await _unitOfWork.Params.GetParamByCode("activation_cost");
                        decimal activation_cost = decimal.Parse(param.Value);
                        isChanged = true;

                        wallet.ActivationCost = activation_cost;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                if (wallet.BonusDeadline < DateTime.Now)
                {
                    wallet.Bonus = 0;
                    isChanged = true;
                }

                try
                {
                    if (isChanged)
                    {
                        _unitOfWork.Wallets.Update(wallet);
                        await _unitOfWork.Commit();
                    }
                }
                catch (Exception)
                {

                    throw;
                }

                wallet.Ref = await CalculateRefPayment(userID);
                responseMessage.Data = wallet;

                responseMessage.StatusCode = 200;
                responseMessage.Message = "Wallet loaded successfully";
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Wallet failed to load";
            }

            return responseMessage;
        }

        private async Task<decimal> CalculateRefPayment(Guid userID)
        {

            try
            {
                var legOneWallets = await _unitOfWork.Wallets.GetWalletsByUnpiadLegOneUserID(userID);
                var legTwoWallets = await _unitOfWork.Wallets.GetWalletsByUnpaidLegTwoUserID(userID);

                var total = (from wallet in legOneWallets
                             join user in await _unitOfWork.Users.GetAll() on wallet.UserID equals user.ID where user.IsActivated
                             select new Wallet()
                             {
                                 Ref = wallet.ActivationCost * wallet.LegOnePercentage/100
                             }).Sum(p => p.Ref);

                total += (from wallet in legTwoWallets
                          join user in await _unitOfWork.Users.GetAll() on wallet.UserID equals user.ID
                          where user.IsActivated
                          select new Wallet()
                          {
                              Ref = wallet.ActivationCost * wallet.LegTwoPercentage / 100
                          }).Sum(p => p.Ref);


                return total;
            }
            catch (Exception)
            {
                throw;
            }
        }
          public async Task<decimal> ProcessRefPayment(Guid userID)
        {

            try
            {
                var legOneWallets = await _unitOfWork.Wallets.GetWalletsByUnpiadLegOneUserID(userID);
                var legTwoWallets = await _unitOfWork.Wallets.GetWalletsByUnpaidLegTwoUserID(userID);

                var total = (from wallet in legOneWallets where !wallet.IsBanned && !wallet.IsLocked && wallet.IsActive
                             join user in await _unitOfWork.Users.GetAll() on wallet.UserID equals user.ID
                             where user.IsActivated
                             select new Wallet()
                             {
                                 Ref = wallet.ActivationCost * wallet.LegOnePercentage/100
                             }).Sum(p => p.Ref);

                total += (from wallet in legTwoWallets
                          where !wallet.IsBanned && !wallet.IsLocked && wallet.IsActive
                          join user in await _unitOfWork.Users.GetAll() on wallet.UserID equals user.ID
                          where user.IsActivated
                          select new Wallet()
                          {
                              Ref = wallet.ActivationCost * wallet.LegTwoPercentage/100
                          }).Sum(p => p.Ref);

                if (total <= 0)
                {
                    throw new Exception();
                }


                for (int i = 0; i < legOneWallets.Count(); i++)
                {
                    legOneWallets[i].IsPaidLegOne = true;
                }

                for (int i = 0; i < legTwoWallets.Count(); i++)
                {
                    legTwoWallets[i].IsPaidLegTwo = true;
                }

                legOneWallets.AddRange(legTwoWallets);
                _unitOfWork.Wallets.UpdateMultiple(legOneWallets.ToArray());

                return total;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResponseMessage<Wallet>> Recharge(Payment payment)
        {
            ResponseMessage<Wallet> responseMessage = new ResponseMessage<Wallet>();
            Params spy_naira_exchange = await _unitOfWork.Params.GetParamByCode("spy_naira_exchange");
            Params spy_purchase_divider = await _unitOfWork.Params.GetParamByCode("spy_purchase_divider");
            decimal spyAmt = payment.Amount / decimal.Parse(spy_naira_exchange.Value);

            Wallet wallet;
            try
            {
                wallet = await GetWalletByUserID(payment.UserID);
                decimal delimiter = wallet.Gem / decimal.Parse(spy_purchase_divider.Value);
                spyAmt = spyAmt > delimiter ? delimiter: spyAmt;

                wallet.Spy += spyAmt;
                wallet.DateModified = DateTime.Now;
                wallet.Gem -= spyAmt * decimal.Parse(spy_purchase_divider.Value);

                payment.DateCreated = DateTime.Now;
                payment.ID = Guid.NewGuid();

                Transaction newTransaction = new Transaction()
                {
                    ReceiverRefCode = wallet.RefCode,
                    ReceiverID = wallet.UserID,
                    SenderWalletID = default,
                    TransactionCurrency = "SPY",
                    TransactionType = "Credit",
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    IsConfirmed = true,
                    IsActive = true,
                    IsOfficial = true,
                    ReceiverWalletID = wallet.ID,
                    Amount =payment.Amount,
                    CountryID = wallet.UserCountryID,
                    TransactionDesc = $"Recharge of {spyAmt} SPY",
                    SenderRefCode = payment.TrxRef,
                    SenderID = default
                };

                Notification notification = new Notification()
                {
                    RecieverID = payment.UserID,
                    CreatedBy = default,
                    DateCreated = payment.DateCreated,
                    ID = Guid.NewGuid(),
                    IsActive = true,
                    IsRead = false,
                    IsSpyder = true,
                    Message = $"Credit: {payment.Transaction} Amt: {spyAmt}SPY Date: {payment.DateCreated.ToString("F")} Desc: Credit/SPYDER TrxID: {payment.TrxRef}. Bal:{wallet.Spy}"

                };

                await _unitOfWork.Transactions.Create(newTransaction);
                await _unitOfWork.Notifications.Create(notification);
                await _unitOfWork.Payments.Create(payment);
                _unitOfWork.Wallets.Update(wallet);


                if (await _unitOfWork.Commit() > 0)
                {
                    responseMessage.Data = wallet;
                    responseMessage.Message = "Recharge successful!";
                    responseMessage.StatusCode = 200;

                    return responseMessage;
                }
                else
                {
                    responseMessage.StatusCode = 1018;
                    responseMessage.Message = "Recharge failed!";
                    return responseMessage;
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Invalid Operation! Contact Admin";
                return responseMessage;
            }
        }
        public async Task<ResponseMessage<WalletVM>> GetWalletDetails(Guid userID)
        {
            ResponseMessage<WalletVM> responseMessage = new ResponseMessage<WalletVM>();
            try
            {
                WalletVM walletVM = new WalletVM();
                var wallets = await GetWallets();

                walletVM.Wallet = wallets.Find(x => x.UserID == userID);
                walletVM.FirstLegWallets = (from wallet in wallets.Where(x => x.LegOneUserID == userID)
                                            select new Wallet()
                                            {
                                                RefCode = wallet.RefCode,
                                                DateCreated = wallet.DateCreated,
                                                IsPaidLegOne = wallet.IsPaidLegOne,
                                                IsActive = wallet.IsActive,
                                                IsBanned = wallet.IsBanned
                                            }).ToArray();

                walletVM.SecondLegWallets = (from wallet in wallets.Where(x => x.LegTwoUserID == userID)
                                             select new Wallet()
                                             {
                                                 RefCode = wallet.RefCode,
                                                 DateCreated = wallet.DateCreated,
                                                 IsPaidLegTwo = wallet.IsPaidLegTwo,
                                                 IsActive = wallet.IsActive,
                                                 IsBanned = wallet.IsBanned
                                             }).ToArray();

                walletVM.TotalFirstLeg = walletVM.FirstLegWallets.Length;
                walletVM.TotalSecondLeg = walletVM.SecondLegWallets.Length;
                walletVM.UnpaidFirstLegBalance = walletVM.FirstLegWallets.Where(x => !x.IsPaidLegOne).Count();
                walletVM.UnpaidSecondLegBalance = walletVM.SecondLegWallets.Where(x => !x.IsPaidLegTwo).Count();

                responseMessage.Data = walletVM;
                responseMessage.StatusCode = 200;
                responseMessage.Message = "Success";
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Failed. Try Again!";
            }

            return responseMessage;
        }

        public async Task<ResponseMessage<Wallet>> Create(Wallet wallet)
        {
            ResponseMessage<Wallet> responseMessage = new ResponseMessage<Wallet>();
            try
            {
                wallet.DateCreated = DateTime.Now;
                wallet.ID = Guid.NewGuid();
                wallet.IsActive = true;

                await _unitOfWork.Wallets.Create(wallet);
                await _unitOfWork.Commit();

                responseMessage.Data = wallet;
                responseMessage.StatusCode = 200;
                responseMessage.Message = "Success";
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Failed. Try Again!";
            }

            return responseMessage;
        }
        public async Task<ResponseMessage<Transaction>> Payment(Guid senderID, decimal amount, Guid countryID, string desc, string senderRef)
        {
            ResponseMessage<Transaction> responseMessage = new ResponseMessage<Transaction>();
            Wallet spyderWallet;

            try
            {
                spyderWallet = await GetWalletByRefCode("SpyderUnique");
                if (spyderWallet == null)
                {
                    await SetUpCompanyWallet();
                    spyderWallet = await GetWalletByRefCode("SpyderUnique");
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Failed. Try Again!";
                return responseMessage;
            }

            try
            {
                var payerWallet = await GetWalletByUserID(senderID);

                Transaction newTransaction = new Transaction()
                {
                    ReceiverRefCode = spyderWallet.RefCode,
                    ReceiverID = spyderWallet.ID,
                    SenderWalletID = payerWallet.ID,
                    TransactionCurrency = "SPY",
                    TransactionType = "Debit",
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    IsConfirmed = true,
                    IsActive = true,
                    IsOfficial = true,
                    ReceiverWalletID = spyderWallet.ID,
                    Amount = amount,
                    CountryID = countryID,
                    TransactionDesc = desc,
                    SenderRefCode = senderRef,
                    SenderID = senderID
                };

                if (payerWallet.BonusDeadline < DateTime.Now)
                {
                    payerWallet.Bonus = 0;
                }

                if (payerWallet.Spy + payerWallet.Bonus >= newTransaction.Amount)
                {

                    spyderWallet.Spy += newTransaction.Amount;

                    payerWallet.Bonus -= newTransaction.Amount;

                    if (payerWallet.Bonus < 0)
                    {
                        payerWallet.Spy += payerWallet.Bonus;
                        payerWallet.Bonus = 0;
                    }

                    Notification payerNotification = new Notification()
                    {
                        RecieverID = newTransaction.SenderID,
                        CreatedBy = default,
                        DateCreated = newTransaction.DateCreated,
                        ID = Guid.NewGuid(),
                        IsActive = true,
                        IsRead = false,
                        IsSpyder = true,
                        Message = $"Debit: {newTransaction.SenderRefCode} Amt: {newTransaction.Amount}SPY Date: {newTransaction.DateCreated.ToString("F")} Desc: PYT/SPYDER TrxID: {newTransaction.ID}. SPY Bal:{payerWallet.Spy} SPY Bonus:{payerWallet.Bonus}"

                    };

                    await _unitOfWork.Notifications.Create(payerNotification);
                    _unitOfWork.Wallets.Update(spyderWallet);
                    _unitOfWork.Wallets.Update(payerWallet);
                    await _unitOfWork.Transactions.Create(newTransaction);

                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Transaction Successful!";
                    return responseMessage;
                    //if (await _unitOfWork.Commit() >= 1)
                    //{
                    //    responseMessage.Data = newTransaction;
                    //    responseMessage.StatusCode = 200;
                    //    responseMessage.Message = "Transaction Successful!";
                    //    return responseMessage;
                    //}
                    //else
                    //{
                    //    responseMessage.StatusCode = 201;
                    //    responseMessage.Message = "Transaction not successful!";
                    //    return responseMessage;
                    //}
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Insufficient Fund!";
                    return responseMessage;
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Wallet not found!";
                return responseMessage;
            }

        }

        public async Task<ResponseMessage<Wallet>> Transfer(Transaction newTransaction)
        {
            ResponseMessage<Wallet> responseMessage = new ResponseMessage<Wallet>();
            try
            {
                Wallet receiverWallet = default;

                try
                {
                    if (string.IsNullOrWhiteSpace(newTransaction.WalletAddress) && string.IsNullOrWhiteSpace(newTransaction.ReceiverRefCode))
                    {
                        responseMessage.StatusCode = 201;
                        responseMessage.Message = "Wallet not found!";
                        return responseMessage;
                    }
                    else if (!string.IsNullOrWhiteSpace(newTransaction.WalletAddress))
                    {
                        receiverWallet = await _unitOfWork.Wallets.GetWalletByAddress(newTransaction.WalletAddress);
                    }
                    else
                    {
                        var split = newTransaction.ReceiverRefCode.Split('.');
                        receiverWallet = await GetWalletByRefCode(split[0]);
                    }

                    if (receiverWallet == default)
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Wallet not found!";
                    return responseMessage;
                }


                Params network_fee_percentageParam = await _unitOfWork.Params.GetParamByCode("network_fee_percentage");
                decimal network_fee_percentage = 0;

                try
                {
                    network_fee_percentage = decimal.Parse(network_fee_percentageParam.Value);
                }
                catch (Exception)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Try again...";
                    return responseMessage;
                }

                Wallet senderWallet = await GetWalletByUserID(newTransaction.SenderID);

                if (senderWallet.Spy >= newTransaction.Amount)
                {
                    newTransaction.NetworkFee = newTransaction.Amount * (network_fee_percentage*1.0M / 100);
                    receiverWallet.Spy += newTransaction.Amount - newTransaction.NetworkFee;
                    senderWallet.Spy -= newTransaction.Amount;

                    newTransaction.ReceiverRefCode = receiverWallet.RefCode;
                    newTransaction.ReceiverID = receiverWallet.UserID;
                    newTransaction.SenderWalletID = senderWallet.ID;
                    newTransaction.SenderRefCode = senderWallet.RefCode;
                    newTransaction.TransactionCurrency = "SPY";
                    newTransaction.TransactionType = "Transfer";
                    newTransaction.ID = Guid.NewGuid();
                    newTransaction.DateCreated = DateTime.Now;
                    newTransaction.IsConfirmed = true;
                    newTransaction.IsActive = true;
                    newTransaction.IsOfficial = false;
                    newTransaction.ReceiverWalletID = receiverWallet.ID;

                    Notification senderNotification = new Notification()
                    {
                        RecieverID = newTransaction.SenderID,
                        CreatedBy = default,
                        DateCreated = newTransaction.DateCreated,
                        ID = Guid.NewGuid(),
                        IsActive = true,
                        IsRead = false,
                        IsSpyder = true,
                        Message = $"Debit:{newTransaction.SenderRefCode} Amt:{newTransaction.Amount}SPY Date: {newTransaction.DateCreated.ToString("F")} Desc: Trf {newTransaction.ReceiverRefCode} TrxID: {newTransaction.ID}. Bal:{senderWallet.Spy}"

                    };

                    Notification receiverNotification = new Notification()
                    {
                        RecieverID = newTransaction.ReceiverID,
                        CreatedBy = default,
                        DateCreated = newTransaction.DateCreated,
                        ID = Guid.NewGuid(),
                        IsActive = true,
                        IsRead = false,
                        IsSpyder = true,
                        Message = $"Credit:{newTransaction.ReceiverRefCode} Amt:{newTransaction.Amount}SPY Date: {newTransaction.DateCreated.ToString("F")} TRF: {newTransaction.SenderRefCode} TrxID: {newTransaction.ID}. Bal:{receiverWallet.Spy}"
                    };

                    await _unitOfWork.Notifications.Create(senderNotification);
                    await _unitOfWork.Notifications.Create(receiverNotification);
                    _unitOfWork.Wallets.Update(receiverWallet);
                    _unitOfWork.Wallets.Update(senderWallet);
                    await _unitOfWork.Transactions.Create(newTransaction);

                    if (await _unitOfWork.Commit() >= 1)
                    {
                        responseMessage.Data  = await GetWalletByUserID(newTransaction.SenderID); ;
                        responseMessage.StatusCode = 200;
                        responseMessage.Message = $"{newTransaction.Amount} SPY Transferred!";
                    }
                    else
                    {
                        responseMessage.StatusCode = 201;
                        responseMessage.Message = "Transaction not successful!";
                    }
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Insufficient Fund!";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Failed. Try Again!";
            }
            return responseMessage;
        }

        public async Task<ResponseMessage<Wallet>> Swap(Transaction newTransaction)
        {
            ResponseMessage<Wallet> responseMessage = new ResponseMessage<Wallet>();
            try
            {
                Wallet senderWallet = await GetWalletByID(newTransaction.SenderWalletID);
                Params param = await _unitOfWork.Params.GetParamByCode("spy_gem_exchange");

                if (param == null)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Transaction not successful!";
                    return responseMessage;
                }

                decimal exchange = Decimal.Parse(param.Value);

                if (senderWallet == null)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Wallet not found!";
                    return responseMessage;
                }

                Params network_fee_percentageParam = await _unitOfWork.Params.GetParamByCode("network_fee_percentage");
                decimal network_fee_percentage = 0;

                try
                {
                    network_fee_percentage = decimal.Parse(network_fee_percentageParam.Value);
                }
                catch (Exception)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Try again...";
                    return responseMessage;
                }

                if (senderWallet.Gem >= newTransaction.Amount)
                {
                    newTransaction.NetworkFee = newTransaction.Amount * (network_fee_percentage*1.0M / 100);

                    senderWallet.Spy += (newTransaction.Amount - newTransaction.NetworkFee) / exchange;
                    senderWallet.Gem -= newTransaction.Amount;

                    newTransaction.ReceiverRefCode = senderWallet.RefCode;
                    newTransaction.ReceiverID = senderWallet.ID;
                    newTransaction.SenderWalletID = senderWallet.ID;
                    newTransaction.TransactionCurrency = "GEM - SPY";
                    newTransaction.TransactionType = "Swap";
                    newTransaction.ID = Guid.NewGuid();
                    newTransaction.DateCreated = DateTime.Now;
                    newTransaction.IsConfirmed = true;
                    newTransaction.IsActive = true;
                    newTransaction.IsOfficial = true;
                    newTransaction.ReceiverWalletID = senderWallet.ID;

                    Notification senderNotification = new Notification()
                    {
                        RecieverID = newTransaction.SenderID,
                        CreatedBy = default,
                        DateCreated = newTransaction.DateCreated,
                        ID = Guid.NewGuid(),
                        IsActive = true,
                        IsRead = false,
                        IsSpyder = true,
                        Message = $"Debit: {newTransaction.SenderRefCode} Amt: {newTransaction.Amount}GEM Date: {newTransaction.DateCreated.ToString("F")} Desc: Trf {newTransaction.ReceiverRefCode} NetworkFee: {newTransaction.NetworkFee} TrxID: {newTransaction.ID}. Bal:{senderWallet.Gem}"

                    };

                    Notification receiverNotification = new Notification()
                    {
                        RecieverID = newTransaction.ReceiverID,
                        CreatedBy = default,
                        DateCreated = newTransaction.DateCreated,
                        ID = Guid.NewGuid(),
                        IsActive = true,
                        IsRead = false,
                        IsSpyder = true,
                        Message = $"Credit: {newTransaction.ReceiverRefCode} Amt: {newTransaction.Amount}SPY Date: {newTransaction.DateCreated.ToString("F")} TRF: {newTransaction.SenderRefCode} TrxID: {newTransaction.ID}. Bal:{senderWallet.Spy}"
                    };

                    _unitOfWork.Wallets.Update(senderWallet);
                    await _unitOfWork.Transactions.Create(newTransaction);
                    await _unitOfWork.Notifications.Create(senderNotification);
                    await _unitOfWork.Notifications.Create(receiverNotification);

                    if (await _unitOfWork.Commit() >= 1)
                    {
                        responseMessage.Data = await GetWalletByID(newTransaction.SenderWalletID);
                        responseMessage.StatusCode = 200;
                        responseMessage.Message = $"{newTransaction.Amount} GEM Swapped!";
                    }
                    else
                    {
                        responseMessage.StatusCode = 201;
                        responseMessage.Message = "Transaction not successful!";
                    }
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Insufficient Fund!";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Failed. Try Again!";
            }
            return responseMessage;
        }

        public async Task<IEnumerable<Wallet>> GetReferalWallets(Guid userID)
        {
            try
            {
                var wallets = from wallet in await GetWallets()
                              join user in await _unitOfWork.Users.GetAll() on wallet.UserID equals user.ID
                              select new Wallet()
                              {
                                  RefCode = wallet.RefCode,
                                  DateCreated = wallet.DateCreated,
                                  IsActive = wallet.IsActive,
                                  LegOneUserID = wallet.LegOneUserID,
                                  UserID = wallet.UserID
                              };

                var referrals = from thisWallet in wallets where thisWallet.LegOneUserID == userID
                                    join secondLeg in wallets on thisWallet.UserID equals secondLeg.LegOneUserID into thisWalletSecondLegs
                                    select new Wallet()
                                    {
                                        RefCode = thisWallet.RefCode,
                                        Ref = thisWalletSecondLegs.Where(s=> s.IsActive).Count(),
                                        DateCreated = thisWallet.DateCreated,
                                        IsActive = thisWallet.IsActive
                                    };

                return referrals;
            }
            catch (Exception)
            {
                return default;
            }
        }
        private async Task<Wallet> SetUpCompanyWallet()
        {
            ResponseMessage<Wallet> responseMessage = new ResponseMessage<Wallet>();
            try
            {
                Wallet wallet = new Wallet()
                {
                    DateCreated = DateTime.Now,
                    ID = Guid.NewGuid(),
                    IsActive = true,
                    RefCode = "SpyderUnique",
                    IsOfficial = true
                };

                await _unitOfWork.Wallets.Create(wallet);
                await _unitOfWork.Commit();

                return wallet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetWalletByID(id);
            _unitOfWork.Wallets.Delete(entity);
            await _unitOfWork.Commit();
        }

        public async Task<Wallet> SetNewWalletUp(string RefererCode)
        {

            try
            {
                var referralWallet = await GetWalletByRefCode(RefererCode);

                if (referralWallet.IsBanned || referralWallet.IsLocked || !referralWallet.IsActive)
                {
                    throw new Exception();
                }

                Wallet wallet = new Wallet()
                {
                    LegOneUserID = referralWallet.UserID,
                    LegTwoUserID = referralWallet.LegOneUserID,
                    ID = Guid.NewGuid(),
                    Address = GenService.RandomGen50Code(),
                    DateCreated = DateTime.Now,
                    IsActive = false

                };
                return wallet;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ResponseMessage<Wallet>> UpdateBankDetails(RequestObject<Wallet> requestObject)
        {
            ResponseMessage<Wallet> responseMessage = new ResponseMessage<Wallet>();
            try
            {
                Wallet walletProp = requestObject.Data;
                Guid userID = Guid.Parse(requestObject.UserID);
                User user = await _unitOfWork.Users.Find(userID);
                Wallet wallet = await GetWalletByUserID(userID);

                if (EncryptionService.Validate(requestObject.ItemID, user.Password))
                {
                    wallet.BankAccountName = walletProp.BankAccountName;
                    wallet.BankAccountNumber = walletProp.BankAccountNumber;
                    wallet.BankName = walletProp.BankName;

                    wallet.DateModified = DateTime.Now;

                    _unitOfWork.Wallets.Update(wallet);
                    if (await _unitOfWork.Commit() > 0)
                    {
                        responseMessage.Data = await _unitOfWork.Wallets.GetWalletByUserID(userID); ;
                        responseMessage.StatusCode = 200;
                        responseMessage.Message = "Details Updated Successfully";
                    }
                    else
                    {
                        responseMessage.StatusCode = 201;
                        responseMessage.Message = "Details Update Failed.";
                    }
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Update Denied!";
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
