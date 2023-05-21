using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using ECommerceWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ECommerceWeb.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [Authorize]
    public class CarritoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHost;
        [BindProperty]
        public CarritoComprasVM CarritoComprasVM { get; set; }

        public CarritoController(IUnitOfWork unitOfWork, IEmailSender emailSender, IWebHostEnvironment webHost)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _webHost = webHost;
        }
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var idUsuario = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            CarritoComprasVM = new CarritoComprasVM()
            {
                Carritos = await _unitOfWork.CarritoCompras.GetAll(c => c.IdUsuario == idUsuario, includeProperties: "Usuario,Producto"),
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

        public async Task<IActionResult> Resumen()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var idUsuario = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            CarritoComprasVM = new CarritoComprasVM()
            {
                Carritos = await _unitOfWork.CarritoCompras.GetAll(c => c.IdUsuario == idUsuario, includeProperties: "Usuario,Producto"),
                Pedido = new()
            };

            if(CarritoComprasVM.Carritos.Count() <= 0)
            {
                TempData["error"] = "No hay productos en el carrito";
                return RedirectToAction(nameof(Index));
            }

            CarritoComprasVM.Pedido.Usuario = await _unitOfWork.Usuario.GetFirstOrDefault(u => u.Id == idUsuario);

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
        public async Task<IActionResult> ResumenPOST(CarritoComprasVM carritoComprasVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var idUsuario = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            CarritoComprasVM.Carritos = await _unitOfWork.CarritoCompras.GetAll(c => c.IdUsuario == idUsuario, includeProperties: "Producto,Usuario");

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

            await _unitOfWork.Pedido.Add(CarritoComprasVM.Pedido);
            await _unitOfWork.Save();

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
                await _unitOfWork.PedidoDetalle.Add(detalle);
            }

            string pathTemplate = _webHost.WebRootPath + Path.DirectorySeparatorChar.ToString()
                        + "Templates" + Path.DirectorySeparatorChar.ToString() + "ConfirmacionPedido.html";

            
            _unitOfWork.CarritoCompras.RemoveRange(CarritoComprasVM.Carritos);
            HttpContext.Session.Clear();

            await _unitOfWork.Save();
            TempData["exito"] = "Pedido realizado con exito";

            var htmlBody = "";
            using (StreamReader streamReader = System.IO.File.OpenText(pathTemplate))
            {
                htmlBody = streamReader.ReadToEnd();
            }
            //HtmlEncoder.Default.Encode(callbackUrl)
            string messageBody = string.Format(htmlBody,
                CarritoComprasVM.Pedido.IdPedido,
                CarritoComprasVM.Pedido.TotalPedido,
                CarritoComprasVM.Pedido.Direccion,
                CarritoComprasVM.Pedido.Localidad,
                CarritoComprasVM.Pedido.Provincia,
                CarritoComprasVM.Pedido.EstadoPedido);



            await _emailSender.SendEmailAsync(CarritoComprasVM.Pedido.Usuario.Email, "Pedido realizado - ecommerce web", messageBody);

            return Redirect("/Cliente/Home/Index");
        }


        public async Task<IActionResult> Sumar(int id)
        {
            CarritoCompras carritoDB = await _unitOfWork.CarritoCompras.GetFirstOrDefault(c => c.IdCarritoCompra == id);
            carritoDB.Cantidad += 1;
            _unitOfWork.CarritoCompras.Update(carritoDB);
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Restar(int id)
        {
            CarritoCompras carritoDB = await _unitOfWork.CarritoCompras.GetFirstOrDefault(c => c.IdCarritoCompra == id);
            if(carritoDB.Cantidad <= 1)
            {
                _unitOfWork.CarritoCompras.Remove(carritoDB);
                var cantidad = (await _unitOfWork.CarritoCompras.GetAll(c => c.IdUsuario == carritoDB.IdUsuario)).ToList().Count -1;
                HttpContext.Session.SetInt32(SD.SesionCarroCompras, cantidad);
            }
            else
            {
                carritoDB.Cantidad -= 1;
                _unitOfWork.CarritoCompras.Update(carritoDB);
            }
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Borrar(int id)
        {
            CarritoCompras carritoDB = await _unitOfWork.CarritoCompras.GetFirstOrDefault(c => c.IdCarritoCompra == id);
            _unitOfWork.CarritoCompras.Remove(carritoDB);
            await _unitOfWork.Save();
            var cantidad = (await _unitOfWork.CarritoCompras.GetAll(c => c.IdUsuario == carritoDB.IdUsuario)).ToList().Count;
            HttpContext.Session.SetInt32(SD.SesionCarroCompras, cantidad);
            return RedirectToAction(nameof(Index));
        }

    }
}
