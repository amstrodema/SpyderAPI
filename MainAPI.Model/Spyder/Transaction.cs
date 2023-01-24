using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class Transaction
    {
        public Guid ID { get; set; }
        public Guid CountryID { get; set; }
        public Guid SenderID { get; set; }
        public Guid ReceiverID { get; set; }
        public Guid SenderWalletID { get; set; }
        public Guid ReceiverWalletID { get; set; }
        public string TransactionType { get; set; }
        public string SenderRefCode { get; set; }
        public string ReceiverRefCode { get; set; }
        public string WalletAddress { get; set; }
        public decimal NetworkFee { get; set; }
        public bool IsConfirmed { get; set; }
        public decimal Amount { get; set; }
        public string InitiatorBUSDAddress { get; set; }
        public string RecieverBUSDAddress { get; set; }
        public string BUSDTransactionHash { get; set; }
        public string TransactionDesc { get; set; }
        public string TransactionCurrency { get; set; }
        public bool IsOfficial { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; } = default;
        public DateTime DateModified { get; set; } = default;

    }
}
