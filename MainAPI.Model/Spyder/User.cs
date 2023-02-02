using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class User
    {
        public Guid ID { get; set; }
        public Guid WalletID { get; set; }
        public Guid CountryID { get; set; }
        public Guid FilterCountryID { get; set; }
        public bool IsGlobal { get; set; }
        public int AccessLevel { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string EmailVerification { get; set; }
        public string ResetVerification { get; set; }
        public string Email { get; set; }
        public bool IsActivated { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string RefCode { get; set; }
        public string RefererCode { get; set; }
        public string Image { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public DateTime EmailVerificationDate { get; set; } = default;
        public DateTime ResetVerificationDate { get; set; } = default;
        public DateTime DateCreated { get; set; } = default;
        public DateTime DateModified { get; set; } = default;
    }
}
