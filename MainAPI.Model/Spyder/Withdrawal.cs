using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class Withdrawal
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateCreated { get; set; }
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public DateTime DateModified { get; set; }
    }
}
