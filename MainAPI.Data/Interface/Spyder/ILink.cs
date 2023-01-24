using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainAPI.Models.Spyder;

namespace MainAPI.Data.Interface.Spyder
{
    public interface ILink: IGeneric<Link>
    {
        Task<IEnumerable<Link>> GetItemLinks(Guid itemID);
    }
}
