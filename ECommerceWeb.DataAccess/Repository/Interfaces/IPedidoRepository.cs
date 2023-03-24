using ECommerceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.DataAccess.Repository.Interfaces
{
    public interface IPedidoRepository :IRepository<Pedido>
    {
        void Update(Pedido pedido);
        void UpdateEstado(Pedido pedido, string estado);
        void UpdateEstadoPago(Pedido pedido, string estado);
    }
}
