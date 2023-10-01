using System.Collections.Generic;

namespace Core.Entities
{
    public class AuthUsuario : BaseEntity
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Dni { get; set; }
        public ICollection<AuthUsuarioRol> AuthUsuarioRoles { get; set; }
    }
}
