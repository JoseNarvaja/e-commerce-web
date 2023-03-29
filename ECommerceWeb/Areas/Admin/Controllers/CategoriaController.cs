using ECommerceWeb.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Areas.Admin.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public CategoriaController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
