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
    public class UserBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly WalletBusiness _walletBusiness;
        private readonly LogInMonitorBusiness logInMonitorBusiness;
        private readonly CountryBusiness countryBusiness;
        private readonly SettingsBusiness settingsBusiness;

        public UserBusiness(IUnitOfWork unitOfWork, WalletBusiness walletBusiness, LogInMonitorBusiness logInMonitorBusiness, CountryBusiness countryBusiness, SettingsBusiness settingsBusiness)
        {
            _unitOfWork = unitOfWork;
            _walletBusiness = walletBusiness;
            this.logInMonitorBusiness = logInMonitorBusiness;
            this.countryBusiness = countryBusiness;
            this.settingsBusiness = settingsBusiness;
        }

        public async Task<List<User>> GetUsers() =>
          await _unitOfWork.Users.GetAll();

        public async Task<User> GetUserByID(Guid id) =>
                  await _unitOfWork.Users.Find(id);
        public async Task<ResponseMessage<ProfileVM>> GetUserProfile(Guid id)
        {
            ResponseMessage<ProfileVM> responseMessage = new ResponseMessage<ProfileVM>();
            try
            {
                var user = await _unitOfWork.Users.Find(id);
                if (user == null)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Unable to retrieve profile data";

                    return responseMessage;
                }
                ProfileVM profile = new ProfileVM()
                {
                    Name = $"{user.Username}",
                    ID = user.ID,
                    Username = user.Username,
                    City = user.City,
                    Country = (await _unitOfWork.Countries.Find(user.CountryID)).Name,
                    Image = user.Image
                };
                responseMessage.StatusCode = 200;
                responseMessage.Data = profile;
            }
            catch (Exception e)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Unable to retrieve data";
            }

            return responseMessage;
        }
                  

        public async Task<User> GetUserByEmail(string email) =>
                  await _unitOfWork.Users.GetUserByEmail(email);
        public async Task<User> GetUserByUserName(string username) =>
                  await _unitOfWork.Users.GetUserByUserName(username);
        public async Task<User> GetUserByRefCode(string refCode) =>
                  await _unitOfWork.Users.GetUserByRefCode(refCode);

        public async Task<IEnumerable<User>> GetUsersByAccessLevel(int accessLevel) =>
                  await _unitOfWork.Users.GetUsersByAccessLevel(accessLevel);
        public async Task<User> GetUserByEmailVerification(string emailVerification) =>
                  await _unitOfWork.Users.GetUserByEmailVerification(emailVerification);
        public async Task<User> GetUserByResetVerification(string resetVerification) =>
                  await _unitOfWork.Users.GetUserByResetVerification(resetVerification);

        public async Task<ResponseMessage<string>> LogOut(string id)
        {
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();
            try
            {
                Guid userID = Guid.Parse(id);
                var monitor = await _unitOfWork.LogInMonitors.GetLogInMonitorByUserID(userID);

                if (monitor == null)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "User not logged in";
                    return responseMessage;
                }

                _unitOfWork.LogInMonitors.Delete(monitor);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Logged out successfully";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Log out not successful";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 200;
                responseMessage.Message = "Logged out";
            }

            return responseMessage;
        }
        public async Task<ResponseMessage<string>> CheckUsername(string username)
        {
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();
            try
            {
                var user = await GetUserByUserName(username);
                if (user != null)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Username exists already";
                }
                else
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Username is available";
                }
            }
            catch (Exception)
            {

            }

            return responseMessage;
        }

        public async Task<ResponseMessage<User>> Create(User user)
        {
            ResponseMessage<User> responseMessage = new ResponseMessage<User>();
            try
            {
                if (string.IsNullOrWhiteSpace(user.Username))
                {
                    responseMessage.StatusCode = 1018;
                    responseMessage.Message = "Username invalid";
                    return responseMessage;
                }
                if (user.Username.ToLower().Contains("spyder") || user.Username.ToLower().Contains("amstrodema"))
                {
                    responseMessage.StatusCode = 1018;
                    responseMessage.Message = "Username invalid";
                    return responseMessage;
                }

                if (await GetUserByUserName(user.Username) != null)
                {
                    responseMessage.StatusCode = 1018;
                    responseMessage.Message = "Username exists already";
                    return responseMessage;
                }
                if (await GetUserByEmail(user.Email) != null)
                {
                    responseMessage.StatusCode = 1018;
                    responseMessage.Message = "Email exists already";
                    return responseMessage;
                }

                user.Password = EncryptionService.Encrypt(user.Password);
                user.DateCreated = DateTime.Now;
                user.ID = Guid.NewGuid();
                user.RefCode = user.Username + GenService.RandomGen5Code();
                user.AccessLevel = 1;
                user.EmailVerification = user.Username + GenService.Gen10DigitCode();
                user.EmailVerificationDate = DateTime.Now;


                Guid defaultCountryID = await SetDefaultUserCountry();

                if (user.CountryID == default)
                {
                    user.CountryID = defaultCountryID;
                }
                Wallet wallet = new Wallet();
                try
                {
                    wallet = await _walletBusiness.SetNewWalletUp(user.RefererCode);
                    wallet.UserID = user.ID;
                    wallet.RefCode = user.RefCode;
                    user.WalletID = wallet.ID;
                }
                catch (Exception)
                {
                    var referralWallet = await _unitOfWork.Wallets.GetWalletByRefCode("amstrodema");

                    if (referralWallet == null)
                    {
                        referralWallet = await SetSystemDefaults(defaultCountryID);
                    }

                    wallet = new Wallet()
                    {
                        LegOneUserID = referralWallet.UserID,
                        LegTwoUserID = referralWallet.LegOneUserID,
                        Spy = 500,
                        ID = Guid.NewGuid(),
                        Address = GenService.RandomGen50Code(),
                        DateCreated = DateTime.Now,
                        Gem = 10000,
                        IsActive = false

                    };
                    wallet.UserID = user.ID;
                    wallet.RefCode = user.RefCode;
                    user.WalletID = wallet.ID;
                    user.RefererCode = referralWallet.RefCode;

                }

                wallet.Spy = await SetParams("initial_spy", "Initial SPY", "10");
                wallet.Gem = await SetParams("initial_gem", "Initial Gem", "1000");

                Settings settings = new Settings()
                {
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    IsAllowMessaging = true,
                    IsShowEmail = false,
                    UserID = user.ID,
                    ViewCountryID = user.CountryID,
                    IsAllowAccess = true,
                    IsAnoymousMessaging = true,
                    IsLocalRange = false,
                    IsReactionNotification = true,
                    IsRecieveAnoymousMessages = true,
                    IsSendNotificationToMail = false,
                    IsShowPhoneNo = false,

                };


                // user.WalletID

                wallet.LegOnePercentage = await SetParams("percentage1", "1st Leg Referral Percentage", "20");
                wallet.LegTwoPercentage = await SetParams("percentage2", "2nd Leg Referral Percentage", "10");
                wallet.ActivationCost = await SetParams("activation_cost", "Activation Cost", "1000");
              


                await _unitOfWork.Users.Create(user);
                await _unitOfWork.Wallets.Create(wallet);
                await _unitOfWork.Settings.Create(settings);

                await _unitOfWork.Commit();

                //To do: Send verification link to email

                responseMessage.Data = user;
                responseMessage.StatusCode = 200;
                responseMessage.Message = "Success";
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "An error occurred. Try Again!";
            }

            return responseMessage;
        }

        private async Task<User> SetTopAdminUser(string username, string password, string refCode, string address, Guid countryID)
        {
            User spyderUser = new User()
            {
                AccessLevel = 7,
                Address = address,
                ID = Guid.NewGuid(),
                Username = username,
                IsActivated = true,
                IsActive = true,
                IsVerified = true,
                DateCreated = DateTime.Now,
                RefCode = refCode,
                Password = EncryptionService.Encrypt(password),
                CountryID = countryID
            };
            await _unitOfWork.Users.Create(spyderUser);

            return spyderUser;
        }

        private async Task<Wallet> SetTopAdminWallet(decimal spy, decimal gem, string refCode, Guid userID, Guid leg1 = default, Guid leg2 = default)
        {
            Wallet wallet = new Wallet()
            {
                ID = Guid.NewGuid(),
                IsActive = true,
                Gem = gem,
                Spy = spy,
                DateCreated = DateTime.Now,
                Address = GenService.RandomGen50Code(),
                RefCode = refCode,
                IsOfficial = true,
                UserID = userID,

            };
            if (leg1 != default)
            {

                wallet.LegOneUserID = leg1;
                wallet.LegTwoUserID = leg2;
            }
            await _unitOfWork.Wallets.Create(wallet);

            return wallet;
        }

        private async Task<Guid> SetDefaultUserCountry()
        {
            var country = await _unitOfWork.Countries.GetCountryByCountryAbbrev("NGN");
            if (country == null)
            {
                country = new Country()
                {
                    Abbrv = "NGN",
                    Code = "+234",
                    DateCreated = DateTime.Now,
                    CreatedBy = default,
                    ID = Guid.NewGuid(),
                    IsActive = true,
                    Name = "Nigeria"
                };

                await _unitOfWork.Countries.Create(country);
            }
            return country.ID;
        }
        private async Task<Wallet> SetSystemDefaults(Guid defaultCountryID)
        {

            await SetParams("spy_gem_exchange", "Gem to SPY rate", "100000");
            await SetParams("spy_purchase_divider", "Gem divider to buy SPY", "1000");
            await SetParams("min_withrawal", "Minimum Withdrawal", "3000");
            await SetParams("spy_naira_exchange", "SPY to Naira", "100");
            await SetParams("vote_cost", "Vote Cost", "1");
            await SetParams("gem_gain_percentage", "Percentage Gem Gain Per Action", "1");

            await SetParams("comment_cost", "Comment Cost", "0.1");
            await SetParams("like_action_cost", "Like Action Cost", "0.05");
            await SetParams("marriage_cost", "Marriage Cost", "10");
            await SetParams("death_cost", "Death Cost", "10");
            await SetParams("missing_cost", "Missing Cost", "5");
            await SetParams("confession_cost", "Confession Cost", "10");
            await SetParams("gallery_link_cost", "Gallery Link Cost", "0.1");
            await SetParams("gallery_image_cost", "Gallery Image Cost", "0.2");

            await SetParams("network_fee_percentage", "Network Fee", "0.1");



            User spyderAdmin = await SetTopAdminUser("spyder", "MainHomePassword!!09!!", "spyder", "Spyder", defaultCountryID);
            User userAdmin = await SetTopAdminUser("amstrodema", "Main!!001Password!36", "amstrodema", "Spyder", defaultCountryID);
            Wallet spyderAdminWallet = await SetTopAdminWallet(90000000, 50000, spyderAdmin.RefCode, spyderAdmin.ID);
            Wallet userAdminWallet = await SetTopAdminWallet(900000, 5000, userAdmin.RefCode, userAdmin.ID, spyderAdminWallet.UserID, spyderAdminWallet.UserID);

            await _unitOfWork.FeatureGroups.Create(new Models.Spyder.Feature.FeatureGroup()
            {
                Code = "Marriage_Type",
                Name = "Marriage Type",
                CreatedBy = userAdmin.ID,
                DateCreated = DateTime.Now,
                GroupNo = 2,
                ID = Guid.NewGuid(),
                IsActive = true
            });

            await _unitOfWork.FeatureGroups.Create(new Models.Spyder.Feature.FeatureGroup()
            {
                Code = "Marriage_Feature",
                Name = "Marriage Feature",
                CreatedBy = userAdmin.ID,
                DateCreated = DateTime.Now,
                GroupNo = 3,
                ID = Guid.NewGuid(),
                IsActive = true
            });

            await SetUpHall("Hall of Legends", "hall-of-legends", "legends", 4000000, 190, userAdmin.CountryID, userAdmin.ID);
            await SetUpHall("Hall of Heros", "hall-of-heros", "heros", 150000, 350, userAdmin.CountryID, userAdmin.ID);
            await SetUpHall("Hall of Shame", "hall-of-shame", "hall-of-shame", 500000, 150, userAdmin.CountryID, userAdmin.ID);
            await SetUpHall("Hall of Fame", "hall-of-fame", "hall-of-fame", 100000, 100, userAdmin.CountryID, userAdmin.ID);
            await SetUpHall("Hall of Fallen Heros", "hall-of-fallen-heros", "hall-of-fallen-heros", 100000, 100, userAdmin.CountryID, userAdmin.ID);

            Settings spyderSettings = new Settings()
            {
                ID = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                IsActive = true,
                IsAllowMessaging = false,
                IsShowEmail = false,
                UserID = spyderAdmin.ID,
                ViewCountryID = spyderAdmin.CountryID,
                IsAllowAccess = false,
                IsAnoymousMessaging = false,
                IsLocalRange = false,
                IsReactionNotification = false,
                IsRecieveAnoymousMessages = false,
                IsSendNotificationToMail = false,
                IsShowPhoneNo = false

            };

            Settings userAdminSettings = new Settings()
            {
                ID = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                IsActive = true,
                IsAllowMessaging = false,
                IsShowEmail = false,
                UserID = userAdmin.ID,
                ViewCountryID = userAdmin.CountryID,
                IsAllowAccess = false,
                IsAnoymousMessaging = false,
                IsLocalRange = false,
                IsReactionNotification = false,
                IsRecieveAnoymousMessages = false,
                IsSendNotificationToMail = false,
                IsShowPhoneNo = false

            };

            spyderAdmin.WalletID = spyderAdminWallet.ID;
            spyderAdmin.Email = "webspyder83@gmail.com";
            userAdmin.WalletID = userAdminWallet.ID;
            userAdmin.Email = "oluyinka.akintayo@gmail.com";

            await _unitOfWork.Settings.Create(spyderSettings);
            await _unitOfWork.Settings.Create(userAdminSettings);

            return userAdminWallet;
        }

        private async Task SetUpHall(string name, string clickObj, string hallCode, int requiredVotes, decimal cost, Guid countryID, Guid createdBy)
        {
            await _unitOfWork.Halls.Create(new Models.Spyder.Hall.Hall()
            {
                ID = Guid.NewGuid(),
                Name = name,
                ClickObject = clickObj,
                HallCode = hallCode,
                RequiredVotes = requiredVotes,
                Cost = cost,
                CountryID = countryID,
                DateCreated = DateTime.Now,
                IsActive = true,
                CreatedBy = createdBy
            });
        }

        private async Task<decimal> SetParams(string code, string name, string value)
        {
            Params param = await _unitOfWork.Params.GetParamByCode(code);

            if (param == null)
            {
                param = new Params()
                {
                    ID = Guid.NewGuid(),
                    Code = code,
                    IsActive = true,
                    DateCreated = DateTime.Now,
                    Name = name,
                    Value = value
                };

                await _unitOfWork.Params.Create(param);
            }

            try
            {
                return decimal.Parse(param.Value);
            }
            catch (Exception)
            {
                return default;
            }

        }

        public async Task<ResponseMessage<string>> ResendEmailVerification(string email)
        {
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();
            try
            {
                var user = await GetUserByEmail(email);

                if (user == null)
                {
                    user = await GetUserByUserName(email);
                }

                if (user != null)
                {
                    user.EmailVerification = user.Username + GenService.Gen10DigitCode();
                    user.EmailVerificationDate = DateTime.Now;
                    await Update(user);

                    //To do: Send verification link to email

                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Email verification resent!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Email registration not found!";
                }


            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Something went wrong. Try Again!";
            }

            return responseMessage;

        }



        public async Task<int> Update(User user)
        {
            user.DateModified = DateTime.Now;
            _unitOfWork.Users.Update(user);
            return await _unitOfWork.Commit();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetUserByID(id);
            _unitOfWork.Users.Delete(entity);
            await _unitOfWork.Commit();
        }

        public async Task<ResponseMessage<UserVM>> ValidateUser(LoginVM login)
        {
            ResponseMessage<UserVM> responseMessage = new ResponseMessage<UserVM>();
            try
            {
                User user = await GetUserByUserName(login.Username);
                if (user == null)
                {
                    string email = login.Username;
                    user = await GetUserByEmail(email);
                }

                if (user == null)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Log in unsuccessful";
                    return responseMessage;
                }

                if (login.Portal > 0)
                {
                    if (user.AccessLevel < 3)
                    {
                        responseMessage.StatusCode = 201;
                        responseMessage.Message = "Access Denied";
                        return responseMessage;
                    }
                }

                if (EncryptionService.Validate(login.Password, user.Password))
                {
                    if (!user.IsActive)
                    {
                        responseMessage.StatusCode = 202;
                        responseMessage.Message = "Verify your email address!";
                        return responseMessage;
                    }

                    LogInMonitor logInMonitor = new LogInMonitor()
                    {
                        DateCreated = DateTime.Now,
                        ID = Guid.NewGuid(),
                        IsActive = true,
                        IsUserLoggedIn = true,
                        UserID = user.ID,
                        UserCountryID = user.CountryID,
                        AppID = Guid.NewGuid()
                    };

                    await _unitOfWork.LogInMonitors.Create(logInMonitor);
                    UserVM userVM = new UserVM();

                    try
                    {
                        userVM.Settings = (await settingsBusiness.GetSettingsByUserID(user.ID)).Data;
                    }
                    catch (Exception)
                    {
                        responseMessage.StatusCode = 209;
                        responseMessage.Message = "Login failed. Contact Support For Assistance.";
                        return responseMessage;
                    }
                    ClientSystem clientSystem = new ClientSystem()
                    {
                        AppID = logInMonitor.AppID
                    };

                    if (await _unitOfWork.Commit() > 0)
                    {
                        userVM.User = user;
                        userVM.ClientSystem = clientSystem;
                        responseMessage.Data = userVM;
                        responseMessage.StatusCode = 200;
                        responseMessage.Message = $"Hello, {user.Username}! ";
                    }
                    else
                    {
                        responseMessage.StatusCode = 209;
                        responseMessage.Message = "Login failed. Try again!";
                    }


                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Incorrect login details!";
                }

            }
            catch (Exception e)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Something went wrong. Try Again!" + e;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage<string>> VerifyEmail(string verificationCode)
        {
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();
            try
            {
                User user = await GetUserByEmailVerification(verificationCode);

                if (user.EmailVerification == verificationCode)
                {
                    if (DateTime.Now > user.EmailVerificationDate.AddMinutes(10))
                    {
                        responseMessage.StatusCode = 201;
                        responseMessage.Message = "Verification link has expired!";
                        return responseMessage;
                    }

                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Email verification successful!";
                    user.IsVerified = true;
                    user.IsActive = true;

                    _unitOfWork.Users.Update(user);
                    await _unitOfWork.Commit();
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Verification link is invalid!";
                }


            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Registration not found!";
            }

            return responseMessage;
        }

        public async Task<ResponseMessage<User>> ActivateAccount(Payment payment)
        {
            ResponseMessage<User> responseMessage = new ResponseMessage<User>();
            Guid userID;
            try
            {
                userID = payment.UserID;
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "User not found!";
                return responseMessage;
            }

            //to do: check web3 transaction for payment
            try
            {
                User user = await GetUserByID(userID);
                if (user != null)
                {
                    if (user.IsActivated)
                    {
                        responseMessage.StatusCode = 200;
                        responseMessage.Message = "Account activated already!";
                        return responseMessage;
                    }
                    Wallet wallet = await _walletBusiness.GetWalletByID(user.WalletID);

                    user.IsActivated = true;
                    user.DateModified = DateTime.Now;
                    wallet.IsActive = true;
                    wallet.DateModified = DateTime.Now;
                    payment.DateCreated = DateTime.Now;
                    payment.ID = Guid.NewGuid();

                    await _unitOfWork.Payments.Create(payment);
                    _unitOfWork.Users.Update(user);
                    _unitOfWork.Wallets.Update(wallet);

                    if (await _unitOfWork.Commit() >= 1)
                    {
                        responseMessage.StatusCode = 200;
                        responseMessage.Message = "Account activated successfully!";
                    }
                    else
                    {
                        responseMessage.StatusCode = 201;
                        responseMessage.Message = "Account not activated. Try again!";
                    }

                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "User not found!";
                }

            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Something went wrong. Try Again!";
            }

            return responseMessage;
        }

        public async Task<ResponseMessage<string>> ForgotPassword(string email)
        {
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();

            //to do: check web3 transaction for payment
            try
            {
                User user = await GetUserByEmail(email);

                if (user == null)
                {
                    user = await GetUserByUserName(email);
                }

                if (user != null)
                {
                    user.ResetVerification = GenService.RandomGen50Code();
                    user.ResetVerificationDate = DateTime.Now;

                    if (await Update(user) >= 1)
                    {
                        responseMessage.StatusCode = 200;
                        responseMessage.Message = "Password reset link sent!";
                        //To DO: send reset link to email
                    }
                    else
                    {
                        responseMessage.StatusCode = 201;
                        responseMessage.Message = "Password reset link not sent!";
                    }
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "User not found!";
                }

            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Something went wrong. Try Again!";
            }

            return responseMessage;
        }

        public async Task<ResponseMessage<string>> ResetPassword(string resetPasswordCode)
        {
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();

            //to do: check web3 transaction for payment
            try
            {
                User user = await GetUserByResetVerification(resetPasswordCode);


                if (user != null)
                {
                    if (DateTime.Now > user.ResetVerificationDate.AddMinutes(10))
                    {
                        responseMessage.StatusCode = 201;
                        responseMessage.Message = "Verification link has expired!";
                        return responseMessage;
                    }
                    string newPassword = GenService.Gen10DigitCode();
                    user.Password = EncryptionService.Encrypt(newPassword);
                    user.ResetVerification = "";
                    user.ResetVerificationDate = default;

                    if (await Update(user) >= 1)
                    {
                        responseMessage.StatusCode = 200;
                        responseMessage.Message = "New password is: " + newPassword;
                        //To DO: send password to email
                    }
                    else
                    {
                        responseMessage.StatusCode = 201;
                        responseMessage.Message = "Password reset not successful!";
                    }
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "User not found!";
                }

            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Something went wrong. Try Again!";
            }

            return responseMessage;
        }

        public async Task<bool> IsLoggedIn(Guid userID)
        {
            var isLogged = await logInMonitorBusiness.GetLogInMonitorByUserID(userID);
            if (isLogged == null)
            {
                return false;
            }
            return true;
        }
        public async Task SendEmail(Email email)
        {
            MailServices mailSender = new MailServices(email);
            await mailSender.SendMail();
        }
    }
}
