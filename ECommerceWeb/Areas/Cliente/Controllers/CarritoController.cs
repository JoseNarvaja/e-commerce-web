using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceWeb.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [Authorize]
    public class CarritoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CarritoComprasVM CarritoComprasVM { get; set; }

        public CarritoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var idUsuario = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            CarritoComprasVM = new CarritoComprasVM()
            {
                Carritos = _unitOfWork.CarritoCompras.GetAll(c => c.IdUsuario == idUsuario, includeProperties: "Usuario,Producto")
            };

            foreach(var carrito in CarritoComprasVM.Carritos)
            {
                if(carrito.Producto.PrecioDescuento != null)
                {
                    CarritoComprasVM.PrecioTotal += carrito.Producto.PrecioDescuento.Value * carrito.Cantidad;
                }
                else
                {
                    CarritoComprasVM.PrecioTotal += carrito.Producto.Precio * carrito.Cantidad;
                }
            }
         
            return View(CarritoComprasVM);
        }

        public IActionResult Sumar(int id)
        {
            CarritoCompras carritoDB = _unitOfWork.CarritoCompras.GetFirstOrDefault(c => c.IdCarritoCompra == id);
            carritoDB.Cantidad += 1;
            _unitOfWork.CarritoCompras.Update(carritoDB);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Restar(int id)
        {
            CarritoCompras carritoDB = _unitOfWork.CarritoCompras.GetFirstOrDefault(c => c.IdCarritoCompra == id);
            if(carritoDB.Cantidad <= 1)
            {
                _unitOfWork.CarritoCompras.Remove(carritoDB);
            }
            else
            {
                carritoDB.Cantidad -= 1;
                _unitOfWork.CarritoCompras.Update(carritoDB);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Borrar(int id)
        {
            CarritoCompras carritoDB = _unitOfWork.CarritoCompras.GetFirstOrDefault(c => c.IdCarritoCompra == id);
            _unitOfWork.CarritoCompras.Remove(carritoDB);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

    }
}
