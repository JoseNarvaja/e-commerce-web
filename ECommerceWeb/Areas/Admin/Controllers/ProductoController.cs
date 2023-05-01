using ECommerceWeb.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using ECommerceWeb.DataAccess.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using ECommerceWeb.Utility;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.RolAdmin)]
    public class ProductoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnviroment;

        public ProductoController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnviroment)
        {
            _unitOfWork = unitOfWork;
            _hostEnviroment= hostEnviroment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Producto> productos = await Task.Run(() => { return _unitOfWork.Producto.GetAll(includeProperties:"Categoria"); });
            return View(productos);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            var productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                Categorias = _unitOfWork.Categoria.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Nombre,
                    Value = u.IdCategoria.ToString()
                })
            };

            if(id != null & id != 0)
            {
                productoVM.Producto = _unitOfWork.Producto.GetFirstOrDefault(u => u.IdProducto == id);
            }
            return View(productoVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductoVM productoVM, IFormFile? file)
        {
            if(productoVM.Producto.PrecioDescuento > productoVM.Producto.Precio)
            {
                ModelState.AddModelError("Producto.PrecioDescuento", "El precio de descuento no puede ser mayor al precio corriente");
            }
            if(ModelState.IsValid)
            {
                string wwwRootPath = _hostEnviroment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\productos");
                    var extension = Path.GetExtension(file.FileName);

                    if(productoVM.Producto.URLImagen!= null)
                    {
                        var oldPath = Path.Combine(wwwRootPath, productoVM.Producto.URLImagen.TrimStart('\\'));
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    productoVM.Producto.URLImagen = @"\images\productos\" + fileName + extension;

                }
               if(productoVM.Producto.IdProducto == 0)
                {
                    _unitOfWork.Producto.Add(productoVM.Producto);
                    TempData["exito"] = "Producto creado con éxito";
                }
                else
                {
                    _unitOfWork.Producto.Update(productoVM.Producto);
                    TempData["exito"] = "Producto actualizado con éxito";
                }
                _unitOfWork.Save();
               return RedirectToAction("Index");
            }

            productoVM.Categorias = _unitOfWork.Categoria.GetAll().Select(u => new SelectListItem
            {
                Text = u.Nombre,
                Value = u.IdCategoria.ToString()
            });
            return View(productoVM);
        }

        [HttpGet("/Admin/Producto/Delete/{id:int?}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var producto = _unitOfWork.Producto.GetFirstOrDefault(u => u.IdProducto == id);
            if(producto == null)
            {
                TempData["error"] = "No se pudo borrar el producto";
                return RedirectToAction("Index");
            }

            var imagenProducto = Path.Combine(_hostEnviroment.WebRootPath, producto.URLImagen.TrimStart('\\'));
            if (System.IO.File.Exists(imagenProducto))
            {
                System.IO.File.Delete(imagenProducto);
            }
            _unitOfWork.Producto.Remove(producto);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}
