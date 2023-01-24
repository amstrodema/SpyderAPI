using MainAPI.Data.Interface.Spyder;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository.Spyder
{
    public class ConfessionRepository:GenericRepository<Confession>,IConfession
    {
        public ConfessionRepository(MainAPIContext db) : base(db) { }
        public async Task<IEnumerable<Confession>> GetConfessionsByDialogueTypeNo(int dialogueTypeNo)
        {
            return await GetBy(x => x.DialogueTypeNo == dialogueTypeNo);
        }
    }
}
