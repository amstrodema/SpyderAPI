using MainAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface
{
    public interface IStatusEnum : IGeneric<StatusEnum>
    {
        Task<StatusEnum> GetStatusByCode(int code);
    }
}
