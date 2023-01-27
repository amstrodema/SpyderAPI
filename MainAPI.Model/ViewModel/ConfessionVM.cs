using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel
{
    public class ConfessionVM
    {
        public Guid ID { get; set; }
        public Guid CountryID { get; set; }
        public string CountryName { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string Image { get; set; }
        public bool IsAnonymous { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string CreatedBy { get; set; }
        public Guid CreatedByID { get; set; }
        public int DialogueTypeNo { get; set; }
        public int TotalLikes { get; set; }
        public int TotalDisLikes { get; set; }
        public bool IsLike { get; set; }
        public bool IsReact { get; set; }
        public DateTime DateFilter { get; set; }
    }
}
