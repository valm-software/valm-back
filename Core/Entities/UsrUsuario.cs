using Core.Interfaces;

namespace Core.Entities
{
    public class UsrUsuario : BaseEntity
    {
        public required string Usuario { get; set; }
        public required string Contraseña { get; set; }
        public ICollection<UsrUsuarioRol> UsrUsuarioRol { get; set; }

    }
}
