using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class CountryRepository: GenericRepository<Country>, ICountry
    {
        public CountryRepository(MainAPIContext db) : base(db) { }
        public async Task<Country> GetCountryByCountryID(Guid countryID)
        {
            return await GetOneBy(u => u.ID == countryID && u.IsActive);
        }
        public async Task<Country> GetCountryByCountryAbbrev(string abbrv)
        {
            return await GetOneBy(u => u.Abbrv == abbrv && u.IsActive);
        }
    }
}
