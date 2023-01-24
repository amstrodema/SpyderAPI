using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Spyder;
using MainAPI.Models.ViewModel;
using MainAPI.Models.ViewModel.Spyder;
using MainAPI.Models.ViewModel.Spyder.Petition;
using MainAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder
{
    public class GenericBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ParamsBusiness paramsBusiness;
        private readonly PetitionBusiness petitionBusiness;
        private readonly MarriageBusiness marriageBusiness;
        private readonly MissingBusiness missingBusiness;
        private readonly ConfessionBusiness confessionBusiness;
        private readonly DeathBusiness deathBusiness;
        private readonly HallBusiness hallBusiness;
        private readonly VoteBusiness voteBusiness;

        public GenericBusiness(IUnitOfWork unitOfWork, ParamsBusiness paramsBusiness, PetitionBusiness petitionBusiness, MarriageBusiness marriageBusiness,
            MissingBusiness missingBusiness, ConfessionBusiness confessionBusiness, DeathBusiness deathBusiness, HallBusiness hallBusiness, VoteBusiness voteBusiness)
        {
            _unitOfWork = unitOfWork;
            this.paramsBusiness = paramsBusiness;
            this.petitionBusiness = petitionBusiness;
            this.marriageBusiness = marriageBusiness;
            this.missingBusiness = missingBusiness;
            this.confessionBusiness = confessionBusiness;
            this.deathBusiness = deathBusiness;
            this.hallBusiness = hallBusiness;
            this.voteBusiness = voteBusiness;
        }

        public async Task<ResponseMessage<TrendingVM>> GetTrending(RequestObject<string> requestObject)
        {
            TrendingVM trendingVM = new TrendingVM();
            var marriages = await marriageBusiness.GetMarriages();
            var deaths = await deathBusiness.GetDeaths();


            //trendingVM.NewRegistration = (await _unitOfWork.Users.GetAll()).Count();
            //trendingVM.Earnings = (from transaction in await _unitOfWork.Transactions.GetAll()
            //                       where !transaction.IsOfficial && transaction.TransactionType == "Credit"
            //                       select transaction).Sum(p => p.Amount);

            //trendingVM.ReferralBonus = (from wallet in await _unitOfWork.Wallets.GetAll()
            //                            where !wallet.IsOfficial
            //                            select wallet).Sum(p => p.ActivationCost * p.LegOnePercentage + p.ActivationCost * p.LegTwoPercentage);



            try
            {
                var petitionz = await petitionBusiness.GetPetitions();
                var hallRecords = (from petition in petitionz
                                   where petition.IsApproved
                                  select petition).ToList();
                var halls = await hallBusiness.GetHalls();

                Guid id = halls.FirstOrDefault(p => p.HallCode == "heros").ID;
                trendingVM.HerosRecord = hallRecords == null ? 0 : hallRecords.Where(p => p.HallID == id).Count();

                id = halls.FirstOrDefault(p => p.HallCode == "legends").ID;
                trendingVM.LegendRecord = hallRecords == null ? 0 : hallRecords.Where(p => p.HallID == id).Count();

                id = halls.FirstOrDefault(p => p.HallCode == "hall-of-shame").ID;
                trendingVM.HOSRecord = hallRecords == null ? 0 : hallRecords.Where(p => p.HallID == id).Count();

                id = halls.FirstOrDefault(p => p.HallCode == "fallen-heros").ID;
                trendingVM.FallHeroRecord = hallRecords == null ? 0 : hallRecords.Where(p => p.HallID == id).Count();

                id = halls.FirstOrDefault(p => p.HallCode == "hall-of-fame").ID;
                trendingVM.HOFRecord = hallRecords == null ? 0 : hallRecords.Where(p => p.HallID == id).Count();
            }
            catch (Exception)
            {
            }


            var petitions = (await petitionBusiness.GetPetitionVMs(requestObject)).ToList();

            trendingVM.PetitionVMs = petitions.Take(5);
            trendingVM.Marriages = marriages.Take(5);
            trendingVM.Deaths = deaths.Take(5);


            trendingVM.Missing = (await missingBusiness.GetMissingByItemTypeID(new RequestObject<int>() { Data = 1, ItemID = "All" })).ToList();
            trendingVM.MissingRecord = trendingVM.Missing.Count();
            trendingVM.Missing = trendingVM.Missing.Take(5);

            trendingVM.Stolen = (await missingBusiness.GetMissingByItemTypeID(new RequestObject<int>() { Data = 2, ItemID = "All" })).ToList();
            trendingVM.StolenRecord = trendingVM.Stolen.Count();
            trendingVM.Stolen = trendingVM.Stolen.Take(5);

            var found = (await missingBusiness.GetMissingByItemTypeID(new RequestObject<int>() { Data = 3, ItemID = "All" })).ToList();
            trendingVM.FoundRecord = found.Count();

            trendingVM.ConfessionVMs = (await confessionBusiness.GetConfessionHeaders(new RequestObject<int>() { Data = 1, ItemID = "All" })).ToList();
            trendingVM.ConfessionRecord = trendingVM.ConfessionVMs.Count();
            trendingVM.ConfessionVMs = trendingVM.ConfessionVMs.Take(5);

            var scammers = (await confessionBusiness.GetConfessionHeaders(new RequestObject<int>() { Data = 2, ItemID = "All" })).ToList();
            trendingVM.ScammersRecord = scammers.Count();

            var whistleBlower = (await confessionBusiness.GetConfessionHeaders(new RequestObject<int>() { Data = 3, ItemID = "All" })).ToList();
            trendingVM.WhistleBlowerRecord = whistleBlower.Count();

            trendingVM.MarriedRecord = marriages.Count();
            trendingVM.DeathRecord = deaths.Count();

            ResponseMessage<TrendingVM> responseMessage = new ResponseMessage<TrendingVM>();
            responseMessage.Data = trendingVM;
            return responseMessage;
        }

        private List<SearchVM> SearchPetition(List<PetitionHybrid> petitions, string searchString)
        {
            List<SearchVM> searchVMs = new List<SearchVM>();

            try
            {
                searchVMs.AddRange((from record in petitions.Where(o => Checker(o.Brief, searchString) || Checker(o.HallName, searchString) ||
                  Checker(o.CreatedBy, searchString) || Checker(o.DateCreated, searchString) ||
                   Checker(o.NameOfPetitioned, searchString) || Checker(o.PetitionCountry, searchString) ||
                     Checker(o.Petitioner, searchString) || Checker(o.RecordOwnerName, searchString) || Checker(o.Type, searchString) ||
                     Checker(o.RecordOwnerStory, searchString)
                   )
                                   select new SearchVM()
                                   {
                                       ID = record.ID,
                                       Image = ImageService.GetImageFromFolder(record.RecordOwnerImage, "Petition"),
                                       Title = record.IsApproved ? $"{record.HallName}: {record.RecordOwnerName}" : $"Petition: {record.RecordOwnerName}",
                                       Brief = ShortestText(record.RecordOwnerStory),
                                       ValueType = record.IsApproved ? "Hall" : "Petition",
                                       DateCreated = record.DateCreated
                                   }).ToList());
            }
            catch (Exception)
            {

            }



            return searchVMs;
        }

        private List<SearchVM> SearchConfession(List<ConfessionVM> records, string searchString)
        {
            List<SearchVM> searchVMs = new List<SearchVM>();

            try
            {
                searchVMs.AddRange(from record in records.Where(o => Checker(o.Title, searchString) || Checker(o.Details, searchString) ||
                  Checker(o.CreatedBy, searchString) || Checker(o.Date, searchString) ||
                   Checker(o.CountryName, searchString)
                   )
                                   select new SearchVM()
                                   {
                                       ID = record.ID,
                                       Image = ImageService.GetImageFromFolder(record.Image, "Confession"),
                                       Title = record.Title,
                                       Brief = ShortestText(record.Details),
                                       ValueType = ResolveConfession(record.DialogueTypeNo),
                                       DateCreated = DateTime.Parse(record.Date)
                                   });
            }
            catch (Exception)
            {

            }

            return searchVMs;
        }

        private List<SearchVM> SearchDeath(List<DeathVM> records, string searchString)
        {
            List<SearchVM> searchVMs = new List<SearchVM>();


            try
            {
                searchVMs.AddRange(
                    (from record in records.Where(o => Checker(o.Name, searchString) || Checker(o.Address, searchString) ||
                  Checker(o.Author, searchString) || Checker(o.DateCreated, searchString) || Checker(o.BirthDate, searchString) || Checker(o.DeathDate, searchString) ||
                   Checker(o.CountryName, searchString) || Checker(o.CauseOfDeath, searchString) ||
                     Checker(o.City, searchString) || Checker(o.DeathCertNo, searchString) || Checker(o.DetailsOfPerson, searchString)
                     || Checker(o.PostalCode, searchString) || Checker(o.State, searchString)|| Checker(o.Type, searchString)
                   )
                     select new SearchVM()
                     {
                         ID = record.ID,
                         Image = ImageService.GetImageFromFolder(record.Image, "Death"),
                         Title = "Death: " + record.Name,
                         Brief = record.CauseOfDeath.Length > 30 ? ShortestText(record.CauseOfDeath) : ShortestText(record.DetailsOfPerson),
                         ValueType = "Death",
                         DateCreated = record.DateCreated
                     }).ToList());
            }
            catch (Exception)
            {

            }




            return searchVMs;
        }

        private bool Checker(dynamic value, string searchString)
        {
            try
            {
                string val = value;
                return val.ToLower().Contains(searchString.ToLower());
            }
            catch (Exception)
            {
                try
                {
                    DateTime date = value;
                    return date.ToString("f").ToLower().Contains(searchString.ToLower());
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        private List<SearchVM> SearchMissing(List<MissingHybrid> missings, string searchItem)
        {
            List<SearchVM> searchVMs = new List<SearchVM>();

            //var splitted = searchItem.Split(" ");

            searchVMs = (from record in missings.Where(o => Checker(o.Title, searchItem) || Checker(o.CountryName, searchItem) ||
                      Checker(o.DateCreated, searchItem) || Checker(o.Desc, searchItem) ||
                        Checker(o.FullInfo, searchItem) || Checker(o.ItemTypeName, searchItem) ||
                        Checker(o.LastSeen, searchItem) || Checker(o.Author, searchItem)
                       )
                         select new SearchVM()
                         {
                             ID = record.ID,
                             Image = ImageService.GetImageFromFolder(record.Image, "Missing"),
                             Title = $"{record.ItemTypeName}: " + record.Title,
                             Brief = ShortestText(record.Desc),
                             ValueType = record.ItemTypeName
                         }).ToList();


            return searchVMs;
        }
        private List<SearchVM> SearchMarriage(List<MarriageHybrid> marriages, string searchItem)
        {
            List<SearchVM> searchVMs = new List<SearchVM>();

            //var splitted = searchItem.Split(" ");

            searchVMs = (from record in marriages.Where(o => Checker(o.Author, searchItem) || Checker(o.CountryName, searchItem) || Checker(o.WeddingDate, searchItem) ||
                      Checker(o.BrideFName, searchItem) || Checker(o.BrideLName, searchItem) || Checker(o.CertNo, searchItem) || Checker(o.Type, searchItem) ||
                        Checker(o.City, searchItem) || Checker(o.CountryName, searchItem) || Checker(o.Status, searchItem) || Checker(o.Toast, searchItem) ||
                        Checker(o.DateCreated, searchItem) || Checker(o.GroomFName, searchItem) || Checker(o.GroomLName, searchItem)
                       )
                         select new SearchVM()
                         {
                             ID = record.ID,
                             Image = ImageService.GetImageFromFolder(record.Image, "Marriage"),
                             Title = $"{record.BrideFName} weds {record.GroomFName}",
                             Brief = ShortestText(record.Toast),
                             ValueType = "Marriage"
                         }).ToList();

            return searchVMs;
        }

        private async Task<Tuple<List<SearchVM>, List<SearchVM>>> SplitSearch(string searchString, List<SearchVM> searchVMs, List<MarriageHybrid> marriages, List<MissingHybrid> missings, List<ConfessionVM> confessions, List<PetitionHybrid> petitions, List<DeathVM> deaths)
        {
            List<SearchVM> searchAlternateVMs = new List<SearchVM>();
            List<SearchVM> newSearchVMs = new List<SearchVM>();

            var features = from feature in await _unitOfWork.Features.GetAll()
                           join featureType in await _unitOfWork.FeatureTypes.GetAll() on feature.FeatureTypeID equals featureType.ID
                           join featureGroup in await _unitOfWork.FeatureGroups.GetAll() on feature.FeatureGroupID equals featureGroup.ID
                           select new FeatureHybridVM()
                           {
                               Value = feature.Value,
                               FeatureTypeID = featureType.ID,
                               ID = feature.ID,
                               FeatureTypeName = featureType.Name,
                               FeatureGroupID = featureGroup.ID,
                               FeatureGroupName = featureGroup.Name,
                               ItemID = feature.ItemID
                           };

            var featuresItemIDs = features.Where(p => Checker(p.Value, searchString) || Checker(p.FeatureGroupName, searchString) || Checker(p.FeatureTypeName, searchString)).Select(o => o.ItemID).ToList();

            foreach (var searchItem in searchString.Split(" "))
            {
                if (searchItem == "")
                {
                    continue;
                }
                searchAlternateVMs.AddRange(from record in marriages.Where(o => (
                                             Checker(o.Author, searchItem) || Checker(o.CountryName, searchItem) || Checker(o.WeddingDate, searchItem) ||
                      Checker(o.BrideFName, searchItem) || Checker(o.BrideLName, searchItem) || Checker(o.CertNo, searchItem) || Checker(o.Type, searchItem) ||
                        Checker(o.City, searchItem) || Checker(o.CountryName, searchItem) || Checker(o.Status, searchItem) || Checker(o.Toast, searchItem) ||
                        Checker(o.DateCreated, searchItem) || Checker(o.GroomFName, searchItem) || Checker(o.GroomLName, searchItem)
                            ) && searchVMs.FirstOrDefault(p => p.ID == o.ID) == default
                             && searchAlternateVMs.FirstOrDefault(p => p.ID == o.ID) == default
                           )
                                            select new SearchVM()
                                            {
                                                ID = record.ID,
                                                Image = ImageService.GetImageFromFolder(record.Image, "Marriage"),
                                                Title = $"{record.BrideFName} weds {record.GroomFName}",
                                                Brief = ShortestText(record.Toast),
                                                ValueType = "Marriage"
                                            });


                searchAlternateVMs.AddRange(from record in missings.Where(o => (Checker(o.Title, searchItem) || Checker(o.CountryName, searchItem) ||
                         Checker(o.DateCreated, searchItem) || Checker(o.Desc, searchItem) ||
                          Checker(o.FullInfo, searchItem) || Checker(o.ItemTypeName, searchItem) ||
                          Checker(o.LastSeen, searchItem) || Checker(o.Author, searchItem)) && searchVMs.FirstOrDefault(p => p.ID == o.ID) == default
                           && searchAlternateVMs.FirstOrDefault(p => p.ID == o.ID) == default
                         )
                                            select new SearchVM()
                                            {
                                                ID = record.ID,
                                                Image = ImageService.GetImageFromFolder(record.Image, "Missing"),
                                                Title = $"{record.ItemTypeName}: " + record.Title,
                                                Brief = ShortestText(record.Desc),
                                                ValueType = record.ItemTypeName
                                            });


                searchAlternateVMs.AddRange(from record in deaths.Where(o => (Checker(o.Name, searchItem) || Checker(o.Address, searchItem) ||
             Checker(o.Author, searchItem) || Checker(o.DateCreated, searchItem) || Checker(o.BirthDate, searchItem) || Checker(o.DeathDate, searchItem) ||
              Checker(o.CountryName, searchItem) || Checker(o.CauseOfDeath, searchItem) || Checker(o.Type, searchString) ||
                Checker(o.City, searchItem) || Checker(o.DeathCertNo, searchItem) || Checker(o.DetailsOfPerson, searchItem)
                || Checker(o.PostalCode, searchItem) || Checker(o.State, searchItem)) && searchVMs.FirstOrDefault(p => p.ID == o.ID) == default
              && searchAlternateVMs.FirstOrDefault(p => p.ID == o.ID) == default
            )
                                            select new SearchVM()
                                            {
                                                ID = record.ID,
                                                Image = ImageService.GetImageFromFolder(record.Image, "Death"),
                                                Title = "Death: " + record.Name,
                                                Brief = record.CauseOfDeath.Length > 30 ? ShortestText(record.CauseOfDeath) : ShortestText(record.DetailsOfPerson),
                                                ValueType = "Death",
                                                DateCreated = record.DateCreated
                                            });



                searchAlternateVMs.AddRange(from record in confessions.Where(o => (Checker(o.Title, searchItem) || Checker(o.Details, searchItem) ||
             Checker(o.CreatedBy, searchItem) || Checker(o.Date, searchItem) ||
              Checker(o.CountryName, searchItem)) && searchVMs.FirstOrDefault(p => p.ID == o.ID) == default
              && searchAlternateVMs.FirstOrDefault(p => p.ID == o.ID) == default
            )
                                            select new SearchVM()
                                            {
                                                ID = record.ID,
                                                Image = ImageService.GetImageFromFolder(record.Image, "Confession"),
                                                Title = record.Title,
                                                Brief = ShortestText(record.Details),
                                                ValueType = ResolveConfession(record.DialogueTypeNo),
                                                DateCreated = DateTime.Parse(record.Date)
                                            });


                searchAlternateVMs.AddRange(from record in petitions.Where(o => (Checker(o.Brief, searchItem) || Checker(o.HallName, searchItem) ||
            Checker(o.CreatedBy, searchItem) || Checker(o.DateCreated, searchItem) ||
             Checker(o.NameOfPetitioned, searchItem) || Checker(o.PetitionCountry, searchItem) ||
               Checker(o.Petitioner, searchItem) || Checker(o.RecordOwnerName, searchItem) || Checker(o.Type, searchString) ||
               Checker(o.RecordOwnerStory, searchItem)) && searchVMs.FirstOrDefault(p => p.ID == o.ID) == default
               && searchAlternateVMs.FirstOrDefault(p => p.ID == o.ID) == default
             )
                                            select new SearchVM()
                                            {
                                                ID = record.ID,
                                                Image = ImageService.GetImageFromFolder(record.RecordOwnerImage, "Petition"),
                                                Title = record.IsApproved ? $"{record.HallName}: {record.RecordOwnerName}" : $"Petition: {record.RecordOwnerName}",
                                                Brief = ShortestText(record.RecordOwnerStory),
                                                ValueType = record.IsApproved ? "Hall" : "Petition",
                                                DateCreated = record.DateCreated
                                            });



                featuresItemIDs.AddRange(features.Where(a => (Checker(a.Value, searchItem) || Checker(a.FeatureGroupName, searchItem)
                || Checker(a.FeatureTypeName, searchItem)) && featuresItemIDs.FirstOrDefault(p => p == a.ID) == default)
                    .Select(o => o.ItemID).ToList());
            }


            var grouped = featuresItemIDs.GroupBy(p => p);

            newSearchVMs.AddRange(from id in grouped.Select(p => p.Key).Where(o => searchVMs.FirstOrDefault(p => p.ID == o) == default
                             && searchAlternateVMs.FirstOrDefault(p => p.ID == o) == default )
                               join record in missings on id equals record.ID
                               select new SearchVM()
                               {
                                   ID = record.ID,
                                   Image = ImageService.GetImageFromFolder(record.Image, "Missing"),
                                   Title = $"{record.ItemTypeName}: " + record.Title,
                                   Brief = ShortestText(record.Desc),
                                   ValueType = record.ItemTypeName
                               });

            newSearchVMs.AddRange(from id in grouped.Select(p => p.Key).Where(o => searchVMs.FirstOrDefault(p => p.ID == o) == default
                         && searchAlternateVMs.FirstOrDefault(p => p.ID == o) == default)
                               join record in marriages on id equals record.ID
                               select new SearchVM()
                               {
                                   ID = record.ID,
                                   Image = ImageService.GetImageFromFolder(record.Image, "Marriage"),
                                   Title = $"{record.BrideFName} weds {record.GroomFName}",
                                   Brief = ShortestText(record.Toast),
                                   ValueType = "Marriage"
                               });


            return Tuple.Create(newSearchVMs, searchAlternateVMs);
        }

        private async Task<List<PetitionHybrid>> GetPetitionData(string countryId)
        {
            var pet = await _unitOfWork.Petitions.GetAll();

            try
            {
                Guid countryID = Guid.Parse(countryId);
                pet = pet.Where(m => m.PetitionCountryID == countryID).ToList();
            }
            catch (Exception)
            {

            }

            var petitions = from record in pet
                            join hall in await _unitOfWork.Halls.GetAll() on record.HallID equals hall.ID
                            join country in await _unitOfWork.Countries.GetAll() on record.PetitionCountryID equals country.ID
                            join user in await _unitOfWork.Users.GetAll() on record.PetitionerID equals user.ID
                            select new PetitionHybrid()
                            {
                                ID = record.ID,
                                Brief = record.Brief,
                                PetitionCountry = country.Name,
                                CreatedBy = record.IsAnonymous ? "Anonymous" : $"{user.Username} {user.FName} {user.LName}",
                                DateCreated = record.DateCreated,
                                HallName = hall.Name,
                                NameOfPetitioned = record.NameOfPetitioned,
                                Petitioner = record.IsAnonymous ? "Anonymous" : $"{user.Username} {user.FName}",
                                RecordOwnerName = record.RecordOwnerName,
                                RecordOwnerStory = record.RecordOwnerStory,
                                PetitionCountryID = record.PetitionCountryID,
                                RecordOwnerImage = record.RecordOwnerImage,
                                IsApproved = record.IsApproved,
                                Type = record.IsApproved ? "Hall": "Petition"
                            };

            return petitions.ToList();
        }
        private async Task<List<DeathVM>> GetDeathData(string countryId)
        {
            var deat = await _unitOfWork.Deaths.GetAll();

            try
            {
                Guid countryID = Guid.Parse(countryId);
                deat = deat.Where(m => m.CountryID == countryID).ToList();
            }
            catch (Exception)
            {

            }

            var records = from record in deat
                          join country in await _unitOfWork.Countries.GetAll() on record.CountryID equals country.ID
                          join user in await _unitOfWork.Users.GetAll() on record.CreatedBy equals user.ID
                          select new DeathVM()
                          {
                              ID = record.ID,
                              Author = $"{user.Username} {user.FName} {user.LName}",
                              CreatedBy = record.CreatedBy,
                              DateCreated = record.DateCreated,
                              Address = record.Address,
                              BirthDate = record.BirthDate,
                              Image = record.Image,
                              Name = record.Name,
                              CountryID = record.CountryID,
                              CountryName = country.Name,
                              CauseOfDeath = record.CauseOfDeath,
                              City = record.City,
                              DeathCertNo = record.DeathCertNo,
                              DeathDate = record.DeathDate,
                              DetailsOfPerson = record.DetailsOfPerson,
                              PostalCode = record.PostalCode,
                              State = record.State,
                              Type = "Death"
                          };
            return records.ToList();
        }
        private async Task<List<ConfessionVM>> GetConfessionData(string countryId)
        {
            var conf = await _unitOfWork.Confessions.GetAll();
            try
            {
                Guid countryID = Guid.Parse(countryId);
                conf = conf.Where(m => m.CountryID == countryID).ToList();
            }
            catch (Exception)
            {

            }

            var records = from record in conf
                          join country in await _unitOfWork.Countries.GetAll() on record.CountryID equals country.ID
                          join user in await _unitOfWork.Users.GetAll() on record.CreatedBy equals user.ID
                          select new ConfessionVM()
                          {
                              ID = record.ID,
                              CreatedBy = record.IsAnonymous ? "Anonymous" : $"{user.Username} {user.FName} {user.LName}",
                              CreatedByID = record.CreatedBy,
                              Date = record.DateCreated.ToString("f"),
                              Details = record.Details,
                              Image = record.Image,
                              IsAnonymous = record.IsAnonymous,
                              Title = record.Title,
                              CountryID = record.CountryID,
                              CountryName = country.Name,
                              DialogueTypeNo = record.DialogueTypeNo
                          };
            return records.ToList();
        }
        private async Task<List<MissingHybrid>> GetMissingData(string countryId)
        {
            var misn = await _unitOfWork.Missings.GetAll();
            try
            {
                Guid countryID = Guid.Parse(countryId);
                misn = misn.Where(m => m.CountryID == countryID).ToList();
            }
            catch (Exception)
            {

            }

            var missings = from record in misn
                           join country in await _unitOfWork.Countries.GetAll() on record.CountryID equals country.ID
                           join user in await _unitOfWork.Users.GetAll() on record.CreatedBy equals user.ID
                           select new MissingHybrid()
                           {
                               ID = record.ID,
                               Desc = record.Desc,
                               CountryName = country.Name,
                               Author = $"{user.Username} {user.FName} {user.LName}",
                               DateCreated = record.DateCreated,
                               FullInfo = record.FullInfo,
                               Title = record.Title,
                               LastSeen = record.LastSeen,
                               Image = record.Image,
                               ItemTypeName = ResolveMissing(record.AwarenessTypeNo)
                           };
            return missings.ToList();
        }
        private async Task<List<MarriageHybrid>> GetMarriageData(string countryId)
        {
            var marig = await _unitOfWork.Marriages.GetAll();
            try
            {
                Guid countryID = Guid.Parse(countryId);
                marig = marig.Where(m => m.CountryID == countryID).ToList();
            }
            catch (Exception)
            {

            }

            var marriages = from record in marig
                            join country in await _unitOfWork.Countries.GetAll() on record.CountryID equals country.ID
                            join user in await _unitOfWork.Users.GetAll() on record.CreatedBy equals user.ID
                            select new MarriageHybrid()
                            {
                                ID = record.ID,
                                BrideFName = record.BrideFName,
                                CountryName = country.Name,
                                Author = $"{user.Username} {user.FName} {user.LName}",
                                DateCreated = record.DateCreated,
                                BrideLName = record.BrideLName,
                                CertNo = record.CertNo,
                                City = record.City,
                                Image = record.Image,
                                CountryID = record.CountryID,
                                GroomFName = record.GroomFName,
                                GroomLName = record.GroomLName,
                                Status = record.Status,
                                Toast = record.Toast,
                                Type = record.Type,
                                WeddingDate = record.DateCreated.ToString("f")
                            };
            return marriages.ToList();
        }
        public async Task<ResponseMessage<IEnumerable<SearchVM>>> GetSearchResult(RequestObject<string> requestObject)
        {

            ResponseMessage<IEnumerable<SearchVM>> responseMessage = new ResponseMessage<IEnumerable<SearchVM>>();
            List<SearchVM> searchVMs = new List<SearchVM>();
            List<SearchVM> searchAlternativeVMs = new List<SearchVM>();
            try
            {
                string searchString = requestObject.Data;

                if (string.IsNullOrWhiteSpace(searchString))
                {
                    return default;
                }

                var petitionData = await GetPetitionData(requestObject.CountryID);
                var petitionSearch = SearchPetition(petitionData, searchString);

                var deathData = await GetDeathData(requestObject.CountryID);
                var deathSearch = SearchDeath(deathData, searchString);

                var confessionData = await GetConfessionData(requestObject.CountryID);
                var confessionSearch = SearchConfession(confessionData, searchString);

                var missingData = await GetMissingData(requestObject.CountryID);
                var missingSearch = SearchMissing(missingData, searchString);

                var marriageData = await GetMarriageData(requestObject.CountryID);
                var marriageSearch = SearchMarriage(marriageData, searchString);


                try
                {
                    searchVMs.AddRange(petitionSearch);
                    searchVMs.AddRange(deathSearch);
                    searchVMs.AddRange(confessionSearch);
                    searchVMs.AddRange(missingSearch);
                    searchVMs.AddRange(marriageSearch);


                    try
                    {
                        var searchResults = await SplitSearch(searchString, searchVMs, marriageData, missingData, confessionData, petitionData, deathData);

                        searchVMs.AddRange(searchResults.Item1);
                        searchAlternativeVMs.AddRange(searchResults.Item2);
                    }
                    catch (Exception)
                    {
                    }




                    searchVMs = searchVMs.OrderByDescending(p => p.DateCreated).ToList();
                    searchAlternativeVMs = searchAlternativeVMs.OrderByDescending(p => p.DateCreated).ToList();
                    searchVMs.AddRange(searchAlternativeVMs);
                }
                catch (Exception)
                {
                }


                responseMessage.StatusCode = 200;
                responseMessage.Message = "Success";
                responseMessage.Data = searchVMs;
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "No record found";
            }

            return responseMessage;
        }
        public async Task<ResponseMessage<IEnumerable<ProfileVM>>> GetProfileContent(Guid userID)
        {
            ResponseMessage<IEnumerable<ProfileVM>> responseMessage = new ResponseMessage<IEnumerable<ProfileVM>>();
            try
            {
                List<ProfileVM> profileVMs = new List<ProfileVM>();

                var petitions = from record in await _unitOfWork.Petitions.GetPetitionsByPetitionerID(userID)
                                join hall in await _unitOfWork.Halls.GetAll() on record.HallID equals hall.ID
                                join vote in await _unitOfWork.Votes.GetAll() on record.ID equals vote.ItemID into recordVotes
                                join comment in await _unitOfWork.Comments.GetAll() on record.ID equals comment.ItemID into recordComments
                                select new ProfileVM()
                                {
                                    Body = ShortenText(record.Brief),
                                    Title = $"Petition to list {record.RecordOwnerName} into the {hall.Name}",
                                    ID = record.ID,
                                    Date = record.DateCreated,
                                    Type = record.IsApproved ? "Hall" : "Petition",
                                    Likes = recordVotes.Where(p => p.IsLike && p.IsReact).Count(),
                                    Dislikes = recordVotes.Where(p => !p.IsLike && p.IsReact).Count(),
                                    Comments = recordComments.Count()
                                };
                profileVMs = petitions.ToList();

                var marriages = from record in await _unitOfWork.Marriages.GetAll()
                                where record.CreatedBy == userID
                                join like in await _unitOfWork.Likes.GetAll() on record.ID equals like.ItemID into recordLikes
                                join comment in await _unitOfWork.Comments.GetAll() on record.ID equals comment.ItemID into recordComments
                                select new ProfileVM()
                                {
                                    Body = ShortenText(record.Toast),
                                    Title = $"{record.BrideFName} weds {record.GroomFName}",
                                    ID = record.ID,
                                    Date = record.DateCreated,
                                    Type = "Marriage",
                                    Likes = recordLikes.Where(p => p.IsLike && p.IsReact).Count(),
                                    Dislikes = recordLikes.Where(p => !p.IsLike && p.IsReact).Count(),
                                    Comments = recordComments.Count()
                                };
                profileVMs.AddRange(marriages);


                var deaths = from record in await _unitOfWork.Deaths.GetAll()
                             where record.CreatedBy == userID
                             join like in await _unitOfWork.Likes.GetAll() on record.ID equals like.ItemID into recordLikes

                             join comment in await _unitOfWork.Comments.GetAll() on record.ID equals comment.ItemID into recordComments

                             select new ProfileVM()
                             {
                                 Body = ShortenText(record.CauseOfDeath),
                                 Title = $"{record.Name}",
                                 ID = record.ID,
                                 Date = record.DateCreated,
                                 Type = "Death",
                                 Likes = recordLikes.Where(p => p.IsLike && p.IsReact).Count(),
                                 Dislikes = recordLikes.Where(p => !p.IsLike && p.IsReact).Count(),
                                 Comments = recordComments.Count()
                             };
                profileVMs.AddRange(deaths);

                var missing = from record in await _unitOfWork.Missings.GetAll()
                              where record.CreatedBy == userID
                              join like in await _unitOfWork.Likes.GetAll() on record.ID equals like.ItemID into recordLikes

                              join comment in await _unitOfWork.Comments.GetAll() on record.ID equals comment.ItemID into recordComments

                              select new ProfileVM()
                              {
                                  Body = ShortenText(record.Desc),
                                  Title = $"{record.Title}",
                                  ID = record.ID,
                                  Date = record.DateCreated,
                                  Type = ResolveMissing(record.AwarenessTypeNo),
                                  Likes = recordLikes.Where(p => p.IsLike && p.IsReact).Count(),
                                  Dislikes = recordLikes.Where(p => !p.IsLike && p.IsReact).Count(),
                                  Comments = recordComments.Count()
                              };
                profileVMs.AddRange(missing);

                var confession = from record in await _unitOfWork.Confessions.GetAll()
                                 where record.CreatedBy == userID
                                 join like in await _unitOfWork.Likes.GetAll() on record.ID equals like.ItemID into recordLikes

                                 join comment in await _unitOfWork.Comments.GetAll() on record.ID equals comment.ItemID into recordComments

                                 select new ProfileVM()
                                 {
                                     Body = ShortenText(record.Details),
                                     Title = $"{record.Title}",
                                     ID = record.ID,
                                     Date = record.DateCreated,
                                     Type = ResolveConfession(record.DialogueTypeNo),
                                     Likes = recordLikes.Where(p => p.IsLike && p.IsReact).Count(),
                                     Dislikes = recordLikes.Where(p => !p.IsLike && p.IsReact).Count(),
                                     Comments = recordComments.Count()
                                 };
                profileVMs.AddRange(confession);

                profileVMs = profileVMs.OrderByDescending(p => p.Date).ToList();


                responseMessage.StatusCode = 200;
                responseMessage.Message = "Success";
                responseMessage.Data = profileVMs;
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Records not found";
            }
            return responseMessage;
        }

        private string ShortenText(string text)
        {
            try
            {
                if (text.Length <= 100)
                {
                    text = text.Substring(0, 99);
                }
            }
            catch (Exception)
            {
            }

            return text; ;
        }
        private string ShortestText(string text)
        {
            try
            {
                if (text.Length >= 35)
                {
                    text = text.Substring(0, 33);
                }
            }
            catch (Exception)
            {
            }

            return text; ;
        }

        private string ResolveMissing(int typeNO)
        {
            switch (typeNO)
            {
                case 1:
                    return "Missing";
                case 2:
                    return "Stolen";
                case 3:
                    return "Found";
                default:
                    return "";
            }
        }

        private string ResolveConfession(int typeNO)
        {
            switch (typeNO)
            {
                case 1:
                    return "Confession";
                case 2:
                    return "Whistle Blower";
                case 3:
                    return "Scammer";
                default:
                    return "";
            }
        }
        public async Task<ResponseMessage<List<ProfileVM>>> GetProfileContentWithComment(Guid userID)
        {
            ResponseMessage<List<ProfileVM>> responseMessage = new ResponseMessage<List<ProfileVM>>();
            var comments = await _unitOfWork.Comments.GetAll();
            try
            {
                List<ProfileVM> profileVMs = new List<ProfileVM>();

                var petitions = from comm in comments
                                where comm.UserID == userID
                                join record in await _unitOfWork.Petitions.GetAll() on comm.ItemID equals record.ID
                                join hall in await _unitOfWork.Halls.GetAll() on record.HallID equals hall.ID
                                join vote in await _unitOfWork.Votes.GetAll() on record.ID equals vote.ItemID into recordVotes
                                join comment in comments on record.ID equals comment.ItemID into recordComments

                                select new ProfileVM()
                                {
                                    Body = ShortenText(record.Brief),
                                    Title = $"Petition to list {record.RecordOwnerName} into the {hall.Name}",
                                    ID = record.ID,
                                    Date = record.DateCreated,
                                    Type = record.IsApproved ? "Hall" : "Petition",
                                    Likes = recordVotes.Where(p => p.IsLike && p.IsReact).Count(),
                                    Dislikes = recordVotes.Where(p => !p.IsLike && p.IsReact).Count(),
                                    Comments = recordComments.Count(),
                                    Comment = comm,
                                    SortDate = comm.DateCreated
                                };
                profileVMs = petitions.ToList();

                var marriages = from comm in comments
                                where comm.UserID == userID
                                join record in await _unitOfWork.Marriages.GetAll() on comm.ItemID equals record.ID
                                join like in await _unitOfWork.Likes.GetAll() on record.ID equals like.ItemID into recordLikes
                                join comment in await _unitOfWork.Comments.GetAll() on record.ID equals comment.ItemID into recordComments
                                select new ProfileVM()
                                {
                                    Body = ShortenText(record.Toast),
                                    Title = $"{record.BrideFName} weds {record.GroomFName}",
                                    ID = record.ID,
                                    Date = record.DateCreated,
                                    Type = "Marriage",
                                    Likes = recordLikes.Where(p => p.IsLike && p.IsReact).Count(),
                                    Dislikes = recordLikes.Where(p => !p.IsLike && p.IsReact).Count(),
                                    Comments = recordComments.Count(),
                                    Comment = comm,
                                    SortDate = comm.DateCreated
                                };
                profileVMs.AddRange(marriages);


                var deaths = from comm in comments
                             where comm.UserID == userID
                             join record in await _unitOfWork.Deaths.GetAll() on comm.ItemID equals record.ID into records
                             from rec in records
                             join like in await _unitOfWork.Likes.GetAll() on rec.ID equals like.ItemID into recordLikes
                             join comment in await _unitOfWork.Comments.GetAll() on rec.ID equals comment.ItemID into recordComments
                             select new ProfileVM()
                             {
                                 Body = ShortenText(rec.CauseOfDeath),
                                 Title = $"{rec.Name}",
                                 ID = rec.ID,
                                 Date = rec.DateCreated,
                                 Type = "Death",
                                 Likes = recordLikes.Where(p => p.IsLike && p.IsReact).Count(),
                                 Dislikes = recordLikes.Where(p => !p.IsLike && p.IsReact).Count(),
                                 Comments = recordComments.Count(),
                                 Comment = comm,
                                 SortDate = comm.DateCreated
                             };
                profileVMs.AddRange(deaths);

                var missing = from comm in comments
                              where comm.UserID == userID
                              join record in await _unitOfWork.Missings.GetAll() on comm.ItemID equals record.ID
                              join like in await _unitOfWork.Likes.GetAll() on record.ID equals like.ItemID into recordLikes

                              join comment in await _unitOfWork.Comments.GetAll() on record.ID equals comment.ItemID into recordComments

                              select new ProfileVM()
                              {
                                  Body = ShortenText(record.Desc),
                                  Title = $"{record.Title}",
                                  ID = record.ID,
                                  Date = record.DateCreated,
                                  Type = ResolveMissing(record.AwarenessTypeNo),
                                  Likes = recordLikes.Where(p => p.IsLike && p.IsReact).Count(),
                                  Dislikes = recordLikes.Where(p => !p.IsLike && p.IsReact).Count(),
                                  Comments = recordComments.Count(),
                                  Comment = comm,
                                  SortDate = comm.DateCreated
                              };
                profileVMs.AddRange(missing);

                var confession = from comm in comments
                                 where comm.UserID == userID
                                 join record in await _unitOfWork.Confessions.GetAll() on comm.ItemID equals record.ID
                                 join like in await _unitOfWork.Likes.GetAll() on record.ID equals like.ItemID into recordLikes

                                 join comment in await _unitOfWork.Comments.GetAll() on record.ID equals comment.ItemID into recordComments

                                 select new ProfileVM()
                                 {
                                     Body = ShortenText(record.Details),
                                     Title = $"{record.Title}",
                                     ID = record.ID,
                                     Date = record.DateCreated,
                                     Type = ResolveConfession(record.DialogueTypeNo),
                                     Likes = recordLikes.Where(p => p.IsLike && p.IsReact).Count(),
                                     Dislikes = recordLikes.Where(p => !p.IsLike && p.IsReact).Count(),
                                     Comments = recordComments.Count(),
                                     Comment = comm,
                                     SortDate = comm.DateCreated
                                 };
                profileVMs.AddRange(confession);

                profileVMs = profileVMs.OrderByDescending(p => p.SortDate).ToList();


                responseMessage.StatusCode = 200;
                responseMessage.Message = "Success";
                responseMessage.Data = profileVMs;
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Records not found";
            }


            return responseMessage;
        }
        public async Task<ResponseMessage<List<ProfileVM>>> GetProfileContentWithReaction(Guid userID)
        {
            ResponseMessage<List<ProfileVM>> responseMessage = new ResponseMessage<List<ProfileVM>>();
            var votes = await _unitOfWork.Votes.GetAll();
            var likes = await _unitOfWork.Likes.GetAll();
            var comments = await _unitOfWork.Comments.GetAll();

            try
            {
                List<ProfileVM> profileVMs = new List<ProfileVM>();

                var petitions = from comm in votes
                                where comm.UserID == userID
                                join record in await _unitOfWork.Petitions.GetAll() on comm.ItemID equals record.ID
                                join hall in await _unitOfWork.Halls.GetAll() on record.HallID equals hall.ID
                                join vote in await _unitOfWork.Votes.GetAll() on record.ID equals vote.ItemID into recordVotes
                                join comment in comments on record.ID equals comment.ItemID into recordComments

                                select new ProfileVM()
                                {
                                    Body = ShortenText(record.Brief),
                                    Title = $"Petition to list {record.RecordOwnerName} into the {hall.Name}",
                                    ID = record.ID,
                                    Date = record.DateCreated,
                                    Type = record.IsApproved ? "Hall" : "Petition",
                                    Likes = recordVotes.Where(p => p.IsLike && p.IsReact).Count(),
                                    Dislikes = recordVotes.Where(p => !p.IsLike && p.IsReact).Count(),
                                    Comments = recordComments.Count(),
                                    voteType = comm.IsLike
                                };
                profileVMs = petitions.ToList();

                var marriages = from comm in likes
                                where comm.UserID == userID
                                join record in await _unitOfWork.Marriages.GetAll() on comm.ItemID equals record.ID
                                join like in await _unitOfWork.Likes.GetAll() on record.ID equals like.ItemID into recordLikes
                                join comment in await _unitOfWork.Comments.GetAll() on record.ID equals comment.ItemID into recordComments

                                select new ProfileVM()
                                {
                                    Body = ShortenText(record.Toast),
                                    Title = $"{record.BrideFName} weds {record.GroomFName}",
                                    ID = record.ID,
                                    Date = record.DateCreated,
                                    Type = "Marriage",
                                    Likes = recordLikes.Where(p => p.IsLike && p.IsReact).Count(),
                                    Dislikes = recordLikes.Where(p => !p.IsLike && p.IsReact).Count(),
                                    Comments = recordComments.Count(),
                                    voteType = comm.IsLike
                                };
                profileVMs.AddRange(marriages);


                var deaths = from comm in likes
                             where comm.UserID == userID
                             join record in await _unitOfWork.Deaths.GetAll() on comm.ItemID equals record.ID
                             join like in await _unitOfWork.Likes.GetAll() on record.ID equals like.ItemID into recordLikes

                             join comment in await _unitOfWork.Comments.GetAll() on record.ID equals comment.ItemID into recordComments

                             select new ProfileVM()
                             {
                                 Body = ShortenText(record.CauseOfDeath),
                                 Title = $"{record.Name}",
                                 ID = record.ID,
                                 Date = record.DateCreated,
                                 Type = "Death",
                                 Likes = recordLikes.Where(p => p.IsLike && p.IsReact).Count(),
                                 Dislikes = recordLikes.Where(p => !p.IsLike && p.IsReact).Count(),
                                 Comments = recordComments.Count(),
                                 voteType = comm.IsLike
                             };
                profileVMs.AddRange(deaths);

                var missing = from comm in likes
                              where comm.UserID == userID
                              join record in await _unitOfWork.Missings.GetAll() on comm.ItemID equals record.ID
                              join like in await _unitOfWork.Likes.GetAll() on record.ID equals like.ItemID into recordLikes

                              join comment in await _unitOfWork.Comments.GetAll() on record.ID equals comment.ItemID into recordComments

                              select new ProfileVM()
                              {
                                  Body = ShortenText(record.Desc),
                                  Title = $"{record.Title}",
                                  ID = record.ID,
                                  Date = record.DateCreated,
                                  Type = ResolveMissing(record.AwarenessTypeNo),
                                  Likes = recordLikes.Where(p => p.IsLike && p.IsReact).Count(),
                                  Dislikes = recordLikes.Where(p => !p.IsLike && p.IsReact).Count(),
                                  Comments = recordComments.Count(),
                                  voteType = comm.IsLike
                              };
                profileVMs.AddRange(missing);

                var confession = from comm in likes
                                 where comm.UserID == userID
                                 join record in await _unitOfWork.Confessions.GetAll() on comm.ItemID equals record.ID
                                 join like in await _unitOfWork.Likes.GetAll() on record.ID equals like.ItemID into recordLikes

                                 join comment in await _unitOfWork.Comments.GetAll() on record.ID equals comment.ItemID into recordComments

                                 select new ProfileVM()
                                 {
                                     Body = ShortenText(record.Details),
                                     Title = $"{record.Title}",
                                     ID = record.ID,
                                     Date = record.DateCreated,
                                     Type = ResolveConfession(record.DialogueTypeNo),
                                     Likes = recordLikes.Where(p => p.IsLike && p.IsReact).Count(),
                                     Dislikes = recordLikes.Where(p => !p.IsLike && p.IsReact).Count(),
                                     Comments = recordComments.Count(),
                                     voteType = comm.IsLike
                                 };
                profileVMs.AddRange(confession);

                profileVMs = profileVMs.OrderByDescending(p => p.Date).ToList();


                responseMessage.StatusCode = 200;
                responseMessage.Message = "Success";
                responseMessage.Data = profileVMs;
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Records not found";
            }


            return responseMessage;
        }

        //public async Task<Area> GetAreaByID(Guid id) =>
        //          await _unitOfWork.Areas.Find(id);
    }
}
