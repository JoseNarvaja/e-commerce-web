using ECommerceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.DataAccess.Repository.Interfaces
{
    public interface ICarouselRepository : IRepository<Carousel>
    {
        void Update (Carousel carousel);
    }
}
