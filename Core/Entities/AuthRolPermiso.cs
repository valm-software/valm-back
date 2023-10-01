namespace Core.Entities
{
    public class AuthRolPermiso
    {
        public int RolId { get; set; }
        public AuthRol AuthRol { get; set; }

        public int PermisoId { get; set; }
        public AuthPermiso AuthPermiso { get; set; }
    }
}
