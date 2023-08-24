using Core.Interfaces;

namespace Core.Entities
{
    public class UsrUsuarioRol : BaseEntity
    {
        public int IdUsuario { get; set; }
        public UsrUsuario UsrUsuario { get; set; }

        public int IdRol { get; set; }
        public UsrRol UsrRol { get; set; }

    }
}
