using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Comment.Spyder
{
    public class Comment
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public Guid UserCountryID { get; set; }
        public Guid ItemID { get; set; }
        public string Details { get; set; }
        public bool IsFlagged { get; set; }

        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; } = default;
        public DateTime DateModified { get; set; } = default;
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
