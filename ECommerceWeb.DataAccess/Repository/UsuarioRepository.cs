using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Models;
using ECommerceWeb.DataAccess.Data;

namespace ECommerceWeb.DataAccess.Repository
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        private readonly AppDbContext _contexto;

        public UsuarioRepository(AppDbContext contexto) : base(contexto)
        {
            _contexto = contexto;
        }   

        public void Update(Usuario usuario)
        {
            _contexto.Usuario.Update(usuario);
        }
    }
}
