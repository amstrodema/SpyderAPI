using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel.Spyder
{
    public class MissingDetails
    {
        public Guid ID { get; set; }
        public string ItemTypeName{ get; set; }
        public string Country { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string FullInfo { get; set; }
        public string Image { get; set; }

        public string LastSeen { get; set; }

        public int AwarenessTypeNo { get; set; }

        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public Guid CreatorID { get; set; }
        public string Time { get; set; }
        public string Day { get; set; }
    }
}
