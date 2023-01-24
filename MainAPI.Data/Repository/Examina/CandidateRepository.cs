using MainAPI.Data.Interface.Examina;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Examina
{
    public class CandidateRepository : GenericRepository<Candidate>, ICandidate
    {
        public CandidateRepository(MainAPIContext db) : base(db) { }

        public async Task<Candidate> GetCandidateByCandidateNo(string candidateNo)
        {
            return await GetOneBy(u => u.CandidateNo == candidateNo);
        }

        public async Task<IEnumerable<Candidate>> GetCandidatesByNodeID(Guid NodeID)
        {
            return await GetBy(u => u.NodeID == NodeID);
        }
    }
}
