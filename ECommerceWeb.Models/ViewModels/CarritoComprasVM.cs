using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.Models.ViewModels
{
    public class CarritoComprasVM
    {
        public IEnumerable<CarritoCompras> Carritos { get; set; }

        public Pedido Pedido { get; set; }
    }
}
