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
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var idUsuario = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            CarritoComprasVM = new CarritoComprasVM()
            {
                Carritos = _unitOfWork.CarritoCompras.GetAll(c => c.IdUsuario == idUsuario, includeProperties: "Usuario,Producto"),
                Pedido = new()
            };

            foreach(var carrito in CarritoComprasVM.Carritos)
            {
                if(carrito.Producto.PrecioDescuento != null)
                {
                    CarritoComprasVM.Pedido.TotalPedido += carrito.Producto.PrecioDescuento.Value * carrito.Cantidad;
                }
                else
                {
                    CarritoComprasVM.Pedido.TotalPedido += carrito.Producto.Precio * carrito.Cantidad;
                }
            }
         
            return View(CarritoComprasVM);
        }

        public IActionResult Resumen()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var idUsuario = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            CarritoComprasVM = new CarritoComprasVM()
            {
                Carritos = _unitOfWork.CarritoCompras.GetAll(c => c.IdUsuario == idUsuario, includeProperties: "Usuario,Producto"),
                Pedido = new()
            };

            CarritoComprasVM.Pedido.Usuario = _unitOfWork.Usuario.GetFirstOrDefault(u => u.Id == idUsuario);

            CarritoComprasVM.Pedido.Nombre = CarritoComprasVM.Pedido.Usuario.Nombre;
            CarritoComprasVM.Pedido.Apellido = CarritoComprasVM.Pedido.Usuario.Apellido;
            CarritoComprasVM.Pedido.Direccion = CarritoComprasVM.Pedido.Usuario.Direccion;
            CarritoComprasVM.Pedido.Localidad = CarritoComprasVM.Pedido.Usuario.Localidad;
            CarritoComprasVM.Pedido.Provincia = CarritoComprasVM.Pedido.Usuario.Provincia;

            foreach (var carrito in CarritoComprasVM.Carritos)
            {
                if (carrito.Producto.PrecioDescuento != null)
                {
                    CarritoComprasVM.Pedido.TotalPedido += carrito.Producto.PrecioDescuento.Value * carrito.Cantidad;
                }
                else
                {
                    CarritoComprasVM.Pedido.TotalPedido += carrito.Producto.Precio * carrito.Cantidad;
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
