﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.Models
{
    public class CarritoCompra
    {
        [Key]
        public int IdCarritoCompra { get; set; }

        [Required(ErrorMessage ="Este campo es obligatorio")]
        public int Cantidad { get; set; }

        [Required]
        public string IdUsuario { get; set; }

        [Required]
        [ForeignKey("IdUsuario")]
        [ValidateNever]
        public Usuario Usuario { get; set;}

        [Required]
        public int IdProducto { get; set; }

        [Required]
        [ForeignKey("IdProducto")]
        [ValidateNever]
        public Producto Producto { get; set; }
    }
}
