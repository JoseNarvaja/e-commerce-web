using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using ECommerceWeb.Utility;
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
        [BindProperty]
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

        [HttpPost]
        [ActionName("Resumen")]
        public IActionResult ResumenPOST(CarritoComprasVM carritoComprasVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var idUsuario = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            CarritoComprasVM.Carritos = _unitOfWork.CarritoCompras.GetAll(c => c.IdUsuario == idUsuario, includeProperties: "Producto");

            CarritoComprasVM.Pedido.FechaPedido = System.DateTime.Now;
            CarritoComprasVM.Pedido.IdUsuario = idUsuario;

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

            CarritoComprasVM.Pedido.EstadoPedido = SD.EstadoEnProceso;
            CarritoComprasVM.Pedido.EstadoPago = SD.EstadoPagoPendiente;

            _unitOfWork.Pedido.Add(CarritoComprasVM.Pedido);
            _unitOfWork.Save();

            foreach (var carro in CarritoComprasVM.Carritos)
            {
                PedidoDetalle detalle = new PedidoDetalle();
                detalle.IdProducto = carro.IdProducto;
                detalle.IdPedido = CarritoComprasVM.Pedido.IdPedido;
                detalle.Cantidad = carro.Cantidad;

                if(carro.Producto.PrecioDescuento != null)
                {
                    detalle.PrecioIndividual = carro.Producto.PrecioDescuento.Value;
                }
                else
                {
                    detalle.PrecioIndividual = carro.Producto.Precio;
                }
                _unitOfWork.PedidoDetalle.Add(detalle);
            }

            _unitOfWork.CarritoCompras.RemoveRange(CarritoComprasVM.Carritos);
            HttpContext.Session.Clear();

            _unitOfWork.Save();
            TempData["exito"] = "Pedido realizado con exito";


            return Redirect("/Cliente/Home/Index");
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
                var cantidad = _unitOfWork.CarritoCompras.GetAll(c => c.IdUsuario == carritoDB.IdUsuario).ToList().Count -1;
                HttpContext.Session.SetInt32(SD.SesionCarroCompras, cantidad);
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
            var cantidad = _unitOfWork.CarritoCompras.GetAll(c => c.IdUsuario == carritoDB.IdUsuario).ToList().Count;
            HttpContext.Session.SetInt32(SD.SesionCarroCompras, cantidad);
            return RedirectToAction(nameof(Index));
        }

    }
}
