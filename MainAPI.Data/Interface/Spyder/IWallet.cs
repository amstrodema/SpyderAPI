using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Spyder
{
    public interface IWallet : IGeneric<Wallet>
    {
        Task<Wallet> GetWalletByUserID(Guid userID);
        Task<Wallet> GetWalletByRefCode(string refCode);
        Task<Wallet> GetWalletByAddress(string address);
        Task<List<Wallet>> GetWalletsByLegOneUserID(Guid userID);
        Task<List<Wallet>> GetWalletsByLegTwoUserID(Guid userID);
        Task<List<Wallet>> GetWalletsByUnpiadLegOneUserID(Guid userID);
        Task<List<Wallet>> GetWalletsByUnpaidLegTwoUserID(Guid userID);
    }
}
