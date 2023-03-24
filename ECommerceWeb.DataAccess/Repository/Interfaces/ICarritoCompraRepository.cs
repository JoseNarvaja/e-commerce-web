using ECommerceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.DataAccess.Repository.Interfaces
{
    public interface ICarritoCompraRepository : IRepository<CarritoCompra>
    {
        void Update(CarritoCompra obj);
        int IncreaseCount(CarritoCompra obj, int count);
        int DecreaseCount(CarritoCompra obj, int count);
    }
}
