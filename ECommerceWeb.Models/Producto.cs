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
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Descripcion { get; set; }

        public string? marca { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(0, float.MaxValue)]
        public float Precio { get; set; }

        [Range(0, float.MaxValue)]
        public float? PrecioDescuento { get; set; }
        [Range(1,100)]
        public float? PorcentajeDescuento { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string URLImagen { get; set; }

        [Required]
        public int IdCategoria { get; set; }

        [ForeignKey("IdCategoria")]
        [ValidateNever]
        public Categoria Categoria { get; set; }

    }
}
