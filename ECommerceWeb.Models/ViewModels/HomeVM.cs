using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Carousel> CarouselImagenes { get; set; }
        public IEnumerable<Producto> Productos { get; set; }
    }
}
