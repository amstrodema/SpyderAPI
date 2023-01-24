using MainAPI.Data.Interface.Examina;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Examina
{
    public class QuestionImageRepository : GenericRepository<QuestionImage>, IQuestionImage
    {
        public QuestionImageRepository(MainAPIContext db) : base(db) { }
        public async Task<IEnumerable<QuestionImage>> GetQuestionImagesByQuestionID(Guid QuestionID)
        {
            return from images in await GetBy(u => u.QuestionID == QuestionID)
                   select images;
        }

        public async Task<IEnumerable<QuestionImage>> GetQuestionImageObjectsByNodeID(Guid NodeID)
        {
            return await GetBy(u => u.NodeID == NodeID);
        }
    }
}
