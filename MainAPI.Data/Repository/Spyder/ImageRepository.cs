using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class ImageRepository : GenericRepository<Image>, IImage
    {
        public ImageRepository(MainAPIContext db) : base(db) { }
        public async Task<IEnumerable<Image>> GetItemImages(Guid itemID)
        {
            return await GetBy(x => x.ItemID == itemID);
        }
    }
}
