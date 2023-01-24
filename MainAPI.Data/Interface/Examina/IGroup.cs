﻿using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Examina
{
    public interface IGroup : IGeneric<Group>
    {
        Task<IEnumerable<Group>> GetGroupsByNodeID(Guid NodeID);
    }
}
