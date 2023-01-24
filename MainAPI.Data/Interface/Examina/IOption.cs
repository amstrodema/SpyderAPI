using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Examina
{
    public interface IOption : IGeneric<Option>
    {
        Task<IEnumerable<Option>> GetOptionsByNodeID(Guid NodeID);
        Task<IEnumerable<Option>> GetOptionsByQuestionID(Guid QuestionID);
    }
}
