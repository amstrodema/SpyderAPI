using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel.Spyder
{
    public class WithdrawalHybrid
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public string RefCode { get; set; }
        public decimal Amount { get; set; }
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public string UserStatus { get; set; }
        public string DateRequested { get; set; }
        public string DatePaid { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
