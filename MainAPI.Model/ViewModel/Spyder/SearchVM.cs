using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel.Spyder
{
    public class SearchVM
    {
        public Guid ID { get; set; }
        public string ValueType { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Brief { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
