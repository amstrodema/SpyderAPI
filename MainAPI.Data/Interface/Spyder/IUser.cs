using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Spyder
{
    public interface IUser : IGeneric<User>
    {
        Task<IEnumerable<User>> GetUsersByAccessLevel(int accessLevel);
        Task<User> GetUserByRefCode(string refCode);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByUserName(string username);
        Task<User> GetUserByEmailVerification(string emailVerification);
        Task<User> GetUserByResetVerification(string resetVerification);
        Task<string> GetUserRefCodeByUserID(Guid userID);
        Task<Guid> GetUserCountryByUserID(Guid userID);
        Task<User> GetUserByPhone(string phone);
        Task<IEnumerable<User>> GetValidUsers();
    }
}
