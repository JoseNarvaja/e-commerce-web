using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.Models.ViewModels
{
    public class ProductVM
    {
        public Producto Producto { get; set; }

        public IEnumerable<Categoria> Categorias { get; set; }
    }
}
