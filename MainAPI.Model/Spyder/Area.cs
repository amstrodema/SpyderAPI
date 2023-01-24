using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class Area
    {
        public Guid ID { get; set; }
        public decimal ActivationCost { get; set; }
        public decimal FirstLegPercentage { get; set; }
        public decimal SecondLegPercentage { get; set; }
    }
}
