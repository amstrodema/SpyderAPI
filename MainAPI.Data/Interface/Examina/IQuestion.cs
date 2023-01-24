using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Examina
{
    public interface IQuestion : IGeneric<Question>
    {
        Task<IEnumerable<Question>> GetQuestionsByExamID(Guid ExamID);
        Task<IEnumerable<Question>> GetQuestionsByNodeID(Guid NodeID);
    }
}
