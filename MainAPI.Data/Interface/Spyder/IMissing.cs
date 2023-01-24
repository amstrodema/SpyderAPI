using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainAPI.Models.Spyder;

namespace MainAPI.Data.Interface.Spyder
{
    public interface IMissing:IGeneric<Missing>
    {
        Task<IEnumerable<Missing>> GetMissingByItemTypeID(Guid itemTypeID);
    }
}
