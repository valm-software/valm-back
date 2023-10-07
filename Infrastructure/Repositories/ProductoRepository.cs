﻿
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(ValmContext context) : base(context) { 
        
        }
        public async Task<IEnumerable<Producto>> GetProductosMasCaros(int cantidad) =>
            await  _valmContext.Productos
            .OrderByDescending(P => P.Precio)
            .Take(cantidad)
            .ToListAsync();


        public override async Task<Producto> GetByIdAsync(int id, bool noTracking = true)
        {
            var queryProducto = noTracking ? _valmContext.Productos.AsNoTracking()
                                            : _valmContext.Productos;

            return await queryProducto
                .Include(p => p.Marca)
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
        }


        public override async Task<IEnumerable<Producto>> GetAllAsync(bool noTracking = true)
        {
            var queryProducto = noTracking ? _valmContext.Productos.AsNoTracking()
                                : _valmContext.Productos;


            return await queryProducto
                .Include(p => p.Marca)
                .Include(p => p.Categoria)
                .ToListAsync();
        }


        public override async Task<(int totalRegistros, IEnumerable<Producto> registros)> GetAllAsync(int pageIndex, int pageSize, string search, bool noTracking = true)
        {

            var queryProducto = noTracking ? _valmContext.Productos.AsNoTracking()
                                : _valmContext.Productos;

            if (!String.IsNullOrEmpty(search))
            {
                queryProducto = queryProducto.Where(p => p.Nombre.ToLower().Contains(search));
            }

            var totalRegistros = await queryProducto
                                    .CountAsync();
            var registros = await queryProducto
                                .Include(p => p.Marca)
                                .Include(p => p.Categoria)
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            return (totalRegistros, registros);

        }

    }
}
