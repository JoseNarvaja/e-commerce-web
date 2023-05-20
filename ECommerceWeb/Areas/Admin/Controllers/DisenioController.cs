using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using ECommerceWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.RolAdmin)]
    public class DisenioController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWorK;

        public DisenioController(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _webHostEnvironment = webHostEnvironment;
            _unitOfWorK = unitOfWork;
        }

        public async Task<IActionResult> CarouselIndex()
        {
            IEnumerable<Carousel> carouseles = await _unitOfWorK.Carousel.GetAll();
            return View(carouseles);
        }

        [HttpGet]
        public async Task<IActionResult> UpsertCarousel(string nombre)
        {
            Carousel carousel = await _unitOfWorK.Carousel.GetFirstOrDefault(c => c.Nombre == nombre);
            if(carousel == null)
            {
                carousel = new Carousel()
                {
                    Id = 0,
                    Nombre = nombre,
                };
            }
            return View(carousel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> UpsertCarousel(Carousel carousel, IFormFile? imagen)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string nombreImagen = carousel.Nombre;
            var carpetaDestino = Path.Combine(wwwRootPath, @"images\carousel");
            if (imagen != null)
            {
                using (var fileStreams = new FileStream(Path.Combine(carpetaDestino, nombreImagen + ".webp"), FileMode.Create))
                {
                    imagen.CopyTo(fileStreams);
                }
            }

            if(carousel.Id> 0)
            {
                _unitOfWorK.Carousel.Update(carousel);
            }
            else
            {
                await _unitOfWorK.Carousel.Add(carousel);
            }

            await _unitOfWorK.Save();

            return RedirectToAction(nameof(CarouselIndex));
        }
    }
}
