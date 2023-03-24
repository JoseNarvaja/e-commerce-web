using ECommerceWeb.DataAccess.Data;
using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.DataAccess.Repository
{
    public class PedidoDetalleRepository : Repository<PedidoDetalle>, IPedidoDetalleRepository
    {
        private readonly AppDbContext _context;

        public PedidoDetalleRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(PedidoDetalle obj)
        {
            _context.PedidoDetalle.Update(obj);
        }
    }
}
