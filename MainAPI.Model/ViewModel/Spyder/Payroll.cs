using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel.Spyder
{
    public class Payroll
    {
        public int Sn { get; set; }
        public string Amount { get; set; }
        public string AcctNo { get; set; }
        public string Bank { get; set; }
        public string AcctName { get; set; }
        //public string RefCode { get; set; }
        //public string RequestDate { get; set; }
        public string PayDate { get; set; }
    }
}