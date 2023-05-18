using ECommerceWeb.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerceWeb.Utility;
using ECommerceWeb.Models.ViewModels;
using ECommerceWeb.Models;
using System.Security.Claims;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ECommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PedidosController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PedidosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string estado)
        {
            IEnumerable<Pedido> pedidos;


            if (User.IsInRole(SD.RolAdmin) || User.IsInRole(SD.RolModerador))
            {
                pedidos = await _unitOfWork.Pedido.GetAll();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var idUsuario = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                pedidos = await _unitOfWork.Pedido.GetAll(p => p.IdUsuario == idUsuario);
            }

            switch (estado)
            {
                case "Completado":
                    pedidos = pedidos.Where(p => p.EstadoPedido == estado);
                    break;
                case "En Proceso":
                    pedidos = pedidos.Where(p => p.EstadoPedido == estado);
                    break;
                case "Enviado":
                    pedidos = pedidos.Where(p => p.EstadoPedido == estado);
                    break;
                case "Reembolsado":
                    pedidos = pedidos.Where(p => p.EstadoPedido == estado);
                    break;
                default:
                    break;
            }

            return View(pedidos);
        }

        public async Task<IActionResult> Detalles(int id)
        {
            PedidoVM pedidoVM = new PedidoVM()
            {
                Pedido = await _unitOfWork.Pedido.GetFirstOrDefault(p => p.IdPedido == id),
                Detalles = await _unitOfWork.PedidoDetalle.GetAll(d => d.IdPedido == id, includeProperties: "Producto")
            };

            return View(pedidoVM);
        }

        [Authorize(Roles = SD.RolModerador + "," + SD.RolAdmin)]
        [HttpPost]
        public async Task<IActionResult> ActualizarDetalles(PedidoVM pedidoVM)
        {
            Pedido pedidoDB = await _unitOfWork.Pedido.GetFirstOrDefault(p => p.IdPedido == pedidoVM.Pedido.IdPedido);
            pedidoDB.Nombre = pedidoVM.Pedido.Nombre;
            pedidoDB.Apellido = pedidoVM.Pedido.Apellido;
            pedidoDB.Telefono = pedidoVM.Pedido.Telefono;
            pedidoDB.Direccion = pedidoVM.Pedido.Direccion;
            pedidoDB.CodigoPostal = pedidoVM.Pedido.CodigoPostal;
            pedidoDB.Localidad = pedidoVM.Pedido.Localidad;
            pedidoDB.Provincia = pedidoVM.Pedido.Provincia;

            _unitOfWork.Pedido.Update(pedidoDB);
            await _unitOfWork.Save();

            TempData["exito"] = "Detalles de envio actualizados";
            return RedirectToAction(nameof(Detalles), new { id = pedidoDB.IdPedido });
        }

        [Authorize(Roles = SD.RolModerador + "," + SD.RolAdmin)]
        [HttpPost]
        public async Task<IActionResult> EnviarPedido(PedidoVM pedidoVM)
        {
            await _unitOfWork.Pedido.UpdateEstado(pedidoVM.Pedido, SD.EstadoEnviado);
            await _unitOfWork.Save();
            TempData["exito"] = "Estado de pedido actualizado";
            return RedirectToAction(nameof(Detalles), new { id = pedidoVM.Pedido.IdPedido });
        }

        [Authorize(Roles = SD.RolModerador + "," + SD.RolAdmin)]
        [HttpPost]
        public async Task<IActionResult> FinalizarPedido(PedidoVM pedidoVM)
        {
            await _unitOfWork.Pedido.UpdateEstado(pedidoVM.Pedido, SD.EstadoCompletado);
            await _unitOfWork.Save();
            TempData["exito"] = "Estado de pedido actualizado";
            return RedirectToAction(nameof(Detalles), new { id = pedidoVM.Pedido.IdPedido });
        }


        [Authorize(Roles = SD.RolModerador + "," + SD.RolAdmin)]
        [HttpPost]
        public IActionResult ReembolsarPedido(PedidoVM pedidoVM)
        {
            //TODO
            return RedirectToAction(nameof(Detalles), new { id = pedidoVM.Pedido.IdPedido });
        }

    }
}
