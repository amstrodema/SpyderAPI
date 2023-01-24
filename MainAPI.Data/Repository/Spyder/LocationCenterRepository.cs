using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class LocationCenterRepository : GenericRepository<LocationCenter>, ILocationCenter
    {
        public LocationCenterRepository(MainAPIContext db) : base(db) { }
    }
}
