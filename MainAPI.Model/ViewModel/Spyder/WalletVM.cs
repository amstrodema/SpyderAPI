using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel.Spyder
{
    public class WalletVM
    {
        public Wallet[] FirstLegWallets { get; set; }
        public Wallet[] SecondLegWallets { get; set; }
        public Wallet Wallet { get; set; }
        public int TotalFirstLeg { get; set; }
        public int TotalSecondLeg { get; set; }
        public int UnpaidFirstLegBalance { get; set; }
        public int UnpaidSecondLegBalance { get; set; }
    }
}
