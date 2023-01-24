using MainAPI.Data.Interface.Examina;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Examina
{
    public class CandidateGroupRepository : GenericRepository<CandidateGroup>, ICandidateGroup
    {
        public CandidateGroupRepository(MainAPIContext db) : base(db) { }

        public async Task<IEnumerable<CandidateGroup>> GetCandidateGroupsByCandidateID(Guid candidateID)
        {
            return await GetBy(u => u.CandidateID == candidateID);
        }

        public async Task<IEnumerable<CandidateGroup>> GetCandidateGroupsByGroupID(Guid groupID)
        {
            return await GetBy(u => u.GroupID == groupID);
        }
        public async Task<IEnumerable<CandidateGroup>> GetCandidateGroupsByNodeID(Guid nodeID)
        {
            return await GetBy(u => u.NodeID == nodeID);
        }
    }
}
