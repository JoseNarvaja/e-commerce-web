using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceWeb.Models.ViewModels
{
    public class ProductoVM
    {
        public Producto Producto { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Categorias { get; set; }
    }
}
