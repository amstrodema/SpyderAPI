using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class MissingRepository : GenericRepository<Missing>, IMissing
    {
        public MissingRepository(MainAPIContext db) : base(db) { }
        public async Task<IEnumerable<Missing>> GetMissingByItemTypeID(Guid itemTypeID)
        {
            return await GetBy(y => y.ItemTypeID == itemTypeID);
        }
    }
}
