using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Models.Spyder
{
    public class ClientSystem
    {
        public Guid AppID { get; set; }
        public string JwtToken { get; set; }
    }
}
