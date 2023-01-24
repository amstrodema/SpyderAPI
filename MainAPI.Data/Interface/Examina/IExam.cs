using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Examina
{
   public interface IExam : IGeneric<Exam>
    {
        Task<IEnumerable<Exam>> GetExamsByNodeID(Guid NodeID);
    }
}
