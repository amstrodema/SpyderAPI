using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class Payment
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public string Message { get; set; }
        public string Reference { get; set; }
        public string Status { get; set; }
        public string Trans { get; set; }
        public string Transaction { get; set; }
        public string TrxRef { get; set; }
        public string PaymentFor { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Amount { get; set; }
    }
}
