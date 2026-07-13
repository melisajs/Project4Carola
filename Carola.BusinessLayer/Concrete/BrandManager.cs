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
    public class BrandManager : IBrandService
    {
        private readonly IBrandDal _brandDal;
        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }
        public async Task TDeleteAsync(int id)
        {
            await _brandDal.DeleteAsync(id);
        }
        public async Task<List<Brand>> TGetAllAsync()
        {
            return await _brandDal.GetAllAsync();
        }
        public async Task<Brand> TGetByIdAsync(int id)
        {
            return await _brandDal.GetByIdAsync(id);
        }
        public async Task TInsertAsync(Brand entity)
        {
            if (string.IsNullOrWhiteSpace(entity.BrandName))
                throw new Exception("Marka adı boş olamaz");

            if (entity.BrandName.Length < 2)
                throw new Exception("Marka adı en az 2 karakter olmalıdır");

            var brands = await _brandDal.GetAllAsync();

            if (brands.Any(x => x.BrandName == entity.BrandName))
                throw new Exception("Bu marka zaten mevcut");

            await _brandDal.InsertAsync(entity);

        }
        public async Task TUpdateAsync(Brand entity)
        {
            await _brandDal.UpdateAsync(entity);
            
            }
        }
    }


