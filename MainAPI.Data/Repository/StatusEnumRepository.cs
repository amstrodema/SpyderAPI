using MainAPI.Data.Interface;
using MainAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository
{
    public class StatusEnumRepository : GenericRepository<StatusEnum>, IStatusEnum
    {
        public StatusEnumRepository(MainAPIContext db) : base(db) { }

        public async Task<StatusEnum> GetStatusByCode(int code)
        {
            return await GetOneBy(u => u.Code == code);
        }
    }
}
