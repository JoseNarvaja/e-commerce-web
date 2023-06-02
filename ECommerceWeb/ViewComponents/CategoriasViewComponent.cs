using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ECommerceWeb.Models;

namespace ECommerceWeb.ViewComponents
{
    public class CategoriasViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriasViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<Categoria> categorias = await _unitOfWork.Categoria.GetAll();
            return View(categorias);
        }

    }
}
