using MainAPI.Data.Interface.Examina;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Examina
{
    public class InstructorExamRepository: GenericRepository<InstructorExam>, IInstructorExam
    {
        public InstructorExamRepository(MainAPIContext db):base(db) { }

        public async Task<IEnumerable<InstructorExam>> GetInstructorExamsByNodeID(Guid NodeID)
        {
            return await GetBy(u => u.NodeID == NodeID);
        }
        public async Task<IEnumerable<InstructorExam>> GetExamInstructorsByExamID(Guid ExamID)
        {
            return await GetBy(u => u.ExamID == ExamID);
        }
        public async Task<IEnumerable<InstructorExam>> GetExamInstructorsByInstructorID(Guid instructorID)
        {
            return await GetBy(u => u.InstructorID == instructorID);
        }
    }
}
