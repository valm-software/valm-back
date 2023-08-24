using Core.Interfaces;

namespace Core.Entities
{
    public class UsrPrivilegio : BaseEntity
    {
        public required string Nombre { get; set; }
        public string Descripcion { get; set; }

        public ICollection<UsrRolPrivilegio> UsrRolPrivilegio { get; set; }

    }
}
