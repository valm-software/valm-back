using System.Collections.Generic;

namespace Core.Entities
{
    public class AuthRol : BaseEntity
    {
        public AuthRol()
        {
            AuthRolPoliticas = new List<AuthRolPolitica>();
            AuthUsuarioRoles = new List<AuthUsuarioRol>();
        }

        public string NombreRol { get; set; }
        public ICollection<AuthRolPolitica> AuthRolPoliticas { get; set; }
        public ICollection<AuthUsuarioRol> AuthUsuarioRoles { get; set; }
    }
}
