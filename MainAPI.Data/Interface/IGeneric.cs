using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface
{
    public interface IGeneric<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> Find(Guid id);
        Task Create(T entity);
        void Update(T entity);
        void UpdateMultiple(T[] entity);
        void Delete(T entity);
        Task CreateMultiple(T[] entity);
    }
}
