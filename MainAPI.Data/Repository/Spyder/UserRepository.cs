using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class UserRepository : GenericRepository<User>, IUser
    {
        public UserRepository(MainAPIContext db) : base(db) { }

        public async Task<User> GetUserByRefCode(string refCode)
        {
            return await GetOneBy(u => u.RefCode == refCode);
        }
        public async Task<User> GetUserByUserName(string username)
        {
            return await GetOneBy(u => u.Username == username);
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await GetOneBy(u => u.Email == email);
        } 
        public async Task<User> GetUserByPhone(string phone)
        {
            return await GetOneBy(u => u.Phone == phone);
        }
        public async Task<string> GetUserRefCodeByUserID(Guid userID)
        {
            return (await GetOneBy(u => u.ID == userID)).RefCode;
        }
        public async Task<Guid> GetUserCountryByUserID(Guid userID)
        {
            return (await GetOneBy(u => u.ID == userID)).CountryID;
        }
        public async Task<User> GetUserByEmailVerification(string emailVerification)
        {
            return await GetOneBy(u => u.EmailVerification == emailVerification);
        }
        public async Task<User> GetUserByResetVerification(string resetVerification)
        {
            return await GetOneBy(u => u.ResetVerification == resetVerification);
        }
        public async Task<IEnumerable<User>> GetUsersByAccessLevel(int accessLevel) => await GetBy(u => u.AccessLevel == accessLevel);
        public async Task<IEnumerable<User>> GetValidUsers() 
            => await GetBy(u => u.IsActive && u.IsActivated && !u.IsBanned && u.IsVerified && !string.IsNullOrEmpty(u.BankAccountNumber) && !string.IsNullOrEmpty(u.BankAccountName) && !string.IsNullOrEmpty(u.BankName));
    }
}
