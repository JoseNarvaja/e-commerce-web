using ECommerceWeb.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ECommerceWeb.Models;
using ECommerceWeb.DataAccess.Data;

namespace ECommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _db;

        public ProductoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            Console.WriteLine("HOLA");
            IEnumerable<Producto> productos = await Task.Run(() => { return _unitOfWork.Producto.GetAll(includeProperties:"Categoria"); });
            return View(productos);
        }

        //[HttpGet]
        //public IActionResult Upsert(int? id)
        //{

        //}

    }
}
