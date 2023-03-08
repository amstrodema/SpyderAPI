using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel.Spyder
{
    public class WithdrawalVM
    {
        public IEnumerable<WithdrawalHybrid> Withdrawals { get; set; }
        public IEnumerable<Payroll> Payrolls { get; set; }
        public string Date { get; set; }
    }
}