using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ValmContext _valmContext) :base(_valmContext) { 

        }

    }
}
