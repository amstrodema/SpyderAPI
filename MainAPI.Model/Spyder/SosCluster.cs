using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class SosCluster
    {
        public Guid ID { get; set; }
        public Guid SosID { get; set; }
        public Guid ClusterID { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal DistanceAwayFromSos { get; set; }
    }
}
