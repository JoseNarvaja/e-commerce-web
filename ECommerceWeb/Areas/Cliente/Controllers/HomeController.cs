using ECommerceWeb.DataAccess.Repository;
using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Models;
using Microsoft.AspNetCore.Mvc;

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
            Producto producto = _unitOfWork.Producto.GetFirstOrDefault(u=>u.IdProducto == id, includeProperties: "Categoria");
            return View(producto);
        }
    }
}
