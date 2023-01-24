using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class Country
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Abbrv { get; set; }
        public string Code { get; set; }
        public string Flag { get; set; }

        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; } = default;
        public DateTime DateModified { get; set; } = default;
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
