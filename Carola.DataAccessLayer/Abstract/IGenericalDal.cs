using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.DataAccessLayer.Abstract
{
    public interface IGenericalDal<T> 
    {
        Task InsertAsync(T  entity) ;
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id) ;



    }
}
