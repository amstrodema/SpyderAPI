using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class WalletRepository : GenericRepository<Wallet>, IWallet
    {
        public WalletRepository(MainAPIContext db) : base(db) { }

        public async Task<Wallet> GetWalletByUserID(Guid userID)
        {
            return await GetOneBy(u => u.UserID == userID);
        }

        public async Task<Wallet> GetWalletByRefCode(string refCode)
        {
            return await GetOneBy(u => u.RefCode == refCode);
        }
        public async Task<Wallet> GetWalletByAddress(string address)
        {
            return await GetOneBy(u => u.Address == address);
        }
        public async Task<List<Wallet>> GetWalletsByLegOneUserID(Guid userID)
        {
            return (await GetBy(u => u.LegOneUserID == userID)).ToList();
        }
        public async Task<List<Wallet>> GetWalletsByLegTwoUserID(Guid userID)
        {
            return (await GetBy(u => u.LegTwoUserID == userID)).ToList();
        }
        public async Task<List<Wallet>> GetWalletsByUnpiadLegOneUserID(Guid userID)
        {
            return (await GetBy(u => u.LegOneUserID == userID && !u.IsPaidLegOne)).ToList();
        }
        public async Task<List<Wallet>> GetWalletsByUnpaidLegTwoUserID(Guid userID)
        {
            return (await GetBy(u => u.LegTwoUserID == userID && !u.IsPaidLegTwo)).ToList();
        }
        //public async Task<IEnumerable<Wallet>> GetUsersByAccessLevel(int accessLevel) => await GetBy(u => u.AccessLevel == accessLevel);

    }
}
