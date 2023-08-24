using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class MarcaRepository : GenericRepository<Marca>, IMarcaRepository
    {
        public MarcaRepository(ValmContext _valmContext) : base( _valmContext) { 

        }
    }
}
