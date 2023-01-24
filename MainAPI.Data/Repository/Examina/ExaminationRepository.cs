using MainAPI.Data.Interface.Examina;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Examina
{
    public class ExaminationRepository : GenericRepository<Exam>, IExam
    {
        public ExaminationRepository(MainAPIContext db) : base(db) { }

        public async Task<IEnumerable<Exam>> GetExamsByNodeID(Guid NodeID)
        {
            return await GetBy(u => u.NodeID == NodeID);
        }
    }
}
