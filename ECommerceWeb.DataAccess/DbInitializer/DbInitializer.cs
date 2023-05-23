using ECommerceWeb.DataAccess.Data;
using ECommerceWeb.Models;
using ECommerceWeb.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if(_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex) { }

            if (!_roleManager.RoleExistsAsync(SD.RolAdmin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.RolAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.RolModerador)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.RolCliente)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new Usuario
                {
                    UserName = "AdminTest@admin.com",
                    Nombre = "Admin",
                    Apellido = "Admin",
                    Email = "adminTest@admin.com"
                }, "Admintest1#").GetAwaiter().GetResult();

                Usuario admin = _context.Usuario.FirstOrDefault(u => u.Email == "adminTest@admin.com");
                _userManager.AddToRoleAsync(admin, SD.RolAdmin).GetAwaiter().GetResult();
            }

            if (_context.Carousel.FirstOrDefault(c => c.Nombre == "Primera") == null)
            {
                Carousel primera = new Carousel()
                {
                    Nombre = "Primera",
                    Titulo = "Completar",
                    Descripcion = "Completar"
                };
                Carousel segunda = new Carousel()
                {
                    Nombre = "Segunda",
                    Titulo = "Completar",
                    Descripcion = "Completar"
                };
                Carousel tercera = new Carousel()
                {
                    Nombre = "Tercera",
                    Titulo = "Completar",
                    Descripcion = "Completar"
                };
                _context.Carousel.AddRange(primera, segunda, tercera);
                _context.SaveChanges();

            }

            return;
        }
    }


}
