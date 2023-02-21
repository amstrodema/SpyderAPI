using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel.Spyder
{
    public class CommentVM
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public string CommenterName { get; set; }
        public string Details { get; set; }
        public string DateCreated { get; set; }
        public DateTime Date { get; set; }
        public string Image { get; set; }
    }
}
