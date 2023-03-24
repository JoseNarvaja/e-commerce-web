using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceWeb.DataAccess.Data;
using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Models;

namespace ECommerceWeb.DataAccess.Repository
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        private readonly AppDbContext _context;

        public ProductoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


        public void Update(Producto obj)
        {
            var objDB = _context.Producto.FirstOrDefault(u => u.IdProducto == obj.IdProducto);
            if(objDB != null)
            {
                objDB.Descripcion = obj.Descripcion;
                objDB.Nombre= obj.Nombre;
                objDB.Marca= obj.Marca;
                objDB.Precio= obj.Precio;
                objDB.PrecioDescuento= obj.PrecioDescuento;
                objDB.PorcentajeDescuento = obj.PorcentajeDescuento;
                objDB.IdCategoria= obj.IdCategoria;
                if(obj.URLImagen != null)
                {
                    objDB.URLImagen = obj.URLImagen;
                }
            }
            return;
        }
    }
}
