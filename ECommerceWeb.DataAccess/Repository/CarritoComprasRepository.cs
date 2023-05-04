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
    public class CarritoComprasRepository : Repository<CarritoCompras>, ICarritoComprasRepository
    {
        private readonly AppDbContext _context;

        public CarritoComprasRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public int DecreaseCount(CarritoCompras obj, int count)
        {
            obj.Cantidad += count;
            return obj.Cantidad;
        }

        public int IncreaseCount(CarritoCompras obj, int count)
        {
            obj.Cantidad -= count;
            return obj.Cantidad;
        }

        public void Update(CarritoCompras obj)
        {
            _context.CarritoCompras.Update(obj);
        }
    }
}
