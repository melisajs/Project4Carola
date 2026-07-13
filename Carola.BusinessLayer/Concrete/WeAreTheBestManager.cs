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
    public class WeAreTheBestManager : IWeAreTheBestService
    {
        private readonly IWeAreTheBestDal _weAreTheBestDal;

        public WeAreTheBestManager(IWeAreTheBestDal weAreTheBestDal)
        {
            _weAreTheBestDal = weAreTheBestDal;
        }

        public async Task TDeleteAsync(int id)
        {
            await _weAreTheBestDal.DeleteAsync(id);
        }

        public async Task<List<WeAreTheBest>> TGetAllAsync()
        {
            return await _weAreTheBestDal.GetAllAsync();
        }

        public async Task<WeAreTheBest> TGetByIdAsync(int id)
        {
            return await _weAreTheBestDal.GetByIdAsync(id);
        }

        public async Task TInsertAsync(WeAreTheBest entity)
        {
            await _weAreTheBestDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(WeAreTheBest entity)
        {
            await _weAreTheBestDal.UpdateAsync(entity);
        }
    }
}
