using MainAPI.Models.Petition.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Spyder
{
    public interface IPetition : IGeneric<Petition>
    {
        Task<IEnumerable<Petition>> GetPetitionsByPetitionerID(Guid petitionerID);
        Task<IEnumerable<Petition>> GetPetitionByPetitionCountryID(Guid PetitionCountryID);
        Task<IEnumerable<Petition>> GetPetitionByHallID(Guid HallID);
    }
}
