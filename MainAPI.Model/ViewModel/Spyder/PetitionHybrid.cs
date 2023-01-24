using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel.Spyder
{
    public class PetitionHybrid
    {
        public Guid ID { get; set; }
        public string HallName { get; set; }
        public string Petitioner { get; set; }
        public string PetitionCountry { get; set; }
        public Guid PetitionCountryID{ get; set; }
        public string NameOfPetitioned { get; set; }
        public string Brief { get; set; }
        public string Type { get; set; }

        public string RecordOwnerName { get; set; }
        public string RecordOwnerStory { get; set; }
        public string RecordOwnerImage { get; set; }

        public bool IsApproved { get; set; }
        public bool IsAnonymous { get; set; }

        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; } = default;
    }
}
