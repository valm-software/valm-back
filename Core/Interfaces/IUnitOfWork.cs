namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IProductoRepository Productos {  get; }
        IMarcaRepository Marcas { get; }
        ICategoriaRepository Categorias { get; }

        // interfaces para Auth_XXX
        IAuthUsuarioRepository AuthUsuarios { get; }
        IAuthRolRepository AuthRoles { get; }
        IAuthPoliticaRepository AuthPermisos { get; }
        IAuthUsuarioRolRepository AuthUsuariosRoles { get; }
        IAuthRolPoliticaRepository AuthRolesPermisos { get; }

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        Task<int> SaveAsync();
    }
}
