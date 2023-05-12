using ECommerceWeb.DataAccess.Data;
using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using ECommerceWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Policy;

namespace ECommerceWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.RolAdmin)]
    [Area("Admin")]
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _contexto;
        private readonly UserManager<IdentityUser> _userManager;
        public UsuarioController(AppDbContext contexto, UserManager<IdentityUser> userManager)
        {
            _contexto = contexto;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            IEnumerable<Usuario> usuarios = _contexto.Set<Usuario>();
            var userRoles = _contexto.UserRoles.ToList();
            var roles = _contexto.Roles.ToList();

            foreach(var usuario in usuarios)
            {
                string idRol = userRoles.FirstOrDefault(r => r.UserId == usuario.Id).RoleId;
                usuario.Rol = roles.FirstOrDefault(r => r.Id == idRol).Name;
            }

            return View(usuarios);
        }

        public IActionResult BloquearDesbloquear(string id)
        {
            var usuarioDb = _contexto.Usuario.FirstOrDefault(u => u.Id == id);

            if(usuarioDb.LockoutEnd!= null && usuarioDb.LockoutEnd > DateTime.Now)
            {
                usuarioDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                usuarioDb.LockoutEnd= DateTime.Now.AddYears(100);
            }

            _contexto.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Permisos(String id)
        {
            string roleId = _contexto.UserRoles.FirstOrDefault(r => r.UserId == id).RoleId;

            PermisosVM permisos = new PermisosVM()
            {
                Usuario = _contexto.Usuario.FirstOrDefault(u => u.Id == id),
                Roles = _contexto.Roles.Select(r => new SelectListItem
                    {
                        Text = r.Name,
                        Value = r.Name
                    })
            };

            permisos.Usuario.Rol = _contexto.Roles.FirstOrDefault(r => r.Id == roleId).Name;
            return View(permisos);
        }

        [HttpPost]
        public IActionResult Permisos(PermisosVM permisos)
        {
            string antiguoRolId = _contexto.UserRoles.FirstOrDefault(r => r.UserId == permisos.Usuario.Id).RoleId;
            string antiguoRol = _contexto.Roles.FirstOrDefault(r => r.Id == antiguoRolId).Name;

            if (!(permisos.Usuario.Rol == antiguoRol))
            {
                Usuario usuario = _contexto.Usuario.FirstOrDefault(u => u.Id == permisos.Usuario.Id);
                _userManager.RemoveFromRoleAsync(usuario, antiguoRol).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(usuario, permisos.Usuario.Rol).GetAwaiter().GetResult();
            }

            TempData["exito"] = "Permisos de usuario actualizados";
            return RedirectToAction(nameof(Index));
        }


    }
}
