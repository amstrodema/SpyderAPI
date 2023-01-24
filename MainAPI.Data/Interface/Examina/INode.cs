using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Examina
{
    public interface INode : IGeneric<Node>
    {
        Task<Node> GetNodeByClientID(Guid clientID);
        Task<Node> GetNodeByAccessCode(string accessCode);
        Task<IEnumerable<Node>> GetNodesByClientID(Guid clientID);
    }
}
