using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Petition.Spyder;
using MainAPI.Models.Spyder;
using MainAPI.Models.Spyder.Hall;
using MainAPI.Models.ViewModel.Spyder.Petition;
using MainAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder
{
    public class PetitionBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly WalletBusiness walletBusiness;

        public PetitionBusiness(IUnitOfWork unitOfWork, WalletBusiness walletBusiness)
        {
            _unitOfWork = unitOfWork;
            this.walletBusiness = walletBusiness;
        }

        public async Task<List<Petition>> GetPetitions() =>
          await _unitOfWork.Petitions.GetAll();

        public async Task<IEnumerable<PetitionVM>> GetPetitionVMs(RequestObject<string> requestObject)
        {
            Request<Guid> request = new Request<Guid>();

            try
            {
                try
                {
                    request.CountryID = Guid.Parse(requestObject.CountryID);
                }
                catch (Exception)
                {
                }

                request.UserID = Guid.Parse(requestObject.UserID);
                return await GetPetitionVMs_LoggedIn(request);
            }
            catch (Exception)
            {
                var x = from petition in await _unitOfWork.Petitions.GetAll()
                        where !petition.IsApproved
                        join hall in await _unitOfWork.Halls.GetAll() on petition.HallID equals hall.ID
                        join vote in await _unitOfWork.Votes.GetAll() on petition.ID equals vote.ItemID into votes
                        select new PetitionVM()
                        {
                            ID = petition.ID,
                            Brief = petition.Brief,
                            BtnBgTypeDisLike = "",
                            BtnBgTypeLike = "",
                            Day = petition.DateCreated.ToString("d"),
                            HallName = hall.Name,
                            IsApproved = petition.IsApproved,
                            IsLike = false,
                            IsReact = false,
                            RecordOwnerImage = ImageService.GetSmallImageFromFolder(petition.RecordOwnerImage, "Petition"),
                            RecordOwnerName = petition.RecordOwnerName,
                            RecordOwnerStory = petition.RecordOwnerStory,
                            Time = petition.DateCreated.ToString("t"),
                            TotalDownVotes = votes.Where(p => p.IsReact && !p.IsLike).Count(),
                            TotalUpVotes = votes.Where(p => p.IsReact && p.IsLike).Count(),
                            VotePercentage = votes.Where(p => p.IsReact && p.IsLike).Count() *1.0f / petition.RequiredVoters * 100,
                            ClickObject = hall.ClickObject,
                            PetitionerID = petition.IsAnonymous? default : petition.PetitionerID
                        };

                x = x.OrderByDescending(p => p.TotalUpVotes);
                return x;
            }

        }



        public async Task<IEnumerable<PetitionVM>> GetPetitionVMs_LoggedIn(Request<Guid> request)
        {
            var petitions = await _unitOfWork.Petitions.GetAll();
            if (request.CountryID != default)
            {
                petitions = petitions.Where(m => m.PetitionCountryID == request.CountryID).ToList();
            }

            var x = from petition in petitions
                    where !petition.IsApproved 
                    join hall in await _unitOfWork.Halls.GetAll() on petition.HallID equals hall.ID
                    join personalVote in await _unitOfWork.Votes.GetVotesByUserID(request.UserID) on petition.ID equals personalVote.ItemID into votes
                    from thisVote in votes.DefaultIfEmpty()
                    join vote in await _unitOfWork.Votes.GetAll() on petition.ID equals vote.ItemID into allVotes
                    select new PetitionVM()
                    {
                        ID = petition.ID,
                        Brief = petition.Brief,
                        BtnBgTypeDisLike = thisVote == default ? petition.BtnBgTypeDisLike : thisVote.BtnBgTypeDisLike,
                        BtnBgTypeLike = thisVote == default ? petition.BtnBgTypeLike : thisVote.BtnBgTypeLike,
                        Day = petition.DateCreated.ToString("d"),
                        HallName = hall.Name,
                        IsApproved = petition.IsApproved,
                        IsLike = thisVote == default ? petition.IsLike : thisVote.IsLike,
                        IsReact = thisVote == default ? petition.IsReact : thisVote.IsReact,
                        RecordOwnerImage = ImageService.GetSmallImageFromFolder(petition.RecordOwnerImage, "Petition"),
                        RecordOwnerName = petition.RecordOwnerName,
                        RecordOwnerStory = petition.RecordOwnerStory,
                        Time = petition.DateCreated.ToString("t"),
                        TotalDownVotes = allVotes.Where(p => p.IsReact && !p.IsLike).Count(),
                        TotalUpVotes = allVotes.Where(p => p.IsReact && p.IsLike).Count(),
                        VotePercentage = allVotes.Where(p => p.IsReact && p.IsLike).Count() * 1.0f / petition.RequiredVoters * 100,
                        ClickObject = hall.ClickObject,
                        PetitionerID = petition.IsAnonymous ? default: petition.PetitionerID
                    };

            x = x.OrderByDescending(p => p.TotalUpVotes);

            return x;
        }

        public async Task<PetitionVM> GetPetitionDetailsVM_Logged(Request<Guid> request)
        {
            var petitions = await _unitOfWork.Petitions.GetAll();

            if (request.CountryID != default)
            {
                petitions = petitions.Where(p => p.PetitionCountryID == request.CountryID).ToList();
            }

            var x = (from petition in petitions
                     where petition.ID == request.ItemID && !petition.IsApproved
                     join hall in await _unitOfWork.Halls.GetAll() on petition.HallID equals hall.ID
                     join personalVote in await _unitOfWork.Votes.GetVotesByUserID(request.UserID) on petition.ID equals personalVote.ItemID into votes
                     from thisVote in votes.DefaultIfEmpty()
                     join vote in await _unitOfWork.Votes.GetAll() on petition.ID equals vote.ItemID into allVotes
                     from allVote in allVotes.DefaultIfEmpty()
                     join user in await _unitOfWork.Users.GetAll() on petition.PetitionerID equals user.ID
                     select new PetitionVM()
                     {
                         ID = petition.ID,
                         Brief = petition.Brief,
                         BtnBgTypeDisLike = thisVote == default ? petition.BtnBgTypeDisLike : thisVote.BtnBgTypeDisLike,
                         BtnBgTypeLike = thisVote == default ? petition.BtnBgTypeLike : thisVote.BtnBgTypeLike,
                         HallName = hall.Name,
                         IsLike = thisVote == default ? petition.IsLike : thisVote.IsLike,
                         IsReact = thisVote == default ? petition.IsReact : thisVote.IsReact,
                         RecordOwnerImage = ImageService.GetImageFromFolder(petition.RecordOwnerImage, "Petition"),
                         RecordOwnerName = petition.RecordOwnerName,
                         Time = petition.DateCreated.ToString("t"),
                         TotalDownVotes = allVotes.Where(p => p.IsReact && !p.IsLike).Count(),
                         TotalUpVotes = allVotes.Where(p => p.IsReact && p.IsLike).Count(),
                         VotePercentage = allVotes.Where(p => p.IsReact && p.IsLike).Count() * 1.0f / petition.RequiredVoters * 100,
                         Day = petition.DateCreated.ToString("d"),
                         RecordOwnerStory = petition.RecordOwnerStory,
                         Petitioner = petition.IsAnonymous?"Anonymous": user.Username,
                         ClickObject = hall.ClickObject,
                         PetitionerID = petition.IsAnonymous ? default : petition.PetitionerID,
                         TotalVotesRequired = petition.RequiredVoters
                     }).ToList();

            return x[0];
        }

        public async Task<PetitionVM> GetPetitionDetailsVM(RequestObject<string> requestObject)
        {
            Request<Guid> request = new Request<Guid>();
            try
            {
                request.ItemID = Guid.Parse(requestObject.ItemID);

                try
                {
                    try
                    {
                        request.CountryID = Guid.Parse(requestObject.CountryID);
                    }
                    catch (Exception)
                    {
                    }

                    request.UserID = Guid.Parse(requestObject.UserID);

                    return await GetPetitionDetailsVM_Logged(request);
                }
                catch (Exception)
                {
                    var petitions = await _unitOfWork.Petitions.GetAll();
                    if (request.CountryID != default)
                    { 
                        petitions = petitions.Where(p => p.PetitionCountryID == request.CountryID).ToList();
                    }

                    var x = (from petition in petitions
                             where petition.ID == request.ItemID && !petition.IsApproved
                             join hall in await _unitOfWork.Halls.GetAll() on petition.HallID equals hall.ID
                             join vote in await _unitOfWork.Votes.GetAll() on petition.ID equals vote.ItemID into allVotes
                             from allVote in allVotes.DefaultIfEmpty()
                             join user in await _unitOfWork.Users.GetAll() on petition.PetitionerID equals user.ID
                             select new PetitionVM()
                             {
                                 ID = petition.ID,
                                 Brief = petition.Brief,
                                 BtnBgTypeDisLike = "",
                                 BtnBgTypeLike = "",
                                 HallName = hall.Name,
                                 IsLike = false,
                                 IsReact = false,
                                 RecordOwnerImage = ImageService.GetImageFromFolder(petition.RecordOwnerImage, "Petition"),
                                 RecordOwnerName = petition.RecordOwnerName,
                                 Time = petition.DateCreated.ToString("t"),
                                 TotalDownVotes = allVotes.Where(p => p.IsReact && !p.IsLike).Count(),
                                 TotalUpVotes = allVotes.Where(p => p.IsReact && p.IsLike).Count(),
                                 VotePercentage = allVotes.Where(p => p.IsReact && p.IsLike).Count() * 1.0f / petition.RequiredVoters * 100,
                                 Day = petition.DateCreated.ToString("d"),
                                 RecordOwnerStory = petition.RecordOwnerStory,
                                 Petitioner = petition.IsAnonymous ? "Anonymous" : user.Username,
                                 ClickObject = hall.ClickObject,
                                 PetitionerID = petition.IsAnonymous ? default : petition.PetitionerID,
                                 TotalVotesRequired = petition.RequiredVoters
                             }).ToList();

                    return x[0];
                }
            }
            catch (Exception)
            {
                return default;
            }


        }

        public async Task<Petition> GetPetitionByID(Guid id) =>
                  await _unitOfWork.Petitions.Find(id);
        public async Task<ResponseMessage<Petition>> Create(Petition petition)
        {
            ResponseMessage<Petition> responseMessage = new ResponseMessage<Petition>();
            try
            {
                User user = await _unitOfWork.Users.Find(petition.PetitionerID);
                if (user == null)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "User not found! Log in and try again";
                }
                else
                {
                    Hall hall = await _unitOfWork.Halls.Find(petition.HallID);
                    if (hall == null)
                    {
                        responseMessage.StatusCode = 201;
                        responseMessage.Message = "Invalid hall selection!";
                    }
                    else
                    {
                        var res = await walletBusiness.Payment(user.ID, hall.Cost, user.CountryID, $"Cost of setting up a petition for hall: {hall.Name}", user.RefCode);
                        if (res.StatusCode == 200)
                        {
                            petition.ID = Guid.NewGuid();
                            petition.DateCreated = DateTime.Now;
                            petition.RequiredVoters = hall.RequiredVotes;
                            petition.PetitionCost = hall.Cost;

                            petition.RecordOwnerImage = ImageService.SaveImageInFolder(petition.RecordOwnerImage, Guid.NewGuid().ToString(), "Petition");

                            Notification petitionerNotification = new Notification()
                            {
                                RecieverID = petition.PetitionerID,
                                CreatedBy = default,
                                DateCreated = petition.DateCreated,
                                ID = Guid.NewGuid(),
                                IsActive = true,
                                IsRead = false,
                                IsSpyder = true,
                                Message = $"You have successfully launched a {hall.Name} petition in regards of {petition.RecordOwnerName}. Cost: {hall.Cost}"
                            };

                            await _unitOfWork.Notifications.Create(petitionerNotification);
                            await _unitOfWork.Petitions.Create(petition);

                            if (await _unitOfWork.Commit() >= 1)
                            {
                                responseMessage.Data = petition;
                                responseMessage.StatusCode = 200;
                                responseMessage.Message = "Petition Successful!";
                                return responseMessage;
                            }
                            else
                            {
                                responseMessage.StatusCode = 201;
                                responseMessage.Message = "Petition not successful!";
                                return responseMessage;
                            }
                        }
                        else
                        {
                            responseMessage.StatusCode = res.StatusCode;
                            responseMessage.Message = res.Message;
                            return responseMessage;
                        }
                       
                    }
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Failed. Try Again!";
            }

            return responseMessage;
        }

        public async Task Approve(Guid petitionID)
        {
            var entity = await GetPetitionByID(petitionID);
            var votes = await _unitOfWork.Votes.GetUpVotesByItemID(petitionID);
            if (!entity.IsApproved)
            {
                try
                {
                    if (entity.RequiredVoters == votes.Count())
                    {
                        entity.DateModified = DateTime.Now;
                        entity.IsApproved = true;
                        await Update(entity);
                    }
                }
                catch (Exception)
                {
                }
            }
            else
            {
                //To Do: If approved and currrent vote is less than half of required, terminate on deadline
            }

        }
        public async Task<int> Update(Petition petition)
        {
            petition.DateModified = DateTime.Now;
            _unitOfWork.Petitions.Update(petition);
            return await _unitOfWork.Commit();
        }

        public async Task<int> Delete(Guid id)
        {
            var entity = await GetPetitionByID(id);
            _unitOfWork.Petitions.Delete(entity);
            return await _unitOfWork.Commit();
        }
    }
}
