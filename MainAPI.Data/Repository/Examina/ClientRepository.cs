using MainAPI.Data.Interface.Examina;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Examina
{
    public class ClientRepository : GenericRepository<Client>, IClient
    {
        public ClientRepository(MainAPIContext db) : base(db) { }

        public async Task<Client> GetClientByCode(string code)
        {
            return await GetOneBy(u => u.Code == code);
        }
        public async Task<Client> GetClientByLicense(string license)
        {
            return await GetOneBy(u => u.License == license);
        }
    }
}
