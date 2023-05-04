using ECommerceWeb.DataAccess.Repository;
using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Models;
using Microsoft.AspNetCore.Authorization;
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

        public IActionResult Index()
        {
            IEnumerable<Producto> productos = _unitOfWork.Producto.GetAll(includeProperties:"Categoria");
            return View(productos);
        }

        public IActionResult Detalles(int id)
        {
            CarritoCompras carrito = new CarritoCompras()
            {
                Producto = _unitOfWork.Producto.GetFirstOrDefault(u => u.IdProducto== id, includeProperties:"Categoria"),
                Cantidad = 1,
                IdProducto = id
            };
            return View(carrito);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Detalles(CarritoCompras carrito)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var idUsuario = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            carrito.IdUsuario = idUsuario;

            CarritoCompras carritoDeDB = _unitOfWork.CarritoCompras.GetFirstOrDefault(u=> u.IdUsuario == idUsuario && u.IdProducto == carrito.IdProducto);

            if(carritoDeDB != null)
            {
                carritoDeDB.Cantidad += carrito.Cantidad;
                _unitOfWork.CarritoCompras.Update(carritoDeDB);
            }
            else
            {
                _unitOfWork.CarritoCompras.Add(carrito);
            }
            TempData["exito"] = "Carrito actualizado con exito";
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

    }
}
