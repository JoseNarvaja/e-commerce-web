using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceWeb.ViewComponents
{
    public class CarroComprasViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public CarroComprasViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(claim != null)
            {
                if(HttpContext.Session.GetInt32(SD.SesionCarroCompras) == null)
                {
                    HttpContext.Session.SetInt32(SD.SesionCarroCompras,
                    _unitOfWork.CarritoCompras.GetAll(c => c.IdUsuario == claim.Value).Count());
                }
                
                return View(HttpContext.Session.GetInt32(SD.SesionCarroCompras));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }

    }
}
