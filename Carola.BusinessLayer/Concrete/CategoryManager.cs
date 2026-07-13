using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Abstract;
using CarolaEnditiyLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public async Task TDeleteAsync(int id)
        {
           await _categoryDal.DeleteAsync(id);
        }

        public async Task<List<Category>> TGetAllAsync()
        {
           return await _categoryDal.GetAllAsync();
        }

        public Task<Category> TGetByIdAsync(int id)
        {
            return _categoryDal.GetByIdAsync(id);
        }

        public Task TInsertAsync(Category entity)
        {
            return _categoryDal.InsertAsync(entity);
        }

        public Task TUpdateAsync(Category entity)
        {
            return _categoryDal.UpdateAsync(entity);
        }
    }
}
