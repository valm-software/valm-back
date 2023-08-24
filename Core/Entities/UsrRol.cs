using Core.Interfaces;

namespace Core.Entities 
{
    public class UsrRol : BaseEntity
    {
        public required string Nombre { get; set; }
        public string Descripcion { get; set; }

        public ICollection<UsrRolPrivilegio> UsrRolPrivilegio { get; set; }
        public ICollection<UsrUsuarioRol> UsrUsuarioRol { get; set; }

    }
}
