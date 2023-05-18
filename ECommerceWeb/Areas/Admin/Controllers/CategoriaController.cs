using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Models;
using ECommerceWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.RolAdmin)]
    public class CategoriaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Categoria> categorias = await _unitOfWork.Categoria.GetAll();
            return View(categorias);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            Categoria categoria = new Categoria();
            if(id != null | id > 0)
            {
                categoria = await _unitOfWork.Categoria.GetFirstOrDefault((u => u.IdCategoria == id));
            }
            return View(categoria);
        }



        [HttpGet("/Admin/Categoria/Delete/{id:int?}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Categoria categoria = await _unitOfWork.Categoria.GetFirstOrDefault(u => u.IdCategoria == id);

            IEnumerable<Producto> productosAsociados = await _unitOfWork.Producto.GetAll(u => u.IdCategoria == id);
            if(productosAsociados.Count() >= 1)
            {
                TempData["error"] = "No se puede eliminar una categoria si tiene productos asociados";
                return RedirectToAction("Index");
            }
            _unitOfWork.Categoria.Remove(categoria);
            await _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Categoria categoria)
        {
            if(ModelState.IsValid)
            {
                if(categoria.IdCategoria > 0)
                {
                    _unitOfWork.Categoria.Update(categoria);
                    TempData["exito"] = "Categoria actualizada con exito";
                }
                else
                {
                    await _unitOfWork.Categoria.Add(categoria);
                    TempData["exito"] = "Categoria agregada con exito";
                }
                await _unitOfWork.Save();
                return RedirectToAction("Index");
                
            }
            return View(categoria);
        }

    }
}
