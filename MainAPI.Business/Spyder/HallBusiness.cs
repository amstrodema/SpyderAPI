using MainAPI.Data.Interface;
using MainAPI.Models;
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
    public class HallBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public HallBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Hall>> GetHalls() =>
         await _unitOfWork.Halls.GetAll();

        public async Task<IEnumerable<Hall>> GetHallsByRoute(string route) =>
         await _unitOfWork.Halls.GetHallsByRoute(route);

        public async Task<Hall> GetHallByID(Guid id) =>
                  await _unitOfWork.Halls.Find(id);
        public async Task<ResponseMessage<Hall>> Create(Hall hall)
        {
            ResponseMessage<Hall> responseMessage = new ResponseMessage<Hall>();
            try
            {
                hall.ID = Guid.NewGuid();
                hall.DateCreated = DateTime.Now;
                hall.IsActive = true;
                await _unitOfWork.Halls.Create(hall);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = hall;
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

        public async Task<int> Update(Hall hall)
        {
            hall.DateModified = DateTime.Now;
            _unitOfWork.Halls.Update(hall);
            return await _unitOfWork.Commit();
        }


        public async Task<IEnumerable<PetitionVM>> GetHallMemberVMs(RequestObject<string> requestObject)
        {
            try
            {
                Guid userID = Guid.Parse(requestObject.UserID);

                return await GetHallMemeberVMs_LoggedIn(requestObject);               
            }
            catch (Exception)
            {
                var petitions = await _unitOfWork.Petitions.GetAll();
                try
                {
                    Guid countryID = Guid.Parse(requestObject.CountryID);
                    petitions = petitions.Where(c => c.PetitionCountryID == countryID).ToList();
                }
                catch (Exception)
                {

                }
                string route = requestObject.Data;
                var x = from petition in petitions
                        where petition.IsApproved
                        join hall in await _unitOfWork.Halls.GetHallsByRoute( route) on petition.HallID equals hall.ID
                        select new PetitionVM()
                        {
                            ID = petition.ID,
                            Brief = petition.Brief,
                            HallName = hall.Name,
                            RecordOwnerImage = ImageService.GetImageFromFolder(petition.RecordOwnerImage, "Petition"),
                            RecordOwnerName = petition.RecordOwnerName
                        };

                return x;
            }
            

           
        }



        public async Task<IEnumerable<PetitionVM>> GetHallMemeberVMs_LoggedIn(RequestObject<string> requestObject)
        {
            var petitions = await _unitOfWork.Petitions.GetAll();
            string route = requestObject.Data;
            Guid userID;
            try
            {
                userID = Guid.Parse(requestObject.UserID);
            }
            catch (Exception)
            {
                return default;
            }

            try
            {
                Guid countryID = Guid.Parse(requestObject.CountryID);
                petitions = petitions.Where(c => c.PetitionCountryID == countryID).ToList();
            }
            catch (Exception)
            {

            }

            var x = from petition in petitions
                    where petition.IsApproved
                    join hall in await _unitOfWork.Halls.GetHallsByRoute(route) on petition.HallID equals hall.ID
                    join personalVote in await _unitOfWork.Votes.GetVotesByUserID(userID) on petition.ID equals personalVote.ItemID into votes
                    from thisVote in votes.DefaultIfEmpty()
                    select new PetitionVM()
                    {
                        ID = petition.ID,
                        Brief = petition.Brief,
                        BtnBgTypeDisLike = thisVote == default ? "" : ResolveBtnBg(thisVote.BtnBgTypeDisLike),
                        BtnBgTypeLike = thisVote == default ? "" : ResolveBtnBg(thisVote.BtnBgTypeLike),
                        HallName = hall.Name,
                        IsLike = thisVote == default ? petition.IsLike : thisVote.IsLike,
                        IsReact = thisVote == default ? petition.IsReact : thisVote.IsReact,
                        RecordOwnerImage = ImageService.GetImageFromFolder(petition.RecordOwnerImage, "Petition"),
                        RecordOwnerName = petition.RecordOwnerName
                    };

            return x;
        }
        public async Task<PetitionVM> GetHallMemberDetailsVM(RequestObject<string> requestObject)
        {
            var petitions = await _unitOfWork.Petitions.GetAll();
            string route = requestObject.Data;
            Guid userID;
            Guid hallRecordID;
            try
            {
                userID = Guid.Parse(requestObject.UserID);
                return await GetHallMemberDetailsVM_Logged(requestObject);
            }
            catch (Exception)
            {
                try
                {
                    hallRecordID = Guid.Parse(requestObject.ItemID);
                }
                catch (Exception)
                {
                    return default;
                }
            }

            try
            {
                Guid countryID = Guid.Parse(requestObject.CountryID);
                petitions = petitions.Where(c => c.PetitionCountryID == countryID).ToList();
            }
            catch (Exception)
            {

            }

            var x = (from petition in await _unitOfWork.Petitions.GetAll()
                     where petition.ID == hallRecordID && petition.IsApproved
                     join hall in await _unitOfWork.Halls.GetAll() on petition.HallID equals hall.ID
                     join vote in await _unitOfWork.Votes.GetAll() on petition.ID equals vote.ItemID into allVotes
                     from allVote in allVotes.DefaultIfEmpty()
                     join comment in await _unitOfWork.Comments.GetAll() on petition.ID equals comment.ItemID into allComments
                     from allComment in allComments.DefaultIfEmpty()
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
                         VotePercentage = (allVotes.Where(p => p.IsReact && p.IsLike).Count() * 1.0f / petition.RequiredVoters) * 100,
                         Comments = allComments,
                         Day = petition.DateCreated.ToString("d"),
                         RecordOwnerStory = petition.RecordOwnerStory,
                         Petitioner = petition.IsAnonymous ? "Anonymous" : user.Username,
                         ClickObject = hall.ClickObject,
                         PetitionerID = petition.IsAnonymous ? default : petition.PetitionerID,
                         TotalVotesRequired = petition.RequiredVoters
                     }).ToList();

            return x[0];
        }
        public async Task<PetitionVM> GetHallMemberDetailsVM_Logged(RequestObject<string> requestObject)
        {
            var petitions = await _unitOfWork.Petitions.GetAll();
            string route = requestObject.Data;
            Guid userID;
            Guid hallRecordID;

            try
            {
                userID = Guid.Parse(requestObject.UserID);

                try
                {
                    hallRecordID = Guid.Parse(requestObject.ItemID);
                }
                catch (Exception)
                {
                    return default;
                }
            }
            catch (Exception)
            {
                return default;
            }

            try
            {
                Guid countryID = Guid.Parse(requestObject.CountryID);
                petitions = petitions.Where(c => c.PetitionCountryID == countryID).ToList();
            }
            catch (Exception)
            {

            }

            var x = (from petition in await _unitOfWork.Petitions.GetAll()
                     where petition.ID == hallRecordID && petition.IsApproved
                     join hall in await _unitOfWork.Halls.GetAll() on petition.HallID equals hall.ID
                     join personalVote in await _unitOfWork.Votes.GetVotesByUserID(userID) on petition.ID equals personalVote.ItemID into votes
                     from thisVote in votes.DefaultIfEmpty()
                     join vote in await _unitOfWork.Votes.GetAll() on petition.ID equals vote.ItemID into allVotes
                     from allVote in allVotes.DefaultIfEmpty()
                     join comment in await _unitOfWork.Comments.GetAll() on petition.ID equals comment.ItemID into allComments
                     from allComment in allComments.DefaultIfEmpty()
                     join user in await _unitOfWork.Users.GetAll() on petition.PetitionerID equals user.ID
                     select new PetitionVM()
                     {
                         ID = petition.ID,
                         Brief = petition.Brief,
                         BtnBgTypeDisLike = thisVote == default ? "" : ResolveBtnBg(thisVote.BtnBgTypeDisLike),
                         BtnBgTypeLike = thisVote == default ? "" : ResolveBtnBg(thisVote.BtnBgTypeLike),
                         HallName = hall.Name,
                         IsLike = thisVote == default ? petition.IsLike : thisVote.IsLike,
                         IsReact = thisVote == default ? petition.IsReact : thisVote.IsReact,
                         RecordOwnerImage = ImageService.GetImageFromFolder(petition.RecordOwnerImage, "Petition"),
                         RecordOwnerName = petition.RecordOwnerName,
                         Time = petition.DateCreated.ToString("t"),
                         TotalDownVotes = allVotes.Where(p => p.IsReact && !p.IsLike).Count(),
                         TotalUpVotes = allVotes.Where(p => p.IsReact && p.IsLike).Count(),
                         VotePercentage = (allVotes.Where(p => p.IsReact && p.IsLike).Count() * 1.0f / petition.RequiredVoters) * 100,
                         Comments = allComments,
                         Day = petition.DateCreated.ToString("d"),
                         RecordOwnerStory = petition.RecordOwnerStory,
                         Petitioner = petition.IsAnonymous ? "Anonymous" : user.Username,
                         ClickObject = hall.ClickObject,
                         PetitionerID = petition.IsAnonymous ? default : petition.PetitionerID,
                         TotalVotesRequired = petition.RequiredVoters
                     }).ToList();

            return x[0];
        }
        private string ResolveBtnBg(string btnType)
        {
            if (btnType == "btn-success" || btnType == "btn-danger")
            {
                return btnType;
            }
            return "";
        }

        public async Task<int> Delete(Guid id)
        {
            var entity = await GetHallByID(id);
            _unitOfWork.Halls.Delete(entity);
            return await _unitOfWork.Commit();
        }
    }
}
