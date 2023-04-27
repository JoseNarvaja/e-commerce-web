using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Categoria> categorias = await Task.Run( () => { return _unitOfWork.Categoria.GetAll(); });
            return View(categorias);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Categoria categoria = new Categoria();
            if(id != null | id > 0)
            {
                categoria = _unitOfWork.Categoria.GetFirstOrDefault((u => u.IdCategoria == id));
            }
            return View(categoria);
        }



        [HttpGet("/Admin/Categoria/Delete/{id:int?}")]
        public IActionResult Delete([FromRoute] int id)
        {
            Categoria categoria = _unitOfWork.Categoria.GetFirstOrDefault(u => u.IdCategoria == id);

            IEnumerable<Producto> productosAsociados = _unitOfWork.Producto.GetAll(u => u.IdCategoria == id);
            if(productosAsociados.Count() >= 1)
            {
                TempData["error"] = "No se puede eliminar una categoria si tiene productos asociados";
                return RedirectToAction("Index");
            }
            _unitOfWork.Categoria.Remove(categoria);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Categoria categoria)
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
                    _unitOfWork.Categoria.Add(categoria);
                    TempData["exito"] = "Categoria agregada con exito";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
                
            }
            return View(categoria);
        }

    }
}
