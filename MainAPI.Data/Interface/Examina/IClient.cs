using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Examina
{
    public interface IClient : IGeneric<Client>
    {
        Task<Client> GetClientByLicense(string license);
        Task<Client> GetClientByCode(string code);
    }
}
