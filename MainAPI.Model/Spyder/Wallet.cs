using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class Wallet
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public Guid UserCountryID { get; set; }
        public string Address { get; set; }
        public string RefCode { get; set; }
        public decimal Bonus { get; set; }
        public decimal Spy { get; set; }
        public decimal Ref { get; set; }
        public decimal Gem { get; set; }
        public string BUSDWallet { get; set; }
        public Guid LegOneUserID { get; set; }
        public Guid LegTwoUserID { get; set; }
        public decimal ActivationCost { get; set; }
        public decimal LegOnePercentage { get; set; }
        public decimal LegTwoPercentage { get; set; }
        public bool IsPaidLegOne { get; set; }
        public bool IsPaidLegTwo { get; set; }
        public bool IsLocked { get; set; }
        public bool IsBanned { get; set; }
        public bool IsActive { get; set; }
        public bool IsOfficial { get; set; }
        public DateTime DateCreated { get; set; } = default;
        public DateTime DateModified { get; set; } = default;
    }
}
