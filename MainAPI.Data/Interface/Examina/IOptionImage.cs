using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Examina
{
    public interface IOptionImage : IGeneric<OptionImage>
    {
        Task<IEnumerable<OptionImage>> GetOptionImageByOptionID(Guid optionID);
        Task<IEnumerable<OptionImage>> GetOptionImageObjectsByNodeID(Guid NodeID);
    }
}
