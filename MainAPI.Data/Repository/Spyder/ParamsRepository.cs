using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class ParamsRepository : GenericRepository<Params>, IParams
    {
        public ParamsRepository(MainAPIContext db) : base(db) { }
        public async Task<Params> GetParamByCode(string code)
        {
            return await GetOneBy(u => u.Code == code);
        }
    }
}
