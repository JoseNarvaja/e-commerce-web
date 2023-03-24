using ECommerceWeb.DataAccess.Data;
using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.DataAccess.Repository
{
    public class CarritoCompraRepository : Repository<CarritoCompra>, ICarritoCompraRepository
    {
        private readonly AppDbContext _context;

        public CarritoCompraRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public int DecreaseCount(CarritoCompra obj, int count)
        {
            obj.Cantidad += count;
            return obj.Cantidad;
        }

        public int IncreaseCount(CarritoCompra obj, int count)
        {
            obj.Cantidad -= count;
            return obj.Cantidad;
        }

        public void Update(CarritoCompra obj)
        {
            _context.CarritoCompra.Update(obj);
        }
    }
}
