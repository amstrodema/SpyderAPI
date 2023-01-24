using MainAPI.Models.Spyder.Hall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface.Spyder
{
    public interface IHall: IGeneric<Hall>
    {
        Task<IEnumerable<Hall>> GetHallsByRoute(string route);
    }
}
