using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class FlagReport
    {
        public Guid ID { get; set; }
        public Guid PetitionerID { get; set; }
        public Guid ItemID { get; set; }
        public string CaseLink { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsTreated { get; set; }
        public bool IsActive { get; set; }
    }
}
