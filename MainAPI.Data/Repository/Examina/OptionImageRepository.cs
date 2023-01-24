using MainAPI.Data.Interface.Examina;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Examina
{
    public class OptionImageRepository : GenericRepository<OptionImage>, IOptionImage
    {
        public OptionImageRepository(MainAPIContext db) : base(db) { }

        public async Task<IEnumerable<OptionImage>> GetOptionImageByOptionID(Guid optionID)
        {
            return from images in await GetBy(u => u.OptionID == optionID)
                   select images;
        }

        public async Task<IEnumerable<OptionImage>> GetOptionImageObjectsByNodeID(Guid NodeID)
        {
            return await GetBy(u => u.NodeID == NodeID);
        }
    }
}
