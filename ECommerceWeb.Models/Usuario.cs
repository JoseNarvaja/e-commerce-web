using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceWeb.Models
{
    public class Usuario : IdentityUser
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Apellido { get; set; }

        public string? Direccion { get; set; }
        public string? CodigoPostal { get; set; }
        public string? Provincia { get; set; }
        public string? Localidad { get; set; }

        [NotMapped]
        public string Rol { get; set; }
    }
}
