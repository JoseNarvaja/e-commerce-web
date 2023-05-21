using ECommerceWeb.DataAccess.Data;
using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.DataAccess.Repository
{
    public class CarouselRepository : Repository<Carousel>, ICarouselRepository
    {
        private readonly AppDbContext _contexto;

        public CarouselRepository(AppDbContext contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public void Update(Carousel carousel)
        {
            _contexto.Update(carousel);
        }
    }
}
