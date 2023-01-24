using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainAPI.Models.Petition;

namespace MainAPI.Models.ViewModel.Spyder.Petition
{
   public class PetitionVM
    {
        public Guid ID { get; set; }
        public Guid PetitionerID { get; set; }
        public string HallName { get; set; }
        public string Brief { get; set; }
        public string RecordOwnerName { get; set; }
        public string RecordOwnerStory { get; set; }
        public string Time { get; set; }
        public string Day { get; set; }
        public string Petitioner { get; set; }
        public string RecordOwnerImage { get; set; }
        public bool IsReact { get; set; }
        public bool IsLike { get; set; }
        public bool IsApproved { get; set; }
        public string BtnBgTypeLike { get; set; }
        public string BtnBgTypeDisLike { get; set; }
        public decimal TotalUpVotes { get; set; } 
        public decimal TotalDownVotes { get; set; }
        public float VotePercentage { get; set; }
        public IEnumerable<Models.Comment.Spyder.Comment> Comments { get; set; }
        public string ClickObject { get; set; }
    }
}
