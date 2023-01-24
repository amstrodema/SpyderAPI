using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Spyder.Feature
{
   public interface IFeature:IGeneric<Models.Spyder.Feature.Feature>
    {
        Task<IEnumerable<Models.Spyder.Feature.Feature>> GetFeaturesByItemID(Guid itemID);
    }
}
