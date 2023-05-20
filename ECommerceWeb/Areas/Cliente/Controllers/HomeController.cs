using ECommerceWeb.DataAccess.Repository;
using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using ECommerceWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceWeb.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Productos = await _unitOfWork.Producto.GetAll(includeProperties: "Categoria"),
                CarouselImagenes = await _unitOfWork.Carousel.GetAll()
            };
            return View(homeVM);
        }

        public async Task<IActionResult> Detalles(int id)
        {
            CarritoCompras carrito = new CarritoCompras()
            {
                Producto = await _unitOfWork.Producto.GetFirstOrDefault(u => u.IdProducto== id, includeProperties:"Categoria"),
                Cantidad = 1,
                IdProducto = id
            };
            return View(carrito);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Detalles(CarritoCompras carrito)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var idUsuario = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            carrito.IdUsuario = idUsuario;

            CarritoCompras carritoDeDB = await _unitOfWork.CarritoCompras.GetFirstOrDefault(u=> u.IdUsuario == idUsuario && u.IdProducto == carrito.IdProducto);

            if(carritoDeDB != null)
            {
                carritoDeDB.Cantidad += carrito.Cantidad;
                _unitOfWork.CarritoCompras.Update(carritoDeDB);
                await _unitOfWork.Save();
            }
            else
            {
                await _unitOfWork.CarritoCompras.Add(carrito);
                await _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SesionCarroCompras,
                    (await _unitOfWork.CarritoCompras.GetAll(c => c.IdUsuario == idUsuario)).Count());
            }

            TempData["exito"] = "Carrito actualizado con exito";

            return RedirectToAction("Index");
        }

    }
}
