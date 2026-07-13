using Carola.DataAccessLayer.Abstract;
using Carola.DataAccessLayer.Concrete;
using Carola.DataAccessLayer.Repository;
using CarolaEnditiyLayer.Entities;

namespace Carola.DataAccessLayer.EntitiyFrameWork
{
    public class EfCategoryDal : GenericRepository<Category>,ICategoryDal
    {
        public EfCategoryDal(CaraloContext context) : base(context)
        {

        }
    }
}
