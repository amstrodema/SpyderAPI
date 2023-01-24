using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Spyder
{
    public interface ICountry : IGeneric<Country>
    {
        Task<Country> GetCountryByCountryID(Guid countryID);
        Task<Country> GetCountryByCountryAbbrev(string abbrv);
    }
}
