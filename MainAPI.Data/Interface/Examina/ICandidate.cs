using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Examina
{
    public interface ICandidate : IGeneric<Candidate>
    {
        Task<Candidate> GetCandidateByCandidateNo(string candidateNo);
        Task<IEnumerable<Candidate>> GetCandidatesByNodeID(Guid NodeID);
    }
}
