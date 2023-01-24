using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder.Hall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class HallRepository:  GenericRepository<Hall>, IHall
    {
        public HallRepository(MainAPIContext db): base(db) { }

        public async Task<IEnumerable<Hall>> GetHallsByRoute(string route)
        {
            return await GetBy(x => x.HallCode == route);
        }
    }
}
