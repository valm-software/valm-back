using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ValmContext _context;
        private IProductoRepository _productos;
        private IMarcaRepository _marcas;
        private ICategoriaRepository _categorias;
        private IAuthUsuarioRepository _authUsuarios;
        private IAuthRolRepository _authRoles;
        private IAuthPermisoRepository _authPermisos;
        private IAuthUsuarioRolRepository _authUsuariosRoles;
        private IAuthRolPermisoRepository _authRolesPermisos;
        private IDbContextTransaction _transaction;

        public UnitOfWork(ValmContext context)
        {
            _context = context;
        }





        public IAuthUsuarioRepository AuthUsuarios
        {
            get
            {
                return _authUsuarios ??= new AuthUsuarioRepository(_context);
            }
        }

        public IAuthRolRepository AuthRoles
        {
            get
            {
                return _authRoles ??= new AuthRolRepository(_context);
            }
        }

        public IAuthPermisoRepository AuthPermisos
        {
            get
            {
                return _authPermisos ??= new AuthPermisoRepository(_context);
            }
        }

        public IAuthUsuarioRolRepository AuthUsuariosRoles
        {
            get
            {
                return _authUsuariosRoles ??= new AuthUsuarioRolRepository(_context);
            }
        }

        //public IAuthRolPermisoRepository AuthRolesPermisos
        //{
        //    get
        //    {
        //        return _authRolesPermisos ??= new AuthRolPermisoRepository(_context);
        //    }
        //}


        public IAuthRolPermisoRepository AuthRolesPermisos
        {
            get
            {
                if (_authRolesPermisos == null)
                {
                     _authRolesPermisos = new AuthRolPermisoRepository(_context);
                }
                
                return _authRolesPermisos;

            }
        }

        public ICategoriaRepository Categorias
        {
            get
            {
                if (_categorias == null)
                {
                    _categorias = new CategoriaRepository(_context);
                }
                return _categorias;
            }
        }
        public IMarcaRepository Marcas
        {
            get
            {
                if (_marcas == null)
                {
                    _marcas = new MarcaRepository(_context);
                }
                return _marcas;
            }
        }
        public IProductoRepository Productos
        {
            get
            {
                if (_productos == null)
                {
                    _productos = new ProductoRepository(_context);
                }
                return _productos;
            }
        }
        
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
