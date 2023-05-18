using ECommerceWeb.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceWeb.Models;
using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.DataAccess.Data;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWeb.DataAccess.Repository
{
    public class PedidoRepository : Repository<Pedido>, IPedidoRepository
    {
        private readonly AppDbContext _contexto;

        public PedidoRepository(AppDbContext contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public void Update(Pedido pedido)
        {
            _contexto.Pedido.Update(pedido);
        }

        public async Task UpdateEstado(Pedido pedido, string estado)
        {
            var pedidoDb = await _contexto.Pedido.FirstOrDefaultAsync(u=> u.IdPedido== pedido.IdPedido);
            pedidoDb.EstadoPedido = estado;
        }

        public async Task UpdateEstadoPago(Pedido pedido, string estado)
        {
            var pedidoDb = await _contexto.Pedido.FirstOrDefaultAsync(u => u.IdPedido == pedido.IdPedido);
            pedidoDb.EstadoPago = estado;
        }
    }
}
