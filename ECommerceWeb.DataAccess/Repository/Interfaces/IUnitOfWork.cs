using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.DataAccess.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        public IProductoRepository Producto { get; }
        public ICategoriaRepository Categoria { get; }
        public ICarritoComprasRepository CarritoCompras { get; }
        public IUsuarioRepository Usuario { get; }
        public IPedidoRepository Pedido { get; }
        public IPedidoDetalleRepository PedidoDetalle { get; }

        void Save();
    }
}
