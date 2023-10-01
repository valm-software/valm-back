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
        IAuthPermisoRepository AuthPermisos { get; }
        IAuthUsuarioRolRepository AuthUsuariosRoles { get; }
        IAuthRolPermisoRepository AuthRolesPermisos { get; }

        int Save();
    }
}
