using MainAPI.Models.Spyder;
using MainAPI.Models.ViewModel.Spyder.Petition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel.Spyder
{
    public class TrendingVM
    {
        public int NewRegistration { get; set; }
        public decimal Earnings { get; set; }
        public decimal ReferralBonus { get; set; }
        public int SosSignals { get; set; }

        public int MarriedRecord { get; set; }
        public int DeathRecord { get; set; }

        public int MissingRecord { get; set; }
        public int StolenRecord { get; set; }
        public int FoundRecord { get; set; }

        public int ConfessionRecord { get; set; }
        public int ScammersRecord { get; set; }
        public int WhistleBlowerRecord { get; set; }

        public int LegendRecord { get; set; }
        public int HerosRecord { get; set; }
        public int FallHeroRecord { get; set; }
        public int HOSRecord { get; set; }
        public int HOFRecord { get; set; }

        public IEnumerable<PetitionVM> PetitionVMs { get; set; }
        public IEnumerable<Marriage> Marriages { get; set; }
        public IEnumerable<Death> Deaths { get; set; }
        public IEnumerable<ConfessionVM> ConfessionVMs { get; set; }
        public IEnumerable<Missing> Missing { get; set; }
        public IEnumerable<Missing> Stolen { get; set; }

    }
}
