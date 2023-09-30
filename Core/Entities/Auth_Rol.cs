using System.Collections.Generic;

namespace Core.Entities
{
    public class Auth_Rol : BaseEntity
    {
        public Auth_Rol()
        {
            Auth_RolPermisos = new List<Auth_RolPermiso>();
            Auth_UsuarioRoles = new List<Auth_UsuarioRol>();
        }

        public string Nombre { get; set; }
        public ICollection<Auth_RolPermiso> Auth_RolPermisos { get; set; }
        public ICollection<Auth_UsuarioRol> Auth_UsuarioRoles { get; set; }
    }
}
