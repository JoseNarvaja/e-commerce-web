using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.Models
{
    public class Carousel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        [MaxLength(30)]
        public string Titulo { get; set; }
        [Required]
        [MaxLength(100)]
        public string Descripcion { get; set; }
    }
}
