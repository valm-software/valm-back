using System.Collections.Generic;

namespace Core.Entities
{
    public class AuthRol : BaseEntity
    {
        public AuthRol()
        {
            AuthRolPermisos = new List<AuthRolPermiso>();
            AuthUsuarioRoles = new List<AuthUsuarioRol>();
        }

        public string Nombre { get; set; }
        public ICollection<AuthRolPermiso> AuthRolPermisos { get; set; }
        public ICollection<AuthUsuarioRol> AuthUsuarioRoles { get; set; }
    }
}
