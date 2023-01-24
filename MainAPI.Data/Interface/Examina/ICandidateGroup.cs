using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Examina
{
    public interface ICandidateGroup : IGeneric<CandidateGroup>
    {
        Task<IEnumerable<CandidateGroup>> GetCandidateGroupsByCandidateID(Guid CandidateID);
        Task<IEnumerable<CandidateGroup>> GetCandidateGroupsByGroupID(Guid groupID);
        Task<IEnumerable<CandidateGroup>> GetCandidateGroupsByNodeID(Guid nodeID);
    }
}
