using Carola.DataAccessLayer.Abstract;
using Carola.DataAccessLayer.Concrete;
using Carola.DataAccessLayer.Repository;
using CarolaEnditiyLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.DataAccessLayer.EntitiyFrameWork
{
    public class EfWeAreTheBestDal : GenericRepository<WeAreTheBest>, IWeAreTheBestDal
    {
        public EfWeAreTheBestDal(CaraloContext context) : base(context)
        {
        }
    }
}
