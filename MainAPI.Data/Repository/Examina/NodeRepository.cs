using MainAPI.Data.Interface.Examina;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Examina
{
    public class NodeRepository : GenericRepository<Node>, INode
    {
        public NodeRepository(MainAPIContext db) : base(db) { }

        public async Task<Node> GetNodeByClientID(Guid clientID)
        {
            return await GetOneBy(u => u.ClientID == clientID);
        }
        public async Task<IEnumerable<Node>> GetNodesByClientID(Guid clientID)
        {
            return await GetBy(u => u.ClientID == clientID);
        }
        public async Task<Node> GetNodeByAccessCode(string accessCode)
        {
            return await GetOneBy(u => u.AccessCode == accessCode);
        }
    }
}
