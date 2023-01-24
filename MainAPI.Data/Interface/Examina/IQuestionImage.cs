using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Examina
{
    public interface IQuestionImage : IGeneric<QuestionImage>
    {
        Task<IEnumerable<QuestionImage>> GetQuestionImagesByQuestionID(Guid QuestionID);
        Task<IEnumerable<QuestionImage>> GetQuestionImageObjectsByNodeID(Guid NodeID);
    }
}
