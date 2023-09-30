using System.Collections.Generic;

namespace Core.Entities
{
    public class Auth_Usuario : BaseEntity
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Dni { get; set; }
        public ICollection<Auth_UsuarioRol> Auth_UsuarioRoles { get; set; }
    }
}
