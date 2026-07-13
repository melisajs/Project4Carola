using Carola.DataAccessLayer.Abstract;
using Carola.DataAccessLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.DataAccessLayer.Repository
{
    public class GenericRepository<T> : IGenericalDal<T> where T : class

    {
        private readonly CaraloContext _context;

        public GenericRepository(CaraloContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(int id)
        {
            var value =await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(value);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task InsertAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(T entity)
        {
           _context.Set<T>().Update(entity);
            return _context.SaveChangesAsync();
        }
    }
}
