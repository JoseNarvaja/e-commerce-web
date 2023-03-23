using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.Models
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }

        [Required]
        public DateTime FechaPedido { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaEnvio { get; set; }
        [Required]
        public string EstadoPedido { get; set; }
        [Required]
        public string EstadoPago { get;set; }
        [Required(ErrorMessage ="Este campo es obligatorio")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int CodigoPostal { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Provincia { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Localidad { get; set; }
        [Required]
        public float TotalPedido { get; set; }

        [Required]
        public string IdUsuario { get; set; }

        [Required]
        [ForeignKey("IdUsuario")]
        [ValidateNever]
        public Usuario Usuario { get; set;}
    }
}
