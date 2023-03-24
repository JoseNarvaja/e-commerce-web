using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.Models
{
    public class PedidoDetalle
    {
        [Key]
        public int IdDetallePedido { get; set;}

        [Required]
        public int Cantidad { get; set;}
        [Required]
        public int PrecioIndividual { get; set;}

        [Required]
        public int IdPedido { get; set;}
        [Required]
        [ForeignKey("IdPedido")]
        [ValidateNever]
        public Pedido Pedido { get; set; }
    }
}
