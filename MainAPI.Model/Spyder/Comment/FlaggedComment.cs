using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Comment.Spyder
{
    public class FlaggedComment
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public Guid CommentID { get; set; }
        public Guid FlaggedBy { get; set; }

        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; } = default;
        public DateTime DateModified { get; set; } = default;
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
