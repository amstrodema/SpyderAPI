using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Petition.Spyder
{
    public class Petition
    {
        public Guid ID { get; set; }
        public Guid HallID { get; set; }
        public Guid PetitionerID { get; set; }
        public Guid PetitionCountryID { get; set; }
        public string NameOfPetitioned { get; set; }
        public string Brief { get; set; }
        public decimal PetitionCost { get; set; }
        public int RequiredVoters { get; set; }

        public string RecordOwnerName { get; set; }
        public string RecordOwnerStory { get; set; }
        public string RecordOwnerImage { get; set; }
        public string BtnBgTypeLike { get; set; }
        public string BtnBgTypeDisLike { get; set; }

        public bool IsReact { get; set; }
        public bool IsLike { get; set; }
        public bool IsApproved { get; set; }
        public bool IsAnonymous { get; set; }

        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; } = default;
        public DateTime DateModified { get; set; } = default;
    }
}
