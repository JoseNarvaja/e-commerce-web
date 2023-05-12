using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.Models.ViewModels
{
    public class PermisosVM
    {
        public Usuario Usuario { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
