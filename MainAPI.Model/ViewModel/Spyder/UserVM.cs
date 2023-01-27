using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.ViewModel.Spyder
{
    public class UserVM
    {
        public User User { get; set; }
        public Settings Settings { get; set; }
        public ClientSystem ClientSystem { get; set; }

    }
}
