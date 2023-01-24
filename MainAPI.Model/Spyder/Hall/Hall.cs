using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder.Hall
{
    public class Hall
    {
        public Guid ID { get; set; }
        public Guid CountryID { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string ClickObject { get; set; }
        public string IconName { get; set; }
        public string Banner { get; set; }
        public string HallCode { get; set; }
        public int RequiredVotes { get; set; }
        public decimal Cost { get; set; }

        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; } = default;
        public DateTime DateModified { get; set; } = default;
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
