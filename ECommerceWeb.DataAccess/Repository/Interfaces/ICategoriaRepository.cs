using ECommerceWeb.Models;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.DataAccess.Repository.Interfaces
{
    public interface ICategoriaRepository :IRepository<Categoria>
    {
        void Update(Categoria obj);
    }
}
