
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


        public override async Task<Producto> GetByIdAsync(int id)
        {
            return await _valmContext.Productos
                .Include(p => p.Marca)
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
        }


        public override async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _valmContext.Productos
                .Include(p => p.Marca)
                .Include(p => p.Categoria)
                .ToListAsync();
        }
    }
}
