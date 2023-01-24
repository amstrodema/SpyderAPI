using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel.Spyder
{
    public class galleryVM
    {
        public IEnumerable<Image> Images { get; set; }
        public IEnumerable<Link> Links { get; set; }
    }
}
