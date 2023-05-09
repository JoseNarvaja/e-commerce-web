using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.Models.ViewModels
{
    public class PedidoVM
    {
        public Pedido Pedido { get; set; }
        public IEnumerable<PedidoDetalle> Detalles { get; set;}
    }
}
