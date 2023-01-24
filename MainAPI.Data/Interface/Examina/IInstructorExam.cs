using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Examina
{
   public interface IInstructorExam: IGeneric<InstructorExam>
    {
        Task<IEnumerable<InstructorExam>> GetInstructorExamsByNodeID(Guid NodeID);
        Task<IEnumerable<InstructorExam>> GetExamInstructorsByExamID(Guid ExamID);
        Task<IEnumerable<InstructorExam>> GetExamInstructorsByInstructorID(Guid instructorID);
    }
}
