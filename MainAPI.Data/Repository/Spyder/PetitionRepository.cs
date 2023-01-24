using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Petition.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
   public class PetitionRepository : GenericRepository<Petition>, IPetition
    {
        public PetitionRepository(MainAPIContext db) : base(db) { }
        public async Task<Petition> GetPetitionByPetitionID(Guid petitionID)
        {
            return await GetOneBy(u => u.ID == petitionID);
        }
        public async Task<IEnumerable<Petition>> GetPetitionsByPetitionerID(Guid petitionerID) => await GetBy(u => u.PetitionerID == petitionerID);
        public async Task<IEnumerable<Petition>> GetPetitionByPetitionCountryID(Guid PetitionCountryID) => await GetBy(u => u.PetitionCountryID == PetitionCountryID);
        public async Task<IEnumerable<Petition>> GetPetitionByHallID(Guid HallID) => await GetBy(u => u.HallID == HallID);
    }
}
