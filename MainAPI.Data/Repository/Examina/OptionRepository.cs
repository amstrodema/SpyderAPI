using MainAPI.Data.Interface.Examina;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Examina
{
    public class OptionRepository : GenericRepository<Option>, IOption
    {
        public OptionRepository(MainAPIContext db) : base(db) { }

        public async Task<IEnumerable<Option>> GetOptionsByNodeID(Guid NodeID)
        {
            return await GetBy(u => u.NodeID == NodeID);
        }
        public async Task<IEnumerable<Option>> GetOptionsByQuestionID(Guid QuestionID)
        {
            return await GetBy(u => u.QuestionID == QuestionID);
        }

    }
}
