using MainAPI.Data.Interface.Examina;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Examina
{
    public class QuestionRepository : GenericRepository<Question>, IQuestion
    {
        public QuestionRepository(MainAPIContext db) : base(db) { }
        public async Task<IEnumerable<Question>> GetQuestionsByExamID(Guid ExamID) => await GetBy(u => u.ExamID == ExamID);

        public async Task<IEnumerable<Question>> GetQuestionsByNodeID(Guid NodeID)
        {
            return await GetBy(u => u.NodeID == NodeID);
        }
    }
}
