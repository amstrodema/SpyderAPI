using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class FlagReportRepository: GenericRepository<FlagReport>, IFlagReport
    {
        public FlagReportRepository(MainAPIContext db) : base(db) { }

        public async Task<FlagReport> GetReportByUserID_ItemID(Guid userID, Guid ItemID)
        {
            return await GetOneBy(x => x.PetitionerID == userID && x.ItemID == ItemID);
        }

    }
}
