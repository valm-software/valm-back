namespace Core.Entities
{
    public class Auth_RolPermiso
    {
        public int RolId { get; set; }
        public Auth_Rol Auth_Rol { get; set; }

        public int PermisoId { get; set; }
        public Auth_Permiso Auth_Permiso { get; set; }
    }
}
