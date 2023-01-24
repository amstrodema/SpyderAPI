using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel.Spyder
{
    public class VoteVM
    {
        public Guid ID { get; set; }
        public int TotalUpVote { get; set; }
        public int TotalDownVote { get; set; }
        public float VotePercentage { get; set; }
        public Guid UserID { get; set; }
        public Guid ItemID { get; set; }
    }
}
