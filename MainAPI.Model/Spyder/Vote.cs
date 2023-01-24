using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class Vote
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public Guid UserCountryID { get; set; }
        public Guid ItemID { get; set; }

        public string BtnBgTypeDisLike { get; set; }
        public string BtnBgTypeLike { get; set; }
        public bool IsLike { get; set; }
        public bool IsReact { get; set; }

        public DateTime DateCreated { get; set; } = default;
        public DateTime DateModified { get; set; } = default;
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
