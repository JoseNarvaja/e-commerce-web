using ECommerceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.DataAccess.Repository.Interfaces
{
    public interface ICarritoComprasRepository : IRepository<CarritoCompras>
    {
        void Update(CarritoCompras obj);
        int IncreaseCount(CarritoCompras obj, int count);
        int DecreaseCount(CarritoCompras obj, int count);
    }
}
