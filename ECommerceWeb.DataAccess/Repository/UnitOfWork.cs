using ECommerceWeb.DataAccess.Data;
using ECommerceWeb.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.DataAccess.Repository
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly AppDbContext _contexto;
        public UnitOfWork(AppDbContext contexto)
        {
            _contexto = contexto;
            Producto = new ProductoRepository(contexto);
            Categoria = new CategoriaRepository(contexto);
            CarritoCompra = new CarritoCompraRepository(contexto);
            Usuario = new UsuarioRepository(contexto);
            Pedido = new PedidoRepository(contexto);
            PedidoDetalle = new PedidoDetalleRepository(contexto);
        }

        public IProductoRepository Producto { get; private set; }

        public ICategoriaRepository Categoria { get; private set; }

        public ICarritoCompraRepository CarritoCompra { get; private set; }

        public IUsuarioRepository Usuario { get; private set; }

        public IPedidoRepository Pedido { get; private set; }

        public IPedidoDetalleRepository PedidoDetalle { get; private set; }

        public void Save()
        {
            _contexto.SaveChanges();
        }
    }
}
